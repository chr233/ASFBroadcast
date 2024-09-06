using System.Text.RegularExpressions;

namespace ASFBroadcast;
internal static partial class RegexUtils
{
    [GeneratedRegex(@"(?:broadcast\/watch\/)?(\d+)")]
    public static partial Regex MatchBraodcastSteamId();
}
