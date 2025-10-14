using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtechGroup.FlightBookingWeb.Models
{
    public class FlightSearchRequest
    {
        public string departure { get; set; }
        public string arrival { get; set; }
        public DateTime departureDate { get; set; }
        public DateTime? returnDate { get; set; }
        public int roundType { get; set; }
        public byte countAdt { get; set; }
        public byte countChd { get; set; }
        public byte countInf { get; set; }
    }
}