namespace Discordia.Embeds
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Arguments;
    using Discord;

    public class Embeddable
    {
        public virtual EmbedBuilder Embed(bool expanded = true)
        {
            var product = new EmbedBuilder();

            BuildFromClass(product);

            BuildFromProperties(product);

            if (!expanded)
            {
                product.ThumbnailUrl = product.ImageUrl;
                product.ImageUrl = "";
            }

            return product;
        }

        public virtual string BuildColumn(int number)
        {
            var sb = new StringBuilder();
            foreach (var property in GetType().GetProperties())
            foreach (var customAttribute in property.GetCustomAttributes<EmbedColumnAttribute>())
                if (customAttribute.ColumnNumber == number)
                {
                    var val = property.GetValue(this) ?? "-";
                    sb.AppendLine($"{Parser.DisplayPascalProperty(property.Name)}: {val}");
                }

            return sb.ToString();
        }

        private void BuildFromClass(EmbedBuilder product)
        {
            foreach (var customAttribute in GetType().GetCustomAttributes())
                if (customAttribute is EmbedAttribute embedAttribute)
                    embedAttribute.ResolveClass(product);
        }

        private EmbedBuilder BuildFromProperties(EmbedBuilder product)
        {
            var efs = new List<Tuple<int, EmbedFieldBuilder>>();

            foreach (var property in GetType().GetProperties())
            foreach (var customAttribute in property.GetCustomAttributes<EmbedAttribute>())
                if (customAttribute is EmbedFieldAttribute efb)
                    efs.Add(efb.RetrieveAndTagResolvedProperty(this, property, product));
                else
                    customAttribute.ResolveProperty(this, property, product);

            //Sort the embed fields.
            var orderedEmbedFields = efs.OrderBy(ef1 => ef1.Item1).Select(tuples => tuples.Item2);

            //Add the ordered fields.
            foreach (var ef in orderedEmbedFields) product.AddField(ef);

            return product;
        }
    }
}