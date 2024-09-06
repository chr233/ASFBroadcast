using ArchiSteamFarm.Steam;
using ASFBroadcast.Data;

namespace ASFBroadcast.Boardcast;

internal static class WebRequest
{
    internal static async Task<GetBroadCastMpdResponse?> GetBoardCastMpd(Bot bot, ulong steamId)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();
        var request = new Uri(SteamCommunityURL, $"/broadcast/getbroadcastmpd/?access_token={token}&steamid={steamId}&broadcastid=0&watchlocation=5");
        var response = await bot.ArchiWebHandler.WebBrowser.UrlGetToJsonObject<GetBroadCastMpdResponse>(request, referer: SteamStoreURL).ConfigureAwait(false);
        return response?.Content;
    }

    internal static async Task<GetBroadCastInfoResponse?> GetBroadCastInfo(Bot bot, ulong steamId, ulong broadcastId)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();
        var request = new Uri(SteamCommunityURL, $"/broadcast/getbroadcastinfo/?access_token={token}&steamid={steamId}&broadcastid={broadcastId}&location=5");
        var response = await bot.ArchiWebHandler.WebBrowser.UrlGetToJsonObject<GetBroadCastInfoResponse>(request, referer: SteamStoreURL).ConfigureAwait(false);
        return response?.Content;
    }

    internal static async Task<BaseResultResponse?> SendHeartBeat(Bot bot, ulong steamId, ulong broadcastId, ulong viewerToken)
    {
        var request = new Uri(SteamCommunityURL, "/broadcast/heartbeat/");
        var token = bot.AccessToken ?? throw new AccessTokenNullException();
        var data = new Dictionary<string, string>
        {
            { "steamid", steamId.ToString() },
            { "broadcastid", broadcastId.ToString() },
            { "viewertoken", viewerToken.ToString() },
            { "access_token", token },
        };

        var response = await bot.ArchiWebHandler.WebBrowser.UrlPostToJsonObject<BaseResultResponse, Dictionary<string, string>>(request, referer: SteamStoreURL).ConfigureAwait(false);
        return response?.Content;
    }
}
