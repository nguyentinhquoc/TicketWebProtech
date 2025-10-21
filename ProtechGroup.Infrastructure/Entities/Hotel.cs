using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProtechGroup.Infrastructure.Entities
{
	[Table("Hotels")]
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        public int Rate { get; set; }
        public string Summary { get; set; }
        public string Detail { get; set; }
        public bool Active { get; set; }
        public DateTime? ShowFromDate { get; set; }
        public DateTime? ShowEndDate { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        [MaxLength(50)]
        public string Sale { get; set; }
        public bool IsHot { get; set; }
        public bool IsDomestic { get; set; }
        [MaxLength(250)]
        public string Title { get; set; }
        [MaxLength(250)]
        public string MetaDescription { get; set; }
        [MaxLength(500)]
        public string MetaKeywords { get; set; }
        [MaxLength(250)]
        public string Url { get; set; }
        public string Service { get; set; }
        public string RuleOrderRoom { get; set; }
        public string LocationDetail { get; set; }
    }
}
