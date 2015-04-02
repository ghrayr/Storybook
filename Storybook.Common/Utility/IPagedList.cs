using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Storybook.Common.Utility
{
    public interface IPagedList
    {
        int TotalRecords { get; set; }
    }
}
