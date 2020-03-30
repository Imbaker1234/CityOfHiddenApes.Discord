namespace Discordia.HelpAttributes
{
    using System;
    using System.Collections.Generic;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PrefixAttribute : HelpAttribute
    {
        private IEnumerable<string> _prefixes;

        public PrefixAttribute(IEnumerable<string> prefixes)
        {
            _prefixes = prefixes;
        }

        public override string Content { get; }
    }
}