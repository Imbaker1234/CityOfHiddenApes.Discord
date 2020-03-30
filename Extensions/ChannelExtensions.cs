namespace Discordia.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Rest;
    using Discord.WebSocket;

    public static class ChannelExtensions
    {
        /// <summary>
        ///     Allows you to supply a list of matching criteria for IUserMessages and returns
        ///     a list of any messages from among the pins in this channel.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public static async Task<List<IUserMessage>> FilterPinsAsync(this SocketTextChannel channel,
            params Func<RestMessage, bool>[] predicates)
        {
            var product = new List<IUserMessage>();

            var pins = await channel.GetPinnedMessagesAsync();

            FilterImpl(predicates, pins, product);

            return product;
        }

        private static void FilterImpl(Func<RestMessage, bool>[] predicates, IReadOnlyCollection<RestMessage> messages,
            List<IUserMessage> product)
        {
            messages.ToList().ForEach(pin =>
            {
                foreach (var pred in predicates)
                {
                    if (!pred.Invoke(pin)) continue;
                    product.Add((IUserMessage) pin);
                    break;
                }
            });

            messages.ForEach(message =>
            {
                predicates.ForEach(func =>
                {
                    if (func.Invoke(message)) product.Add((IUserMessage) message);
                });
            });
        }
    }
}