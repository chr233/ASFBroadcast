using ArchiSteamFarm.Steam;
using ASFBroadcast.Data;

namespace ASFBroadcast.Broadcast;
internal sealed class WatchHandler(Bot bot)
{
    public ulong WatchSteamId { get; set; }
    public ulong BroadcastId { get; private set; }
    public ulong ViewerToken { get; private set; }
    public string? BroadcastSummary { get; private set; }

    public Timer? HeartbeatTimer { get; set; }
    public Timer? SummaryTimer { get; set; }

    public bool IsWatching => HeartbeatTimer != null;

    public async void SendHeartbeatCallback(object? _)
    {
        if (bot.IsConnectedAndLoggedOn)
        {
            await WebRequest.SendHeartBeat(bot, WatchSteamId, BroadcastId, ViewerToken).ConfigureAwait(false);
        }
    }

    public async void UpdateSummaryCallback(object? _)
    {
        if (bot.IsConnectedAndLoggedOn)
        {
            var info = await WebRequest.GetBroadCastInfo(bot, WatchSteamId, BroadcastId).ConfigureAwait(false);

            if (info != null)
            {
                var title = $"[{info.AppId}] {info.Title ?? info.AppTitle}";
                BroadcastSummary = string.Format("正在观看直播间: {0} 标题: {1} [{2}] 人数: {3}", WatchSteamId, title, info.AppId, info.ViewerCount);
            }
        }
    }

    public string StartWatchBroadcast(ulong watchSteamId, GetBroadCastMpdResponse mpd, GetBroadCastInfoResponse info)
    {
        if (!ulong.TryParse(mpd.BroadcastId, out var broadcastId) || !ulong.TryParse(mpd.ViewerToken, out var viewerToken))
        {
            return "观看直播失败, 参数错误";
        }
        WatchSteamId = watchSteamId;
        BroadcastId = broadcastId;
        ViewerToken = viewerToken;

        var title = $"[{info.AppId}] {info.Title ?? info.AppTitle}";
        BroadcastSummary = string.Format("正在观看直播间: {0} 标题: {1} [{2}] 人数: {3}", watchSteamId, title, info.AppId, info.ViewerCount);

        HeartbeatTimer?.Dispose();
        HeartbeatTimer = new Timer(SendHeartbeatCallback, null, TimeSpan.FromSeconds(Config.HeartbeatInterval), TimeSpan.FromSeconds(10));

        SummaryTimer?.Dispose();
        if (Config.SummaryInterval > 0)
        {
            SummaryTimer = new Timer(UpdateSummaryCallback, null, TimeSpan.FromSeconds(Config.SummaryInterval), TimeSpan.FromSeconds(180));
        }

        return BroadcastSummary;
    }

    public string StopWatchBroadcast()
    {
        HeartbeatTimer?.Dispose();
        SummaryTimer?.Dispose();

        if (!IsWatching)
        {
            return "未在观看直播";
        }

        WatchSteamId = 0;
        BroadcastId = 0;
        ViewerToken = 0;
        BroadcastSummary = null;
        return "已停止观看直播";
    }
}
