
using ArchiSteamFarm.Core;
using ArchiSteamFarm.NLog;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Integration;
using ArchiSteamFarm.Steam.Interaction;
using ASFBroadcast.Data;
using System.Text;

namespace ASFBroadcast;

internal static class Utils
{
    /// <summary>
    /// 插件配置
    /// </summary>
    internal static PluginConfig Config { get; set; } = new();

    /// <summary>
    /// 格式化返回文本
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static string FormatStaticResponse(string message)
    {
        return $"<ASF> {message}";
    }

    /// <summary>
    /// 格式化返回文本
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    internal static string FormatStaticResponse(string message, params object?[] args)
    {
        return FormatStaticResponse(string.Format(message, args));
    }

    /// <summary>
    /// 格式化返回文本
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    internal static string FormatBotResponse(this Bot bot, string message)
    {
        return $"<{bot.BotName}> {message}";
    }

    /// <summary>
    /// 格式化返回文本
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    internal static string FormatBotResponse(this Bot bot, string message, params object?[] args)
    {
        return bot.FormatBotResponse(string.Format(message, args));
    }

    internal static StringBuilder AppendLineFormat(this StringBuilder sb, string format, params object?[] args)
    {
        return sb.AppendLine(string.Format(format, args));
    }

    /// <summary>
    /// 获取Bot SessionID
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    internal static string? GetBotSessionID(this Bot bot)
    {
        var sessionID = bot.ArchiWebHandler.WebBrowser.CookieContainer.GetCookieValue(SteamStoreURL, "sessionid");

        if (string.IsNullOrEmpty(sessionID))
        {
            bot.ArchiLogger.LogNullError(nameof(sessionID));
            return null;
        }

        return sessionID;
    }

    /// <summary>
    /// 转换SteamID
    /// </summary>
    /// <param name="steamID"></param>
    /// <returns></returns>
    internal static ulong SteamID2Steam32(ulong steamID)
    {
        return steamID - 0x110000100000000;
    }

    /// <summary>
    /// 获取版本号
    /// </summary>
    internal static Version MyVersion => typeof(ASFBroadcast).Assembly.GetName().Version;

    /// <summary>
    /// 获取插件所在路径
    /// </summary>
    internal static string MyLocation => typeof(ASFBroadcast).Assembly.Location;

    /// <summary>
    /// Steam商店链接
    /// </summary>
    internal static Uri SteamStoreURL => ArchiWebHandler.SteamStoreURL;

    /// <summary>
    /// Steam社区链接
    /// </summary>
    internal static Uri SteamCommunityURL => ArchiWebHandler.SteamCommunityURL;

    /// <summary>
    /// Steam客服链接
    /// </summary>
    internal static Uri SteamHelpURL => ArchiWebHandler.SteamHelpURL;

    /// <summary>
    /// 日志
    /// </summary>
    internal static ArchiLogger ASFLogger => ASF.ArchiLogger;
}
