using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GogsKit.Internal
{
    internal class DateTimeOffsetStringConvert
    {
        public static string Serialize(DateTimeOffset? value)
        {
            return value.HasValue ? Serialize(value.Value) : null;
        }

        public static string Serialize(DateTimeOffset value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:sszzz");
        }

        public static DateTimeOffset? Deserialize(string value)
        {
            DateTimeOffset maybeResult;
            if (!DateTimeOffset.TryParse(value, out maybeResult))
            {
                return null;
            }
            return maybeResult;
        }
    }
}
