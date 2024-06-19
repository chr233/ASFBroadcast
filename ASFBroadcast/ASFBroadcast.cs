using ArchiSteamFarm.Core;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ASFBroadcast.Boardcast;
using ASFBroadcast.Core;
using ASFBroadcast.Data;
using ASFBroadcast.Localization;

using System.ComponentModel;
using System.Composition;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace ASFBroadcast;

[Export(typeof(IPlugin))]
internal sealed class ASFBroadcast : IASF, IBot, IBotConnection, IBotCommand2
{
    public string Name => "ASF Broadcast";
    public Version Version => MyVersion;

    private bool ASFEBridge;

    public static PluginConfig Config => Utils.Config;

    private Timer? StatisticTimer { get; set; }

    /// <inheritdoc/>
    public Task OnASFInit(IReadOnlyDictionary<string, JsonElement>? additionalConfigProperties = null)
    {
        PluginConfig? config = null;

        if (additionalConfigProperties != null)
        {
            foreach (var (configProperty, configValue) in additionalConfigProperties)
            {
                if (configProperty == "ASFEnhance" && configValue.ValueKind == JsonValueKind.Object)
                {
                    try
                    {
                        config = configValue.Deserialize<PluginConfig>();
                        if (config != null)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ASFLogger.LogGenericException(ex);
                    }
                }
            }
        }

        Utils.Config = config ?? new();

        var sb = new StringBuilder();

        //使用协议
        if (!Config.EULA)
        {
            sb.AppendLine(Langs.Line);
            sb.AppendLineFormat(Langs.EulaWarning, Name);
            sb.AppendLine(Langs.Line);
        }

        if (sb.Length > 0)
        {
            sb.Insert(0, "\r\n");
            ASFLogger.LogGenericWarning(sb.ToString());
        }

        if (!Config.EULA)
        {
            return Task.CompletedTask;
        }

        //统计
        if (Config.Statistic && !ASFEBridge)
        {
            var request = new Uri("https://asfe.chrxw.com/ASFBroadcast");
            StatisticTimer = new Timer(
                async (_) =>
                {
                    await ASF.WebBrowser!.UrlGetToHtmlDocument(request).ConfigureAwait(false);
                },
                null,
                TimeSpan.FromSeconds(30),
                TimeSpan.FromHours(24)
            );
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task OnLoaded()
    {
        ASFLogger.LogGenericInfo(Langs.PluginContact);
        ASFLogger.LogGenericInfo(Langs.PluginInfo);

        var flag = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        var handler = typeof(ASFBroadcast).GetMethod(nameof(ResponseCommand), flag);

        const string pluginId = nameof(ASFBroadcast);
        const string cmdPrefix = "AAT";
        const string? repoName = null;

        ASFEBridge = AdapterBridge.InitAdapter(Name, pluginId, cmdPrefix, repoName, handler);

        if (ASFEBridge)
        {
            ASFLogger.LogGenericDebug(Langs.ASFEnhanceRegisterSuccess);
        }
        else
        {
            ASFLogger.LogGenericInfo(Langs.ASFEnhanceRegisterFailed);
            ASFLogger.LogGenericWarning(Langs.PluginStandalongMode);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 获取插件信息
    /// </summary>
    private static string? PluginInfo => string.Format("{0} {1}", nameof(ASFBroadcast), MyVersion);

    /// <summary>
    /// 处理命令
    /// </summary>
    /// <param name="access"></param>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private static Task<string?>? ResponseCommand(Bot bot, EAccess access, string cmd, string[] args)
    {
        int argLength = args.Length;

        return argLength switch
        {
            0 => throw new InvalidOperationException(nameof(args)),
            1 => cmd switch  //不带参数
            {
                //Plugin Info
                "ASFBROADCAST" or
                "ABC" when access >= EAccess.FamilySharing =>
                    Task.FromResult(PluginInfo),

                "STOPWATCH" or
                "SWATCH" when access >= EAccess.Master =>
                    Command.ResponseStopWatching(bot),

                "WATCHSTATUS" or
                "WS" when access >= EAccess.Master =>
                    Command.ResponseGetWatchStatus(bot),

                _ => null,
            },
            _ => cmd switch //带参数
            {
                "BROADCASTINFO" or
                "BINFO" when access >= EAccess.Master =>
                    Command.ResponseGetBroadcastInfo(Utilities.GetArgsAsText(args, 1, ",")),

                "WATCH" when argLength > 2 && access >= EAccess.Master =>
                    Command.ResponseStartWatching(SkipBotNames(args, 0, 1), args.Last()),
                "WATCH" when access >= EAccess.Master =>
                    Command.ResponseStartWatching(bot, args[1]),

                "STOPWATCH" or
                "SWATCH" when access >= EAccess.Master =>
                    Command.ResponseStopWatching(Utilities.GetArgsAsText(args, 1, ",")),

                "WATCHSTATUS" or
                "WS" when access >= EAccess.Master =>
                    Command.ResponseGetWatchStatus(Utilities.GetArgsAsText(args, 1, ",")),

                _ => null,
            }
        };
    }

    /// <inheritdoc/>
    public async Task<string?> OnBotCommand(Bot bot, EAccess access, string message, string[] args, ulong steamId = 0)
    {
        if (ASFEBridge || !Config.EULA)
        {
            return null;
        }

        if (!Enum.IsDefined(access))
        {
            throw new InvalidEnumArgumentException(nameof(access), (int)access, typeof(EAccess));
        }

        try
        {
            var cmd = args[0].ToUpperInvariant();

            if (cmd.StartsWith("ABC."))
            {
                cmd = cmd[4..];
            }

            var task = ResponseCommand(bot, access, cmd, args);
            if (task != null)
            {
                return await task.ConfigureAwait(false);
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(500).ConfigureAwait(false);
                ASFLogger.LogGenericException(ex);
            }).ConfigureAwait(false);

            return ex.StackTrace;
        }
    }

    /// <inheritdoc/>
    public Task OnBotDestroy(Bot bot)
    {
        Command.WatchHandlers.TryRemove(bot, out var _);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task OnBotInit(Bot bot)
    {
        var botHandler = new WatchHandler(bot);
        Command.WatchHandlers.TryAdd(bot, botHandler);

        if (Config.DisableFarming)
        {
            return bot.Actions.Pause(true);
        }
        else
        {
            return Task.CompletedTask;
        }
    }

    /// <inheritdoc/>
    public Task OnBotDisconnected(Bot bot, SteamKit2.EResult reason)
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task OnBotLoggedOn(Bot bot)
    {
        if (Config.DisableFarming)
        {
            return bot.Actions.Pause(true);
        }
        else
        {
            return Task.CompletedTask;
        }
    }
}
