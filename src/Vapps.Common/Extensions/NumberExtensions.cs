using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Extensions
{
    public static class NumberExtensions
    {
        public static bool LargerThanZero(this int value)
        {
            return value > 0;
        }

        public static bool LargerThanZero(this int? value)
        {
            return value.HasValue && value > 0;
        }
    }
}
