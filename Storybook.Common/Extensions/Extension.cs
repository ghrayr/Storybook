using System;
using System.Collections.Generic;
using Storybook.Common.Utility;

namespace Storybook.Common.Extensions
{
    public static class Extension
    {
        /// <summary>
        /// Make the given list as PagedList object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>PagedList</returns>
        public static PagedList<T> ToPagedList<T>(this IList<T> list, int page, int pageSize)
            where T : class, IPagedList
        {
            var paged = new PagedList<T>(list,
                page,
                pageSize,
                list.Count > 0 ? (int) Math.Ceiling((double) list[0].TotalRecords/pageSize) : 0,
                list.Count > 0 ? list[0].TotalRecords : 0);

            return paged;
        }
    }
}
