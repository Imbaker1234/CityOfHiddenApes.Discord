namespace CityOfHiddenApes.Discord.Core.Exceptions
{
    using System;
    using System.Text;
    using Arguments;

    public class ColorException : Exception
    {
        public override string Message { get; }

        private string AvailableColors()
        {
            var sb = new StringBuilder();
            foreach (var enumeration in Enum.GetValues(typeof(ReColor.Colors)))
            {
                sb.AppendLine(enumeration.ToString());
            }
            return sb.ToString();
        }
        
        public ColorException()
        {
        }

        public ColorException(string message) : base(message)
        {
             Message = $"Failed to parse '{message}' to a Color. Available choices are:\n{AvailableColors()}";
        }
    }
}