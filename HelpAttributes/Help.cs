namespace Discordia.HelpAttributes
{
    using System.Collections.Generic;
    using System.Text;
    using Discord;
    using Discord.Commands;

    public class Help
    {
        private readonly IEnumerable<string> _acceptedParameters;
        private readonly string _desc;
        private readonly IEnumerable<string> _examples;
        private readonly string _image;
        private readonly string _thumb;
        private readonly string _title;

        public Help(CommandInfo info)
        {
            _title = $"**{info.Name}**";
            var exampleUsages = new List<string>();
            var acceptedParameters = new List<string>();
            foreach (var att in info.Attributes)
            {
                if (att is HelpAttribute helpAttribute)
                {
                    if (helpAttribute.GetType() == typeof(DescriptionAttribute))
                        _desc = helpAttribute.Content;
                    else if (helpAttribute.GetType() == typeof(ExampleUseAttribute))
                        exampleUsages.Add(helpAttribute.Content);
                    else if (helpAttribute.GetType() == typeof(AcceptedParameterAttribute))
                        acceptedParameters.Add(helpAttribute.Content);
                    else if (helpAttribute.GetType() == typeof(ImageAttribute))
                        _image = helpAttribute.Content;
                    else if (helpAttribute.GetType() == typeof(ThumbAttribute)) _thumb = helpAttribute.Content;
                }

                //Now that we've collected all the attributes assign these lists as single strings
                _acceptedParameters = acceptedParameters;
                _examples = exampleUsages;

                //TODO add other clauses here to capture name and aliases
            }

            if (!Discordia.HelpEmbeds.ContainsKey(info.Name.ToLower()) && _desc != null)
            {
                Discordia.HelpEmbeds.Add(info.Name.ToLower(), Embed());
                Discordia.HelpAllList.Add($"**{info.Name}** - `{_desc}`");
            }
        }

        public Embed Embed()
        {
            //Create Embed Container
            var eb = new EmbedBuilder();

            //Bold Header
            eb.Title = $"**{_title}**";

            //If we have a description add it in its own section.
            if (_desc != null)
                eb.AddField(new EmbedFieldBuilder
                {
                    Name = "Overview:",
                    Value = _desc,
                    IsInline = false
                });

            //If we have an image then assign it.
            eb.ImageUrl = _image ?? "";

            //Get the thumbnail url for our Embed. 
            eb.ThumbnailUrl =
                _thumb ?? //Use one if its provided
                Discordia.HelpThumbnailUrl ?? //Use the one from code config if not. 
                ""; //Use nothing if all else fails.

            //Ready a container for our examples.
            var exampleBuilder = new StringBuilder();

            foreach (var example in _examples) exampleBuilder.AppendLine(example); //Assemble lines of example calls

            //If we have examples: Display them in their own field.
            if (exampleBuilder.ToString() != "")
                eb.AddField(new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Example Usage:",
                    Value = exampleBuilder.ToString()
                });

            //Ready a container for our parameters to be display in the footer.
            var parameterBuilder = new StringBuilder();
            parameterBuilder.AppendLine("Accepted Parameters");

            //Assemble our parameters to show what is available and what each is for.
            foreach (var parameter in _acceptedParameters) parameterBuilder.AppendLine(parameter);

            eb.Color = Color.Teal;
            //TODO Display aliases and perhaps the main command as the header.

            //Display the accepted parameters in the footer.
            eb.Footer = new EmbedFooterBuilder
            {
                IconUrl = Discordia.HelpFooterIconUrl ?? "",
                Text = parameterBuilder.ToString()
            };

            return eb.Build();
        }
    }
}