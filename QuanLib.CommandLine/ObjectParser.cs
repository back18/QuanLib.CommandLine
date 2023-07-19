using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class ObjectParser
    {
        static ObjectParser()
        {
            ObjectParser parser = new(
                (string s) => s ?? throw new ArgumentNullException(nameof(s)),
                (string s, [MaybeNullWhen(false)] out object result) =>
                {
                    result = s ?? throw new ArgumentNullException(nameof(s));
                    return true;
                });
            parsers = new()
            {
                { typeof(string), parser }
            };
        }

        public ObjectParser(ParseDelegate parse, TryParseDelegate? tryParse = null)
        {
            this.parse = parse;
            if (tryParse is not null)
                this.tryParse = tryParse;
            else
            {
                this.tryParse = (string s, [MaybeNullWhen(false)] out object result) =>
                {
                    try
                    {
                        result = this.parse(s);
                        return true;
                    }
                    catch
                    {
                        result = default;
                        return false;
                    }
                };
            }
        }

        private readonly static Dictionary<Type, ObjectParser> parsers;

        private readonly ParseDelegate parse;
        private readonly TryParseDelegate tryParse;

        public object Parse(string s) => parse(s);

        public bool TryParse(string s, [MaybeNullWhen(false)] out object result)
            => tryParse(s, out result);

        public bool Check(string s)
            => TryParse(s, out _);

        public static ObjectParser? GetParser(Type type)
        {
            lock (parsers)
            {
                if (parsers.TryGetValue(type, out var value))
                    return value;
                else
                {
                    ObjectParser? newvalue = GetObjectParser_(type);
                    if (newvalue is null)
                        return null;

                    parsers.Add(type, newvalue);
                    return newvalue;
                }
            }
        }

        private static ObjectParser? GetObjectParser_(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            MethodInfo? parseMethod = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
            if (parseMethod is null)
                return null;
            object parseDelegate(string s) => parseMethod.Invoke(null, new object[] { s });

            MethodInfo? tryParseMethod = type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, new[] { typeof(string), type.MakeByRefType() });
            TryParseDelegate? tryParseDelegate = null;
            if (tryParseMethod is not null && tryParseMethod.ReturnType == typeof(bool))
            {
                tryParseDelegate = (string s, [MaybeNullWhen(false)] out object result) =>
                {
                    object[] parameters = new object[] { s, default };
                    object? obj = tryParseMethod.Invoke(null, parameters);
                    bool b = obj is null ? false : (bool)obj;

                    result = parameters[1];
                    return b;
                };
            }

            return new((string s) => parseMethod.Invoke(null, new object[] { s }), tryParseDelegate);
        }

        public delegate object ParseDelegate(string s);
        public delegate bool TryParseDelegate(string s, [MaybeNullWhen(false)] out object result);
    }
}
