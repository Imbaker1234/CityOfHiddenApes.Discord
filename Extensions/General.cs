namespace CityOfHiddenApes.Discord.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Arguments;
    using global::Discord;

    public static class General
    {
        public static T RetrieveOrCalculate<T>(ref T privateObject, Func<T> impl)
        {
            if (privateObject != null) return privateObject;

            privateObject = impl.Invoke();

            return privateObject;
        }

        public static T RetrieveOrCalculate<T>(T privateObject, Func<T> impl)
        {
            if (privateObject != null) return privateObject;

            privateObject = impl.Invoke();

            return privateObject;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable) action.Invoke(item);
        }
    }
}