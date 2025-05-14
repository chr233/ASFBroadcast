namespace ASFBroadcast.Data;

/// <summary>
/// 插件配置
/// </summary>
public sealed record PluginConfig
{
    /// <summary>
    /// 是否同意使用协议
    /// </summary>
    public bool EULA { get; set; }
    /// <summary>
    /// 启用统计信息
    /// </summary>
    public bool Statistic { get; set; }
    /// <summary>
    /// 禁用挂卡
    /// </summary>
    public bool DisableFarming { get; set; }
    /// <summary>
    /// 心跳发送周期
    /// </summary>
    public int HeartbeatInterval { get; set; } = 30;

    /// <summary>
    /// 同步直播间信息周期
    /// </summary>
    public int SummaryInterval { get; set; } = 180;
}
