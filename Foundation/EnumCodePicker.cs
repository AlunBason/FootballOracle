using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballOracle.Foundation
{
    public class EnumCodePicker
    {
        public int Code { get; set; }
        public string Description { get; set; }

        public static IEnumerable<EnumCodePicker> Of<TEnum>()
        {
            Type t = typeof(TEnum);
            if (t.IsEnum)
            {
                return from Enum e in Enum.GetValues(t)
                       select new EnumCodePicker { Code = Convert.ToInt32(e), Description = e.ToSpacedString() };
            }

            return null;
        }
    }
}
