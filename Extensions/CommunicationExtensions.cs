namespace Discordia
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;

    public static class CommunicationExtensions
    {
        public static async Task WhisperRole(this SocketCommandContext context, string roleName, string message,
            Embed embed)
        {
            await context.Whisper(
                usr =>
                    usr.Roles.SingleOrDefault(role =>
                        role.Name == roleName) != null, message, embed);
        }

        public static async Task Whisper(this SocketCommandContext context, Func<SocketGuildUser, bool> predicate,
            string message = "", Embed embed = null)
        {
            await Task.WhenAll(context.Guild.Users.Where(predicate)
                .Select(user => user.SendMessageAsync(message, false, embed)));
        }

        public static async Task SilentPin(this SocketCommandContext context)
        {
            //TODO This
        }
    }
}