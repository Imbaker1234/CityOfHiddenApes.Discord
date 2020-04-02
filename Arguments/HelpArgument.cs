namespace CityOfHiddenApes.Discord.Core.Arguments
{
    using System;

    /// <summary>
    /// Denotes arguments on a command model. Allows for argument aliases to be grouped
    /// for the purposes of display in Help Embeds.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Parameter)]
    public class HelpArgumentAttribute : Attribute
    {
        public int Number { get; set; }

        public string HowItsUsed { get; set; }

        public HelpArgumentAttribute(int number, string howItsUsed = null)
        {
            Number = number;
            HowItsUsed = howItsUsed;
        }
    }
}