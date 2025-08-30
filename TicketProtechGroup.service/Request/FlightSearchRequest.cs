using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProtechGroup.service.Request
{
    public class FlightSearchRequest
    {
        public string Type { get; set; }
        public string SeatClass { get; set; } // "" là tất cả
        public string Airline { get; set; } // "" là tất cả
        public int NguoiLon { get; set; }
        public int TreEm { get; set; }
        public int EmBe { get; set; }

        public List<ChangModel> Chang { get; set; }
    }

    public class ChangModel
    {
        public string DiemDi { get; set; }
        public string DiemDen { get; set; }
        public DateTime NgayDi { get; set; }
        public DateTime? NgayVe { get; set; }
    }
}
