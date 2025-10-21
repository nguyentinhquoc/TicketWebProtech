using ProtechGroup.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProtechGroup.FlightBookingWeb.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<NewsMod> News { get; set; }
        public IEnumerable<HotelMod> Hotels { get; set; }
    }
}