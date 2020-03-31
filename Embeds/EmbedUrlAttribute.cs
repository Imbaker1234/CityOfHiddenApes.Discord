namespace CityOfHiddenApes.Discord.Core
{
    using System;

    public abstract class EmbedUrlAttribute : EmbedAttribute
    {
        private Uri _uri;

        public EmbedUrlAttribute(string value)
        {
            Value = value;
        }

        protected EmbedUrlAttribute()
        {
            _uri = null;
        }

        public string Value
        {
            get => _uri?.ToString();
            set
            {
                try
                {
                    _uri = new Uri(value);
                }
                catch (UriFormatException ue)
                {
                    throw new Exception($"Embedded string '{value}' is not a valid Url");
                }
            }
        }
    }
}