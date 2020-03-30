namespace Discordia.Embeds
{
    using System;
    using System.Reflection;
    using Arguments;
    using Discord;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class EmbedColorAttribute : EmbedAttribute
    {
        public EmbedColorAttribute(ReColors value)
        {
            Value = new ReColor(value);
        }

        public EmbedColorAttribute()
        {
        }

        public ReColor Value { get; set; }

        public override EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder)
        {
            builder.Color = property.GetValue(owner) as ReColor ?? Value ?? builder.Color ?? ReColor.Random();

            return builder;
        }

        public override EmbedBuilder ResolveClass(EmbedBuilder builder)
        {
            builder.Color = Value;
            return builder;
        }
    }
}