using ProtechGroup.Application.Common;
using ProtechGroup.Domain.DTOs;
using ProtechGroup.Domain;
using System.Collections.Generic;
using ProtechGroup.Infrastructure.Entities;
using ProtechGroup.Domain.Entities;
namespace ProtechGroup.Application.Interfaces
{
    public interface IBamBooAirWaysMethod
    {
        FlightResultOutput BuildFlightResultBamBoo(RootBamBoo rootBamBoo, int countPax, bool isDomestric);
        GroupFlight GetGroupFlightBamBoo(TripInfoBamBoo tripInfo, int waytype,
                                        string keyroot, int countPax, bool isDomestric);
        List<PriceBreakDownFlight> GetPriceBreakDownFlightQH(List<BookingClassBamBoo> booKings, 
                                                                        decimal serviceFee, int countPax);
        List<Segment> GetListSegmentQH(List<SegmentInfoBamBoo> segment_info);
        string GetBodyAirAvailability(SearchInputMod searchInput);

    }
}
