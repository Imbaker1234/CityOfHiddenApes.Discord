namespace Discordia.Embeds
{
    using System;
    using System.Reflection;
    using Arguments;
    using Discord;

    [AttributeUsage(AttributeTargets.Property)]
    public class EmbedFieldAttribute : EmbedAttribute
    {
        /// <summary>
        ///     Upon the call of the Embed() method this attribute is reflected
        ///     against and used to derive an EmbedField based on the decorated
        ///     property.
        /// </summary>
        /// <param name="fieldOrder"></param>
        /// <param name="isInline"></param>
        /// <param name="nameOverride"></param>
        /// <param name="nullDisplayValue"></param>
        public EmbedFieldAttribute(
            int fieldOrder,
            bool isInline = true,
            string nameOverride = null,
            string nullDisplayValue = "-")
        {
            FieldOrder = fieldOrder;
            IsInline = isInline;
            NameOverride = nameOverride;
            NullDisplayValue = nullDisplayValue;
        }

        /// <summary>
        ///     An optional parameter that denotes whether or not this particular
        ///     field should be in line.
        /// </summary>
        public bool IsInline { get; set; }

        /// <summary>
        ///     An optional parameter that sets the name of the Field.
        ///     If this is not provided the name of the property itself
        ///     will be used instead.
        /// </summary>
        public string NameOverride { get; set; }

        /// <summary>
        ///     A mandatory parameter which denotes the order in which the field is
        ///     added to and appears in the Embed.
        /// </summary>
        public int FieldOrder { get; set; }

        /// <summary>
        ///     Defaulting to "-". This value is shown instead when the property is null.
        /// </summary>
        public string NullDisplayValue { get; set; }

        public override EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder)
        {
            throw new NotImplementedException();
        }

        //This only gets called on class level properties and therefore will never be called.
        public override EmbedBuilder ResolveClass(EmbedBuilder builder)
        {
            throw new NotImplementedException();
        }

        public Tuple<int, EmbedFieldBuilder> RetrieveAndTagResolvedProperty(object owner, PropertyInfo property,
            EmbedBuilder builder)
        {
            var ef = new EmbedFieldBuilder
            {
                Value = property.GetValue(owner) ?? NullDisplayValue,
                IsInline = IsInline,
                Name = NameOverride ?? Parser.DisplayPascalProperty(property.Name)
            };
            return new Tuple<int, EmbedFieldBuilder>(FieldOrder, ef);
        }
    }
}