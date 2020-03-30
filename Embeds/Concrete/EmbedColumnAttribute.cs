namespace Discordia.Embeds
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class EmbedColumnAttribute : Attribute
    {
        public EmbedColumnAttribute(int columnNumber)
        {
            ColumnNumber = columnNumber;
        }

        public int ColumnNumber { get; set; }
    }
}