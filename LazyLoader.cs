namespace Discordia
{
    using System;

    public static class LazyLoader
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
    }
}