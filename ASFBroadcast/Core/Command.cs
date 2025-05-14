using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFBroadcast.Broadcast;
using ASFBroadcast.Localization;
using System.Collections.Concurrent;
using System.Text;

namespace ASFBroadcast.Core;

internal static class Command
{
    /// <summary>
    /// 直播观看处理器
    /// </summary>
    internal static ConcurrentDictionary<Bot, WatchHandler> WatchHandlers { get; private set; } = new();

    /// <summary>
    /// 获取直播信息
    /// </summary>
    /// <param name="broadcastUrls"></param>
    /// <returns></returns>
    internal static async Task<string?> ResponseGetBroadcastInfo(string broadcastUrls)
    {
        var bots = Bot.GetBots("ASF");

        Bot? bot = null;
        if (bots != null)
        {
            foreach (var b in bots)
            {
                if (b.IsConnectedAndLoggedOn)
                {
                    bot = b;
                    break;
                }
            }
        }

        if (bot == null)
        {
            return FormatStaticResponse("无可用机器人");
        }

        var urls = broadcastUrls.Split(SeparatorDot, StringSplitOptions.RemoveEmptyEntries);

        var sb = new StringBuilder();

        foreach (var url in urls)
        {
            var match = RegexUtils.MatchBroadcastSteamId().Match(url);

            if (!match.Success || !ulong.TryParse(match.Groups[1].Value, out var targetSteamId))
            {
                sb.AppendLineFormat(FormatStaticResponse(Langs.TwoItem, url, "参数错误, 支持直播链接或者SteamID"));
                continue;
            }

            var broadcastMpd = await WebRequest.GetBoardCastMpd(bot, targetSteamId).ConfigureAwait(false);
            if (broadcastMpd == null || !ulong.TryParse(broadcastMpd.BroadcastId, out var broadcastId))
            {
                sb.AppendLineFormat(FormatStaticResponse(Langs.TwoItem, targetSteamId, "未找到直播, 可能未开启直播"));
                continue;
            }

            var broadcastInfo = await WebRequest.GetBroadCastInfo(bot, targetSteamId, broadcastId).ConfigureAwait(false);
            if (broadcastInfo == null)
            {
                sb.AppendLine(FormatStaticResponse(Langs.TwoItem, targetSteamId, "网络问题, 读取直播信息失败"));
            }
            else
            {
                var title = string.IsNullOrEmpty(broadcastInfo.Title) ? broadcastInfo.Title : broadcastInfo.AppTitle;
                sb.AppendLine(FormatStaticResponse(Langs.TwoItem, targetSteamId, string.Format("直播间: {0} 标题: {1} [{2}] 人数: {3}", targetSteamId, title, broadcastInfo.AppId, broadcastInfo.ViewerCount)));
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 开始观看直播
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="broadcastUrl"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseStartWatching(Bot bot, string broadcastUrl)
    {
        if (string.IsNullOrEmpty(broadcastUrl))
        {
            throw new ArgumentNullException(nameof(broadcastUrl));
        }

        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!WatchHandlers.TryGetValue(bot, out var handler))
        {
            return bot.FormatBotResponse("内部错误");
        }

        var match = RegexUtils.MatchBroadcastSteamId().Match(broadcastUrl);
        if (!match.Success || !ulong.TryParse(match.Groups[1].Value, out var watchSteamId))
        {
            return bot.FormatBotResponse(Langs.TwoItem, broadcastUrl, "参数错误, 支持直播链接或者SteamID");
        }

        var broadcastMpd = await WebRequest.GetBoardCastMpd(bot, watchSteamId).ConfigureAwait(false);
        if (broadcastMpd == null || !ulong.TryParse(broadcastMpd.BroadcastId, out var broadcastId))
        {
            return bot.FormatBotResponse(Langs.TwoItem, watchSteamId, "未找到直播, 可能未开启直播");
        }

        var broadcastInfo = await WebRequest.GetBroadCastInfo(bot, watchSteamId, broadcastId).ConfigureAwait(false);
        if (broadcastInfo == null)
        {
            return bot.FormatBotResponse(Langs.TwoItem, watchSteamId, "网络问题, 读取直播信息失败");
        }
        else
        {
            var result = handler.StartWatchBroadcast(watchSteamId, broadcastMpd, broadcastInfo);
            return bot.FormatBotResponse(result);
        }
    }

    /// <summary>
    /// 开始观看直播 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="broadcastUrl"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseStartWatching(string botNames, string broadcastUrl)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        if (string.IsNullOrEmpty(broadcastUrl))
        {
            throw new ArgumentNullException(nameof(broadcastUrl));
        }

        var bots = Bot.GetBots(botNames);
        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseStartWatching(bot, broadcastUrl))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return string.Join(Environment.NewLine, responses);
    }

    /// <summary>
    /// 停止观看直播
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseStopWatching(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!WatchHandlers.TryGetValue(bot, out var handler))
        {
            return bot.FormatBotResponse("内部错误");
        }

        await Task.Delay(100).ConfigureAwait(false);

        var result = handler.StopWatchBroadcast();
        return bot.FormatBotResponse(result);
    }

    /// <summary>
    /// 停止观看直播 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseStopWatching(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);
        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseStopWatching(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));
        return string.Join(Environment.NewLine, responses);
    }

    /// <summary>
    /// 获取观看状态
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetWatchStatus(Bot bot)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        if (!WatchHandlers.TryGetValue(bot, out var handler))
        {
            return bot.FormatBotResponse("内部错误");
        }

        await Task.Delay(100).ConfigureAwait(false);

        if (handler.IsWatching)
        {
            return bot.FormatBotResponse(handler.BroadcastSummary ?? "未知状态");
        }
        else
        {
            return bot.FormatBotResponse("闲置中");
        }
    }

    /// <summary>
    /// 获取观看状态 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseGetWatchStatus(string botNames)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);
        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseGetWatchStatus(bot))).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));
        return string.Join(Environment.NewLine, responses);
    }
}
