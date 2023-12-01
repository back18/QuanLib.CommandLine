using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.CommandLine
{
    public class ObjectRange
    {
        public ObjectRange(IComparable minValue, IComparable maxValue)
        {
            if (minValue.GetType() != maxValue.GetType())
                throw new ArgumentException($"参数的类型必须一致");

            MinValue = minValue;
            MaxValue = maxValue;
        }

        public IComparable MinValue { get; }

        public IComparable MaxValue { get; }

        public bool IsRange(object value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            if (value.GetType() != MinValue.GetType())
                throw new ArgumentException("类型必须为 " + MinValue.GetType(), nameof(value));

            if (MinValue.CompareTo(value) > 0 || MaxValue.CompareTo(value) < 0)
                return false;
            return true;
        }

        public override string ToString()
        {
            return $"(MinValue: {MinValue} MaxValue: {MaxValue})";
        }
    }
}
