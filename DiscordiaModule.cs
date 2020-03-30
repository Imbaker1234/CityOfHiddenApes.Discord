namespace Discordia
{
    using System;
    using Discord.Commands;

    public class DiscordiaModule : ModuleBase<SocketCommandContext>
    {
        private IDisposable Bubbles { get; set; }

        protected override void BeforeExecute(CommandInfo command)
        {
            Bubbles = Context.Channel.EnterTypingState();
            base.BeforeExecute(command);
        }

        protected override void AfterExecute(CommandInfo command)
        {
            Bubbles.Dispose();
            base.AfterExecute(command);
        }
    }
}