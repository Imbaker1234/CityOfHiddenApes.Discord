namespace Discordia.Arguments
{
    using System.Dynamic;

    public class CommandTypeWrapper<T> : DynamicObject
    {
        protected CommandTypeWrapper(T t)
        {
            _value = t;
        }

        private T _value { get; set; }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _value;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _value = (T) value;
            return base.TrySetMember(binder, value);
        }

        public static implicit operator T(CommandTypeWrapper<T> t) => t._value;
        public static explicit operator CommandTypeWrapper<T>(T t) => new CommandTypeWrapper<T>(t);
    }
}