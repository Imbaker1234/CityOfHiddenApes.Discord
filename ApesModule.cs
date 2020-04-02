namespace CityOfHiddenApes.Discord.Core
{
    using System;
    using global::Discord.Commands;

    public class ApesModule<T> : ModuleBase<T> where T: SocketCommandContext
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