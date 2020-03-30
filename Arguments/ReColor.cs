namespace Discordia.Arguments
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Discord;
    using Newtonsoft.Json;

    public class ReColor
    {
        [JsonIgnore]
        private Color? _value;

        private ReColors _setterEnum;
            
            
        public ReColors SetterEnum
        {
            get => _setterEnum;
            set
            {
                _value = GetColor(value);
                _setterEnum = value;
            }
        }

        public ReColor()
        {
        }
        
        private ReColor(Color value)
        {
            _value = value;
        }

        private ReColor(Color? value)
        {
            if (value != null) _value = value.Value;
        }

        public ReColor(ReColors color)
        {
            _value = GetColor(color);
        }

        public ReColor(string color)
        {
            _value = GetColor(color);
        }

        private static Dictionary<ReColors, Color> Colors { get; } = new Dictionary<ReColors, Color>
        {
            {ReColors.Blue, Color.Blue}, {ReColors.Default, Color.Default}, {ReColors.Gold, Color.Gold},
            {ReColors.Green, Color.Green}, {ReColors.Magenta, Color.Magenta}, {ReColors.Orange, Color.Orange},
            {ReColors.Purple, Color.Purple}, {ReColors.Red, Color.Red}, {ReColors.Teal, Color.Teal},
            {ReColors.DarkBlue, Color.DarkBlue}, {ReColors.DarkerGrey, Color.DarkerGrey},
            {ReColors.DarkGreen, Color.DarkGreen}, {ReColors.DarkGrey, Color.DarkGrey},
            {ReColors.DarkMagenta, Color.DarkMagenta}, {ReColors.DarkOrange, Color.DarkOrange},
            {ReColors.DarkPurple, Color.DarkPurple}, {ReColors.DarkRed, Color.DarkRed},
            {ReColors.DarkTeal, Color.DarkTeal}, {ReColors.LighterGrey, Color.LighterGrey},
            {ReColors.LightGrey, Color.LightGrey}, {ReColors.LightOrange, Color.LightOrange}
        };

        public static ReColor Random()
        {
            var values = Enum.GetValues(typeof(ReColors));
            var random = new Random();
            var randomColor = (ReColors) values.GetValue(random.Next(values.Length));

            return new ReColor(randomColor);
        }

        private Color GetColor(string color)
        {
            color = PascalCase(color);
            if (Enum.TryParse(color, out ReColors result))
            {
                SetterEnum = result;
                return Colors[result];
            }

            SetterEnum = ReColors.DarkBlue;
            return Colors[ReColors.DarkBlue];
        }

        private Color GetColor(ReColors color)
        {
            _setterEnum = color;
            return Colors[color];
        }

        public string PascalCase(string color)
        {
            var split = color.Split(' ');

            var sb = new StringBuilder();
            split.ForEach(sp =>
            {
                var chars = sp.ToCharArray();
                chars[0] = char.ToUpper(chars[0]);
                sb.Append(chars);
            });
            return sb.ToString();
        }

        public static explicit operator ReColor(ReColors c)
        {
            return new ReColor(c);
        }

        public static implicit operator Color(ReColor rc)
        {
            return rc._value.Value;
        }

        public static explicit operator ReColor(Color c)
        {
            return new ReColor(c);
        }

        public static implicit operator Color?(ReColor rc)
        {
            return rc._value;
        }

        public static explicit operator ReColor(Color? c)
        {
            return new ReColor(c);
        }

        public static implicit operator string(ReColor rc)
        {
            return rc.SetterEnum.ToString();
        }

        public static explicit operator ReColor(string c)
        {
            return new ReColor(c);
        }
    }
}