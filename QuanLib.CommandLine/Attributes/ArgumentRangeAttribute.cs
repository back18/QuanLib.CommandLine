using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
    public sealed class ArgumentRangeAttribute : Attribute
    {
        public ArgumentRangeAttribute(Type type, string minValue, string maxValue)
        {
            ArgumentNullException.ThrowIfNull(type, nameof(type));
            ArgumentException.ThrowIfNullOrEmpty(minValue, nameof(minValue));
            ArgumentException.ThrowIfNullOrEmpty(maxValue, nameof(maxValue));

            ObjectParser parser = ObjectParser.GetParser(type) ?? throw new ArgumentException(nameof(type) + " 没有 Parse 方法");
            if (!parser.TryParse(minValue, out var omin))
                throw new ArgumentException($"{minValue} 无法被解析为 {type}");
            if (!parser.TryParse(maxValue, out var omax))
                throw new ArgumentException($"{maxValue} 无法被解析为 {type}");

            if (omin is not IComparable imin || omax is not IComparable imax)
                throw new ArgumentException("参数类型必须实现 IComparable 接口");

            Range = new(imin, imax);
        }

        public ArgumentRangeAttribute(object minValue, object maxValue)
        {
            ArgumentNullException.ThrowIfNull(minValue, nameof(minValue));
            ArgumentNullException.ThrowIfNull(maxValue, nameof(maxValue));
            if (minValue.GetType() != maxValue.GetType())
                throw new ArgumentException($"参数的类型必须一致");

            if (minValue is not IComparable imin || maxValue is not IComparable imax)
                throw new ArgumentException("参数类型必须实现 IComparable 接口");

            Range = new(imin, imax);
        }

        public ObjectRange Range { get; }
    }
}
