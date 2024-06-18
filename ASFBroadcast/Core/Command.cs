using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using ASFBroadcast.Boardcast;
using System.Collections.Concurrent;

namespace ASFBroadcast.Core;

internal static class Command
{
    /// <summary>
    /// 直播观看处理器
    /// </summary>
    internal static ConcurrentDictionary<Bot, WatchHandler> WatchHandlers { get; private set; } = new();

    internal static async Task<string?> ResponseTest()
    {
        var bot = Bot.GetBot("hk");
        if (bot == null)
            return Strings.BotNotFound;

        if (!bot.IsConnectedAndLoggedOn)
            return bot.FormatBotResponse(Strings.BotNotConnected);

        ulong targetSteamId = 76561199560266928;

        var a = await WebRequest.GetBoardCastMpd(bot, targetSteamId).ConfigureAwait(false);

        if (!long.TryParse(a.BroadcastId, out var broadcastId) || !long.TryParse(a.ViewerToken, out var viewerToken))
        {
            return "failed";
        }

        var b = await WebRequest.GetBroadCastInfo(bot, targetSteamId, broadcastId).ConfigureAwait(false);
        var c = await WebRequest.SendHeartBeat(bot, targetSteamId, broadcastId, viewerToken).ConfigureAwait(false);

        return "done";
    }


    ///// <summary>
    ///// 退出指定群组
    ///// </summary>
    ///// <param name="bot"></param>
    ///// <returns></returns>
    //internal static async Task<string?> ResponseLeaveGroup(Bot bot, string groupID)
    //{
    //    if (!bot.IsConnectedAndLoggedOn)
    //        return bot.FormatBotResponse(Strings.BotNotConnected);

    //    if (!ulong.TryParse(groupID, out var intGroupID))
    //        return bot.FormatBotResponse(string.Format(Langs.ArgumentNotInteger, nameof(groupID)));

    //    bool result = await WebRequest.LeaveGroup(bot, intGroupID).ConfigureAwait(false);

    //    return bot.FormatBotResponse(string.Format(Langs.LeaveGroup, result ? Langs.Success : Langs.Failure));
    //}

    ///// <summary>
    ///// 退出指定群组 (多个Bot)
    ///// </summary>
    ///// <param name="botNames"></param>
    ///// <returns></returns>
    ///// <exception cref="ArgumentNullException"></exception>
    //internal static async Task<string?> ResponseLeaveGroup(string botNames, string groupID)
    //{
    //    if (string.IsNullOrEmpty(botNames))
    //        throw new ArgumentNullException(nameof(botNames));

    //    var bots = Bot.GetBots(botNames);

    //    if (bots == null || bots.Count == 0)
    //        return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));

    //    var results = await Utilities.InParallel(bots.Select(bot => ResponseLeaveGroup(bot, groupID))).ConfigureAwait(false);

    //    List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

    //    return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    //}

    ///// <summary>
    ///// 获取群组列表
    ///// </summary>
    ///// <param name="bot"></param>
    ///// <returns></returns>
    //internal static async Task<string?> ResponseGroupList(Bot bot)
    //{
    //    if (!bot.IsConnectedAndLoggedOn)
    //        return bot.FormatBotResponse(Strings.BotNotConnected);

    //    string? result = await WebRequest.GetGroupList(bot).ConfigureAwait(false);

    //    return result != null ? bot.FormatBotResponse(result) : null;
    //}

    ///// <summary>
    ///// 获取群组列表 (多个Bot)
    ///// </summary>
    ///// <param name="botNames"></param>
    ///// <returns></returns>
    ///// <exception cref="ArgumentNullException"></exception>
    //internal static async Task<string?> ResponseGroupList(string botNames)
    //{
    //    if (string.IsNullOrEmpty(botNames))
    //        throw new ArgumentNullException(nameof(botNames));

    //    var bots = Bot.GetBots(botNames);

    //    if (bots == null || bots.Count == 0)
    //        return FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));

    //    var results = await Utilities.InParallel(bots.Select(bot => ResponseGroupList(bot))).ConfigureAwait(false);

    //    List<string> responses = new(results.Where(result => !string.IsNullOrEmpty(result))!);

    //    return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    //}
}
