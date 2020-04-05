namespace CityOfHiddenApes.Discord.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MixedArgumentReader
    {
        //Iterator Properties
        private CharEnumerator Enumerator { get; set; }

        private List<char> _buffer;

        private bool _processing = true;

        private char? _current;
        private char? _nextChar;
        private char? _lastChar;

        public List<char> Buffer => _buffer ??= new List<char>();

        public char? CLastChar
        {
            get => _lastChar ??= BCurrent;
            set => _lastChar = value;
        }

        public char? BCurrent
        {
            get => _current ??= ANextChar;
            set => _current = value;
        }

        public char? ANextChar
        {
            get
            {
                try
                {
                    return Enumerator.Current;
                }
                catch (Exception e)
                {
                    _processing = false;
                    return ' ';
                }
            }
            set => _nextChar = value;
        }


        //Result Properties
        public Dictionary<string, string> NamedArguments { get; set; } = new Dictionary<string, string>();
        public List<string> InferredArguments { get; set; } = new List<string>();

        public MixedArgumentReader(string input)
        {
            Enumerator = input.GetEnumerator();
        }

        public void ClearBuffer()
        {
            _buffer = new List<char>();
        }

        public bool MoveNext(params char?[] skipChars)
        {
            CLastChar = BCurrent;
            BCurrent = ANextChar;
            if (!skipChars.Contains(CLastChar) && CLastChar.HasValue) Buffer.Add(CLastChar.Value);
            var product = Enumerator.MoveNext();
            return product;
        }

        public void ParseStream()
        {
            //Get rid of the command
            ClearCommand();

            //Till we're done parsing the whole command message
            while (MoveNext() && _processing)
            {
                //If we start with quotes then parse the quoted argument
                if (BCurrent == '\"') InferredArguments.Add(ParseQuotedArgument());
                //If its not a space then its a word.
                else if (BCurrent != ' ' && BCurrent != '\n' && BCurrent != '\r' && ANextChar != '\"') ParseUnquotedArgument();
            }
        }

        public void ParseUnquotedArgument()
        {
            var delivered = false; //To catch end of statement entries.
            //Clear out spaces prior to this.
            ClearBuffer();
            //Till we reach either a colon or a space.
            while (MoveNext(' ', '\n', '\r', ':') && _processing)
            {
                //If it's a space then we have a single word argument.
                if (BCurrent == ' ')
                {
                    InferredArguments.Add(CollectBuffer());
                    delivered = true;
                    // ClearWhiteSpace();
                    break;
                }

                //If its a Colon we need to parse a key value argument
                if (BCurrent == ':' && ANextChar == ' ')
                {
                    ParseKeyValueArgument();
                    delivered = true;
                    // ClearWhiteSpace();
                    break;
                } 
                else if (BCurrent == ':' && ANextChar != ' ') Buffer.Add(':');

            }

            if (!delivered)
            {
                MoveNext();
                InferredArguments.Add(CollectBuffer());
            }
        }

        public void ClearWhiteSpace()
        {
            while (MoveNext(' ', '\r', '\n') && _processing)
            {
                if (ANextChar == ' ' || ANextChar == '\n' || ANextChar == '\r') continue;
                ClearBuffer();
                break; //If its a real character such as a quote or non whitespace character
                //then break for processing.
            }
        }

        public string CollectBuffer()
        {
            return new string(Buffer.ToArray());
        }

        public void ParseKeyValueArgument()
        {
            var key = CollectBuffer().Trim(); //Store the last word in the buffer as the key.
            MoveNext(':'); //Move Beyond the colon. Skipping it

            //Clear out any spaces, line breaks, or carriage returns after the ':'
            while (MoveNext(' ', '\n', '\r') && (BCurrent == ' ' || BCurrent == '\n' || BCurrent == '\r') &&
                   _processing)
            {
            }

            string value;
            if (BCurrent == '\"') value = ParseQuotedArgument();
            else
            {
                ClearBuffer();

                while (BCurrent != ' ' && BCurrent != '\n' && BCurrent != '\r' && _processing)
                {
                    MoveNext();
                }

                value = CollectBuffer();
                ClearBuffer();
            }

            NamedArguments.Add(key, value);
        }

        public string ParseQuotedArgument()
        {
            MoveNext('"'); //Move beyond the initial quote.
            ClearBuffer();
            //While we have more characters to parse and the last character 
            //We should accept line breaks, carriage returns, tabs, and spaces.
            while (MoveNext() && _processing)
            {
                if (BCurrent == '\"') //We should break on quotes
                {
                    //Unless the prior or latter character is an asterisk.
                    //in which case we are opening inner quotes.
                    if (ANextChar == '*' || CLastChar == '*') continue;
                    break;
                }
            }

            var product = CollectBuffer().TrimEnd('"');
            ClearWhiteSpace();
            return product;
        }

        public void ClearCommand()
        {
            Enumerator.MoveNext();
            _processing = true;
            while (MoveNext())
            {
                if (ANextChar == ' ' || ANextChar == '\n' || ANextChar == '\r')
                {
                    ClearWhiteSpace();
                    return;
                }
            }

            throw new ArgumentException();
        }
    }
}