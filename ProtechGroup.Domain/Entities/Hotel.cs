using System;

namespace ProtechGroup.Domain.Entities
{
    public class HotelMod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int City { get; set; }
        public int Rate { get; set; }
        public string Summary { get; set; }
        public string Detail { get; set; }
        public bool Active { get; set; }
        public DateTime? ShowFromDate { get; set; }
        public DateTime? ShowEndDate { get; set; }
        public decimal Price { get; set; }
        public string Sale { get; set; }
        public bool IsHot { get; set; }
        public bool IsDomestic { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Url { get; set; }
        public string Service { get; set; }
        public string RuleOrderRoom { get; set; }
        public string LocationDetail { get; set; }
    }
}
