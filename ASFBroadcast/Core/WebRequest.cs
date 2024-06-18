using ArchiSteamFarm.Steam;
using ASFBroadcast.Data;

namespace ASFBroadcast.Boardcast;

internal static class WebRequest
{
    /// <summary>
    /// 离开群组
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="GroupID"></param>
    /// <returns></returns>
    internal static async Task<GetBroadCastMpdResponse?> GetBoardCastMpd(Bot bot, ulong steamId)
    {
        var request = new Uri(SteamCommunityURL, $"/broadcast/getbroadcastmpd/?steamid={steamId}&broadcastid=0&watchlocation=5");
        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<GetBroadCastMpdResponse>(request, referer: SteamStoreURL).ConfigureAwait(false);
        return response?.Content;
    }

    internal static async Task<GetBroadCastInfoResponse?> GetBroadCastInfo(Bot bot, ulong steamId, long broadcastId)
    {
        var request = new Uri(SteamCommunityURL, $"/broadcast/getbroadcastinfo/?steamid={steamId}&broadcastid={broadcastId}&location=5");
        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<GetBroadCastInfoResponse>(request, referer: SteamStoreURL).ConfigureAwait(false);
        return response?.Content;
    }

    internal static async Task<BaseResultResponse?> SendHeartBeat(Bot bot, ulong steamId, long broadcastId, long viewerToken)
    {
        var request = new Uri(SteamCommunityURL, "/broadcast/heartbeat/");

        var data = new Dictionary<string, string>
        {
            { "steamid", steamId.ToString() },
            { "broadcastid", broadcastId.ToString() },
            { "viewertoken", viewerToken.ToString() }
        };

        var response = await bot.ArchiWebHandler.UrlPostToJsonObjectWithSession<BaseResultResponse>(request, data: data, referer: SteamStoreURL).ConfigureAwait(false);
        return response?.Content;
    }
}
