namespace CityOfHiddenApes.Discord.Core.Extensions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using global::Discord.Commands;
    using global::Discord.WebSocket;

    public static class ContextExtensions
    {
        /// <summary>
        ///     Gets the named Category.
        ///     <para>
        ///         If createIfDoesNotExist is true the Category will be created if found to be missing
        ///         and returned to the caller.
        ///     </para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="createIfDoesNotExist"></param>
        /// <returns></returns>
        public static async Task<SocketCategoryChannel> GetCategory(this SocketCommandContext context, ulong identifier)
        {
            var product =
                context.Guild.CategoryChannels.SingleOrDefault(cat => cat.Id == identifier)
                ?? throw new Exception($"Category with provided Id '{identifier}' does not exist.");

            return product;
        }

        /// <summary>
        ///     Gets the named Category.
        ///     <para>
        ///         If createIfDoesNotExist is true the Category will be created if found to be missing
        ///         and returned to the caller.
        ///     </para>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="createIfDoesNotExist"></param>
        /// <returns></returns>
        public static async Task<SocketCategoryChannel> GetCategory(this SocketCommandContext context,
            string identifier,
            bool createIfDoesNotExist = false)
        {
            SocketCategoryChannel product;

            product = context.Guild.CategoryChannels.SingleOrDefault(cat => cat.Name == identifier);

            if (product == null && createIfDoesNotExist)
            {
                var rest = await context.Guild.CreateCategoryChannelAsync(identifier);
                product = context.Guild.GetCategoryChannel(rest.Id);
            }

            return product;
        }

        /// <summary>
        ///     Gets the Channel by its Id as a SocketTextChannel.
        ///     <para>
        ///         If createIfDoesNotExist is true the Channel itself
        ///         will be created at the root level of the Server outside of any category in the
        ///         event that the channel is found to be missing before it is returned to the caller..
        ///     </para>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="categoryName"></param>
        /// <param name="channelName"></param>
        /// <param name="createIfDoesNotExist"></param>
        /// <returns></returns>
        public static async Task<SocketTextChannel> GetTextChannel(this SocketCommandContext context,
            ulong channelId)
        {
            var product = (SocketTextChannel) context.Guild.Channels
                              .SingleOrDefault(ch => ch.Id == channelId)
                          ?? throw new Exception($"A channel with the provided Id '{channelId}' does not exist.");

            return product;
        }


        /// <summary>
        ///     Gets the named Channel.
        ///     <para>
        ///         If createIfDoesNotExist is true the Channel itself
        ///         will be created at the root level of the Server outside of any category in the
        ///         event that the channel is found to be missing before it is returned to the caller..
        ///     </para>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="categoryName"></param>
        /// <param name="channelName"></param>
        /// <param name="createIfDoesNotExist"></param>
        /// <returns></returns>
        public static async Task<SocketTextChannel> GetTextChannel(this SocketCommandContext context,
            string channelName, bool createIfDoesNotExist = false)
        {
            SocketTextChannel product;

            product = (SocketTextChannel) context.Guild.Channels.SingleOrDefault(ch => ch.Name == channelName);

            if (product == null && createIfDoesNotExist)
            {
                var rest = await context.Guild.CreateTextChannelAsync(channelName);
                product = context.Guild.GetTextChannel(rest.Id);
            }

            return product;
        }

        /// <summary>
        ///     Gets the named Channel.
        ///     <para>
        ///         If createIfDoesNotExist is true the Category and the Channel itself
        ///         will be created if found to be missing and the Channel returned to the caller.
        ///     </para>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="categoryName"></param>
        /// <param name="channelName"></param>
        /// <param name="createIfDoesNotExist"></param>
        /// <returns></returns>
        public static async Task<SocketTextChannel> GetTextChannel(this SocketCommandContext context,
            string categoryName, string channelName, bool createIfDoesNotExist = false)
        {
            SocketTextChannel product;
            var parent = await context.GetCategory(categoryName, createIfDoesNotExist);

            product = (SocketTextChannel) parent.Channels.SingleOrDefault(ch => ch.Name == channelName);

            if (product == null && createIfDoesNotExist)
            {
                var rest = await context.Guild.CreateTextChannelAsync(channelName,
                    opts => { opts.CategoryId = parent.Id; });
                product = context.Guild.GetTextChannel(rest.Id);
            }

            return product;
        }


        public static string GetCallerNickName(this SocketCommandContext context)
        {
            try
            {
                return context.Guild.GetUser(context.User.Id).Nickname;
            }
            catch (NullReferenceException)
            {
                return context.User.Username;
            }
        }
    }
}