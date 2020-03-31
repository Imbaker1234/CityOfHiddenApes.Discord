namespace CityOfHiddenApes.Discord.Core
{
    using System;
    using System.Collections.Generic;
    using global::Discord;
    using global::Discord.WebSocket;

    internal static class Memory
    {
        public static Embed HelpAllEmbed { get; set; }

        public static Dictionary<string, Embed> HelpEmbeds { get; set; } = new Dictionary<string, Embed>();

        public static Dictionary<Type, string[]> CommandLineWrapperPrefixes { get; set; } =
            new Dictionary<Type, string[]>();

        public static SocketTextChannel ResourceChannel { get; set; }
        public static string LocalResourceFolder { get; set; }
        public static string HelpFooterIconUrl { get; set; }
        public static string HelpThumbnailUrl { get; set; }
        public static string Sp { get; set; } = "--";
        public static char CommandPrefix { get; set; } = '/';
        public static string Separator { get; set; }
        public static List<string> HelpAllList { get; set; } = new List<string>();
        public static bool DeleteCommands { get; set; } = true;
    }
}