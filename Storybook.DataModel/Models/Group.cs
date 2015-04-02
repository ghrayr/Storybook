using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Storybook.Common.Utility;

namespace Storybook.DataModel.Models
{
    [Table("Groups")]
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Index("GroupNameIndex")]
        [Display(Name = "Group Name")]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Story> Stories { get; set; }
    }

    [NotMapped]
    public class GroupEx : Group, IPagedList
    {
        [Display(Name = "The number of members")]
        public int MembersCount { get; set; }

        [Display(Name = "The number of stories")]
        public int StoriesCount { get; set; }

        #region IPagedList

        public int TotalRecords { get; set; }
        #endregion
    }
}
