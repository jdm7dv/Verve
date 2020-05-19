using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio.Util;

namespace MBT.Escience.Parse
{

    public class DictionaryParser<TKey, TValue>
    {
        /// <summary>
        /// A list of key-value pairs of the form
        /// (key1=val1,key2=val2,key3=val3)
        /// </summary>
        [Parse(ParseAction.Required)]
        public List<ParsableKeyValuePair<TKey, TValue>> KeyValuePairs;

        public static implicit operator DictionaryParser<TKey, TValue>(Dictionary<TKey, TValue> unparsable)
        {
            return new DictionaryParser<TKey, TValue> { KeyValuePairs = unparsable.Select(pair => (ParsableKeyValuePair<TKey, TValue>)pair).ToList() };
        }

        public static implicit operator Dictionary<TKey, TValue>(DictionaryParser<TKey, TValue> parsable)
        {
            return parsable.KeyValuePairs.Select(pair => (KeyValuePair<TKey, TValue>)pair).ToDictionary();
        }
    }


    //public class DictionaryParser<TKeyParse, TValueParse,TKeyResult,TValueResult>  
    //{
    //    /// <summary>
    //    /// A list of key-value pairs of the form
    //    /// (key1=val1,key2=val2,key3=val3)
    //    /// </summary>
    //    [Parse(ParseAction.Required)]
    //    public List<ParsableKeyValuePair<TKeyParse, TValueParse>> KeyValuePairs;

    //    public static implicit operator DictionaryParser<TKeyParse, TValueParse, TKeyResult,TValueResult>(Dictionary<TKeyResult, TValueResult> unparsable)
    //    {
    //        return new DictionaryParser<TKeyParse, TValueParse, TKeyResult, TValueResult> { KeyValuePairs = unparsable.Select(pair => new ParsableKeyValuePair<TKeyParse, TValueParse>((TKeyParse)(object)pair.Key, (TValueParse)(object)pair.Value)).ToList() };
    //    }

    //    public static implicit operator Dictionary<TKeyResult, TValueResult>(DictionaryParser<TKeyParse, TValueParse, TKeyResult, TValueResult> parsable)
    //    {
    //        return parsable.KeyValuePairs.Select(pair => new KeyValuePair<TKeyResult, TValueResult>((TKeyResult)(object)pair.Key, (TValueResult)(object)pair.Value)).ToDictionary();
    //    }
    //}


    /// <summary>
    /// Utility class to enable parsing KeyValuePairs.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public struct ParsableKeyValuePair<TKey, TValue>
    {
        public TKey Key;

        public TValue Value;

        public ParsableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        /// <summary>
        /// Parses a Key:value pair.
        /// </summary>
        public static ParsableKeyValuePair<TKey, TValue> Parse(string parseString)
        {
            string[] fields = parseString.Split('=');
            Helper.CheckCondition<ParseException>(fields.Length == 2, "ParsableKeyValuePair expects a string of the form key=value. {0} is not valid.", parseString);
            TKey key;
            TValue value;

            if (!Parser.TryParse<TKey>(fields[0], out key)) throw new ParseException("Unable to parse {0} as type {1}.", fields[0], typeof(TKey).ToTypeString());
            if (!Parser.TryParse<TValue>(fields[1], out value)) throw new ParseException("Unable to parse {0} as type {1}.", fields[1], typeof(TValue).ToTypeString());

            var result = new ParsableKeyValuePair<TKey, TValue> { Key = key, Value = value };
            return result;
        }

        public override string ToString()
        {
            return Key.ToParseString() + "=" + Value.ToParseString();
        }

        public static implicit operator KeyValuePair<TKey, TValue>(ParsableKeyValuePair<TKey, TValue> parsable)
        {
            return new KeyValuePair<TKey, TValue>(parsable.Key, parsable.Value);
        }

        public static implicit operator ParsableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> nonParsable)
        {
            return new ParsableKeyValuePair<TKey, TValue> { Key = nonParsable.Key, Value = nonParsable.Value };
        }

        
    }
}
