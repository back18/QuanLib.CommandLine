using QuanLib.CommandLine.Attributes;
using QuanLib.CommandLine.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class CommandFunc
    {
        public CommandFunc(MethodInfo methodInfo, object? defaultObject = null, Func<object?, string?>? resultout = null, Func<Exception, string?>? catchexception = null)
        {
            ArgumentNullException.ThrowIfNull(methodInfo, nameof(methodInfo));
            if (!methodInfo.IsStatic && defaultObject is null)
                throw new ArgumentException("非静态方法必须指定默认执行对象", nameof(methodInfo));
            _method = methodInfo;

            if (defaultObject is not null && methodInfo.DeclaringType is not null && !defaultObject.GetType().IsSubclassOf(methodInfo.DeclaringType))
                throw new ArgumentException("默认执行对象的类型，必须派生自方法所属对象类型", nameof(defaultObject));
            DefaultObject = defaultObject;

            resultout ??= (object? o) => o?.ToString();
            catchexception ??= (Exception ex) => $"【错误】{ex.GetType()}:\"{ex.Message}\"";

            ParameterInfo[] parameters = methodInfo.GetParameters();
            List<CommandArgument> arguments = new();
            for (int i = 0; i < parameters.Length; i++)
            {
                int index = i;
                Type type = parameters[i].ParameterType;
                string name = parameters[i].GetCustomAttribute<ArgumentNameAttribute>()?.Name ?? parameters[i].Name ?? string.Empty;
                string description = parameters[i].GetCustomAttribute<ArgumentDescriptionAttribute>()?.Description ?? string.Empty;
                ObjectParser parser =
                    parameters[i].GetCustomAttribute<ArgumentParserAttribute>()?.Parser ??
                    ObjectParser.GetParser(parameters[i].ParameterType) ??
                    throw new ArgumentException(nameof(methodInfo) + " 的一个或多个参数没有 Parse 方法");
                ObjectRange? range = parameters[i].GetCustomAttribute<ArgumentRangeAttribute>()?.Range;

                arguments.Add(new(index, type, name, description, parser, range));
            }
            Arguments = arguments.AsReadOnly();

            _func = (object? obj, out object? result, string[] args) =>
            {
                ArgumentNullException.ThrowIfNull(args, nameof(args));

                try
                {
                    if (args.Length != Arguments.Count)
                        throw new CommandArgumentCountException(Arguments.Count, args.Length);

                    object[] parameters = new object[args.Length];
                    for (int i = 0; i < args.Length; i++)
                    {
                        try
                        {
                            parameters[i] = Arguments[i].Parser.Parse(args[i]);
                        }
                        catch (Exception ex)
                        {
                            throw new CommandArgumentFormatException(Arguments[i], args[i], ex);
                        }

                        ObjectRange? range = Arguments[i].Range;
                        if (range is not null)
                            if (!range.IsRange(parameters[i]))
                                throw new CommandArgumentOutOfRangeException(Arguments[i], parameters[i]);
                    }
                    result = _method.Invoke(obj ?? DefaultObject, parameters);
                    return resultout(result);
                }
                catch (CommandArgumentCountException ex)
                {
                    result = null;
                    return "【错误】" + ex.Message;
                }
                catch (CommandArgumentFormatException ex)
                {
                    result = null;
                    if (ex.Argument is not null)
                        return $"【错误】\"{ex.ArgumentValue}\"无法解析为{ex.Argument.Type}。";
                    else
                        return "【错误】" + ex.Message;
                }
                catch (CommandArgumentOutOfRangeException ex)
                {
                    result = null;
                    if (ex.Argument?.Range is not null)
                        return $"【错误】\"{ex.ArgumentValue}\"超出了预期范围（最小值：{ex.Argument.Range.MinValue} 最大值：{ex.Argument.Range.MaxValue}）";
                    else
                        return "【错误】" + ex.Message;
                }
                catch (Exception ex)
                {
                    result = null;
                    return catchexception(ex);
                }
            };
        }

        private readonly MethodInfo _method;

        private readonly CommandDelegate _func;

        public ReadOnlyCollection<CommandArgument> Arguments { get; }

        public object? DefaultObject { get; }

        public bool IsStatic => _method.IsStatic;

        public string? Run(object? obj, out object? result, params string[] args) => _func(obj, out result, args);

        public static CommandFunc GetFunc<T>(T func, object? defaultObject = null, Func<object?, string?>? resultout = null, Func<Exception, string?>? catchexception = null) where T : Delegate
        {
            return new(func.Method, defaultObject, resultout, catchexception);
        }

        public delegate string? CommandDelegate(object? obj, out object? result, string[] args);
    }
}
