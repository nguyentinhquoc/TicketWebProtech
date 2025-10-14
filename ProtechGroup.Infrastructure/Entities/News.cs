using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProtechGroup.Infrastructure.Entities
{
    [Table("News")]
    public class News
    {
        [Key]
        public int Id { get; set; }
        public int NewsGroupID{ get; set; }

        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]

        public string Url { get; set; }
        [StringLength(250)]

        public string Title { get; set; }
        [StringLength(250)]

        public string MetaDescription { get; set; }
        [StringLength(250)]

        public string MetaKeywords { get; set; }
        [StringLength(250)]


        public string Summary { get; set; }
        [StringLength(250)]

        public string ImageUrl { get; set; }
        public string Content{ get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool AllowComment { get; set; }

        public bool ShowOnHomepage  { get; set; }

        public bool Active { get; set; }
    }
}
