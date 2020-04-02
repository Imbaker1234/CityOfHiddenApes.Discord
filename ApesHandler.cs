namespace CityOfHiddenApes.Discord.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Extensions;
    using global::Discord;
    using global::Discord.Commands;
    using global::Discord.WebSocket;
    using HelpAttributes;

    public class ApesHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        // Retrieve client and CommandService instance via ctor
        public ApesHandler(DiscordSocketClient client, CommandService commands)
        {
            _commands = commands;
            _client = client;
        }

        public async Task InstallCommandsAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;
            // Here we discover all of the command modules in the entry 
            // assembly and load them. Starting from Discord.NET 2.0, a
            // service provider is required to be passed into the
            // module registration method to inject the 
            // required dependencies.

            var assembly = Assembly.GetEntryAssembly();

            RegisterAllTypeReaders(assembly); //TODO Make multiple assemblies registerable.
            // RegisterAllReadTypesAndPrefixes(assembly);
            await _commands.AddModulesAsync(assembly,
                null);

            GatherHelpResources(_commands);
        }

        /// <summary>
        ///     Automatically register all type readers so long as the naming convention of
        ///     "object" and "objectReader" is followed. Such as "Airplane.cs" and "AirplaneReader.cs"
        /// </summary>
        /// <param name="assembly"></param>
        protected void RegisterAllTypeReaders(Assembly assembly)
        {
            foreach (var typeReader in assembly.GetTypes().Where(type => typeof(TypeReader).IsAssignableFrom(type)))
            {
                var matchTypeName = typeReader.ToString().Replace("Reader", "");
                var matchedType = assembly.GetType(matchTypeName);
                var reader = (TypeReader) Activator.CreateInstance(typeReader);
                if (matchedType != null && reader != null) _commands.AddTypeReader(matchedType, reader);
            }
        }

        /// <summary>
        ///     <para>
        ///         Gathers the information from the various Help Attributes
        ///         attached to your Commands. Fills out an embed reachable
        ///         by calling HelpEmbeds[TheNameOfYourCommand].
        ///     </para>
        ///     <para>
        ///         Additionally the name and description of the command are placed
        ///         into a resource dictionary for the creation of a HelpAllEmbed
        ///         reachable by calling /help.
        ///     </para>
        /// </summary>
        /// <param name="cmdSrv"></param>
        private static void GatherHelpResources(CommandService cmdSrv)
        {
            cmdSrv.Commands.ForEach(cmd => new Help(cmd));
            var helpAllBuilder = new EmbedBuilder();
            helpAllBuilder.Title = "**Help**";
            helpAllBuilder.Color = Color.Teal;
            helpAllBuilder.ThumbnailUrl = ""; //TODO Set this later.
            var sb = new StringBuilder();
            Memory.HelpAllList.ForEach(entry => sb.AppendLine(entry + "\n"));
            helpAllBuilder.Description = sb.ToString();
            helpAllBuilder.Footer = new EmbedFooterBuilder
            {
                Text = $"For more information on these commands type '{Memory.CommandPrefix}help command name'"
            };
            Memory.HelpAllEmbed = helpAllBuilder.Build();
        }

        private static bool IsACallForHelp(SocketCommandContext context)
        {
            //If this is a general call for help.
            if (context.Message.Content.ToLower() == $"{Memory.CommandPrefix}help".ToLower())
            {
                context.User.SendMessageAsync("", false, Memory.HelpAllEmbed);
                //Return true to tell the handler to break the execution. No further processing necessary.
                return true;
            }

            //If its not a general call for help it may be a call for help on a more specific command.

            //Match the "help for a specific command" pattern.
            var pattern = "(help (\\w+)|(\\w+) help)";
            var match = Regex.Match(context.Message.Content, pattern, RegexOptions.IgnoreCase).Value;

            //If we have no match this isn't a call for help. Return false and resume trying to call the command.
            if (match == "") return false;

            //Remove the help syntax and just leave the name of the command itself.
            var commandName = Regex.Replace(context.Message.Content, "\\s?help\\s?", "", RegexOptions.IgnoreCase);
            commandName = commandName.Replace(Memory.CommandPrefix.ToString(), "");
            //Message the user with the embed retrieved from the precompiled list of Help Embeds identified by the command's name.
            context.User.SendMessageAsync("", false, Memory.HelpEmbeds[commandName.ToLower()]);
            return true;
        }


        protected async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            var argPos = 0;

            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix(Memory.CommandPrefix, ref argPos) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);
            if (Memory.DeleteCommands && !(context.Channel is SocketDMChannel))
                await context.Message.DeleteAsync(); //TODO make this configurable as to whether to delete or not.

            if (IsACallForHelp(context)) return;

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            //TODO Allow injectable actions from Runtime Constants.

            try
            {
                var result = await _commands.ExecuteAsync(
                    context,
                    argPos,
                    null);

                if (result.IsSuccess) return;

                var eb = new EmbedBuilder();

                eb.Title = "Exception:";
                eb.Description = result.ErrorReason;
                eb.Footer = new EmbedFooterBuilder() { Text = $"Your command:\n\n{context.Message.Content}"};
                eb.Color = Color.Purple;
                
                await context.User.SendMessageAsync(embed: eb.Build());
            }
            catch (Exception e)
            {
                //TODO Logging on exception
            }
        }
    }
}