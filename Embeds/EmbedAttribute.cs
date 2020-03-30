namespace Discordia.Embeds
{
    using System;
    using System.Reflection;
    using Discord;

    public abstract class EmbedAttribute : Attribute
    {
        public abstract EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder);

        public abstract EmbedBuilder ResolveClass(EmbedBuilder builder);
    }
}