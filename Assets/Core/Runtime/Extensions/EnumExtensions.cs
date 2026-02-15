using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Runtime.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetValues<T>() where T : Enum
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}