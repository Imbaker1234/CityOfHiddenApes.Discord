namespace Discordia.Embeds
{
    public abstract class EmbedStringValueAttribute : EmbedAttribute
    {
        public EmbedStringValueAttribute(string value)
        {
            Value = value;
        }

        public EmbedStringValueAttribute()
        {
        }

        public string Value { get; set; }
    }
}