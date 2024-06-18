using System.Text.Json.Serialization;

namespace ASFBroadcast.Data;
public sealed record GetBroadCastMpdResponse
{
    [JsonPropertyName("success")]
    public string? Success { get; set; }

    [JsonPropertyName("retry")]
    public int Retry { get; set; }

    [JsonPropertyName("broadcastid")]
    public string? BroadcastId { get; set; }

    [JsonPropertyName("eresult")]
    public int Eresult { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("viewertoken")]
    public string? ViewerToken { get; set; }

    [JsonPropertyName("hls_url")]
    public string? HlsUrl { get; set; }

    [JsonPropertyName("heartbeat_interval")]
    public int HeartbeatInterval { get; set; }

    [JsonPropertyName("is_rtmp")]
    public int IsRtmp { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("num_viewers")]
    public int NumViewers { get; set; }

    [JsonPropertyName("is_webrtc")]
    public bool? IsWebrtc { get; set; }

    [JsonPropertyName("webrtc_session_id")]
    public string? WebrtcSessionId { get; set; }

    [JsonPropertyName("webrtc_offer_sdp")]
    public string? WebrtcOfferSdp { get; set; }

    [JsonPropertyName("webrtc_turn_server")]
    public string? WebrtcTurnServer { get; set; }

    [JsonPropertyName("is_replay")]
    public int IsReplay { get; set; }

    [JsonPropertyName("cdn_auth_url_parameters")]
    public string? CdnAuthUrlParameters { get; set; }
}
