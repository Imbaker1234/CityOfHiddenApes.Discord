namespace CityOfHiddenApes.Discord.Core.HelpAttributes
{
    using System;
    using System.Text;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ExampleUseAttribute : HelpAttribute
    {
        public ExampleUseAttribute(string whatItDoes, string example)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"--{whatItDoes}");
            sb.AppendLine($"   `{example}`");
            Content = sb.ToString();
        }

        public override string Content { get; }
    }
}