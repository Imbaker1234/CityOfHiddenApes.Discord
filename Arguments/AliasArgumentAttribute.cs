namespace Discordia.Arguments
{
    using System;

    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Parameter)]
    public class AliasArgumentAttribute : Attribute
    {
        public int Number { get; set; }

        public string HowItsUsed { get; set; }

        public AliasArgumentAttribute(int number, string howItsUsed = null)
        {
            Number = number;
            HowItsUsed = howItsUsed;
        }
    }
}