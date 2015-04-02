using System;
using System.Collections.Generic;
using System.Linq;

namespace Storybook.Common.Utility
{
    public class PagedList<T> : IPagedList
    {
        #region Properties

        public List<T> Data { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int TotalRecords { get; set; }

        public T this[int index] { get { return Data[index]; } }

        public bool HasRecords { get { return Data != null && Data.Any(); } }

        #endregion

        #region ctor

        public PagedList()
        {
        }

        public PagedList(IEnumerable<T> data, int page, int pageSize, int pageCount, int totalRecords)
        {
            Data = new List<T>(data);
            Page = page;
            PageSize = pageSize;
            PageCount = pageCount;
            TotalRecords = totalRecords;
        }

        #endregion
    }
}
