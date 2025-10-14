using ProtechGroup.Domain.DTOs;
using ProtechGroup.Domain;
using System.Collections.Generic;


namespace ProtechGroup.Application.Interfaces
{
    public interface IVietNamAirLinesMethod
    {
        FlightResultOutput BuildFlightResultVietNamAirLines(RootVNA alineVNA, int countPax, bool isDomestric);
        GroupFlight GetGroupFlightVietNamAirLines(ListAirOptionVNA airOption, int FareId, int waytype,
                                                         string sesionId, bool isDomestric, int countPax);
        List<PriceBreakDownFlight> GetPriceBreakDownFlightVN(List<ListFareOptionVNA> fareOpt, decimal serviceFee,
                                                                    int countPax);
        List<Segment> GetListChangBayVN(List<ListSegmentVNA> listSegment, string className);
    }
}
