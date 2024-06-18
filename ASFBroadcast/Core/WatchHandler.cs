using ArchiSteamFarm.Steam;

namespace ASFBroadcast.Boardcast;
internal sealed class WatchHandler(Bot bot)
{
    public string? BroadcastName { get; private set; }
    public string? BroadcastId { get; private set; }

}
