using System.Reflection;

namespace ASFBroadcast;

internal static class AdapterBridge
{
    /// <summary>
    /// 注册子模块
    /// </summary>
    /// <param name="pluginName">插件名称</param>
    /// <param name="pluginId">插件唯一标识符</param>
    /// <param name="cmdPrefix">命令前缀</param>
    /// <param name="repoName">自动更新仓库</param>
    /// <param name="cmdHandler">命令处理函数</param>
    /// <returns></returns>
    public static bool InitAdapter(string pluginName, string pluginId, string? cmdPrefix, string? repoName, MethodInfo? cmdHandler)
    {
        try
        {
            var adapterEndpoint = Assembly.Load("ASFEnhance").GetType("ASFEnhance._Adapter_.Endpoint");
            var registerModule = adapterEndpoint?.GetMethod("RegisterModule", BindingFlags.Static | BindingFlags.Public);
            var pluginVersion = Assembly.GetExecutingAssembly().GetName().Version;

            if (registerModule != null && adapterEndpoint != null)
            {
                var result = registerModule?.Invoke(null, [pluginName, pluginId, cmdPrefix, repoName, pluginVersion, cmdHandler]);

                if (result is string str)
                    if (str == pluginName)
                        return true;
                    else
                        ASFLogger.LogGenericWarning(str);
            }
        }
#if DEBUG
        catch (Exception ex)
        {
            ASFLogger.LogGenericException(ex, "Community with ASFEnhance failed");
        }
#else
        catch (Exception)
        {
            ASFLogger.LogGenericDebug("Community with ASFEnhance failed");
        }
#endif
        return false;
    }
}
