using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtechGroup.Domain.Entities
{
    public class NewsMod
    {

        public int Id { get; set; }
        public int NewsGroupID { get; set; }

        public string Name { get; set; }
        

        public string Url { get; set; }
        

        public string Title { get; set; }
        

        public string MetaDescription { get; set; }
        

        public string MetaKeywords { get; set; }
        


        public string Summary { get; set; }
        

        public string ImageUrl { get; set; }
        public string Content { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool AllowComment { get; set; }

        public bool ShowOnHomepage { get; set; }

        public bool Active { get; set; }
    }
}
