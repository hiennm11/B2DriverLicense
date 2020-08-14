using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace B2DriverLicense.AppCrawler
{
    public static class Extensions
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
                => self?.Select((item, index) => (item, index)) ?? new List<(T, int)>();
    }
}
