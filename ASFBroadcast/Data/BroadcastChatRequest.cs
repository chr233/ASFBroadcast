using System.ComponentModel.DataAnnotations;

namespace ASFBroadcast.Data;

/// <summary>
/// 直播弹幕
/// </summary>
public sealed record BroadcastChatRequest
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
    public string? Message { get; set; }
}