using ProtechGroup.Domain.DTOs;
using ProtechGroup.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtechGroup.Application.Interfaces
{
    public interface IVietJetsMethod
    {
        FlightResultOutput BuildFlightResultVietJets(RootVietJets[] alineVJ, int countPax, bool isDomestric);
        GroupFlight GetGroupFlightVietJets(RootVietJets root, int fareId, int wattype, 
                                            int countPax, bool isDomestric);
        List<PriceBreakDownFlight> GetPriceBreakDownFlightVJ(List<FareOption> lFareot, int countPax,
                                                                decimal serviceFee);
        List<Segment> GetListSegmentVJ(List<Flight> flights, string ticketClass);
    }
}
