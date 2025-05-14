using System.Text.Json.Serialization;

namespace ASFBroadcast.Data;

internal sealed record GetBroadCastInfoResponse
{
    [JsonPropertyName("success")]
    public int Success { get; set; }

    [JsonPropertyName("appid")]
    public string? AppId { get; set; }

    [JsonPropertyName("app_title")]
    public string? AppTitle { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("viewer_count")]
    public int ViewerCount { get; set; }

    [JsonPropertyName("permission")]
    public int Permission { get; set; }

    [JsonPropertyName("is_rtmp")]
    public int IsRtmp { get; set; }

    [JsonPropertyName("seconds_delay")]
    public int SecondsDelay { get; set; }

    [JsonPropertyName("is_publisher")]
    public int IsPublisher { get; set; }

    [JsonPropertyName("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    [JsonPropertyName("update_interval")]
    public int UpdateInterval { get; set; }

    [JsonPropertyName("is_online")]
    public bool IsOnline { get; set; }

    [JsonPropertyName("is_replay")]
    public int IsReplay { get; set; }
}

