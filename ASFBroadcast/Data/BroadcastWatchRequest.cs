using System.ComponentModel.DataAnnotations;

namespace ASFBroadcast.Data;

/// <summary>
/// 直播观众数
/// </summary>
public sealed record BroadcastWatchRequest
{
    /// <summary>
    /// 直播间ID
    /// </summary>
    [Required]
    public ulong SteamId { get; set; }

    /// <summary>
    /// 在线时长（秒）
    /// </summary>
    [Required]
    public ulong Seconds { get; set; }
}
