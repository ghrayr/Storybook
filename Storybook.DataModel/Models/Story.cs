using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Storybook.Common.Utility;

namespace Storybook.DataModel.Models
{
    [Table("Stories")]
    public class Story
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        [MinLength(3)]
        [MaxLength(100)]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [MaxLength(256)]
        public string Description { get; set; }

        [Display(Name = "Content")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Posted On")]
        public DateTime PostedOn { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        [Required]
        [NotMapped]
        public List<int> GroupIds { get; set; }
    }

    [NotMapped]
    public class StoryEx : Story, IPagedList
    {
        public string GroupIdsString { get; set; }

        #region IPagedList

        public int TotalRecords { get; set; }
        #endregion
    }
}
