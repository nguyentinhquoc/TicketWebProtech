using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProtechGroup.service.BindApiFlight.RootFlight;

namespace TicketProtechGroup.service.BindApiFlight
{
    //internal class BuildFlightResultBamboo
    //{
    //    internal static FlightResultOutput Build(RootBamboo alineBB)
    //    {
    //        var flightResultOutput = new FlightResultOutput();
    //        flightResultOutput.IsFlightDomestic = true;
    //        flightResultOutput.BlockItems = new List<BlockItem>();
    //        flightResultOutput.Airlines = new List<FlightResultOutput.Airline>();
    //        var airline = new FlightResultOutput.Airline();
    //        airline.AirlineName = "BamBooAirways";
    //        airline.AirlineCode = "QH";

    //        flightResultOutput.Airlines.Add(airline);
    //        BlockItem blockItem = new BlockItem();
    //        blockItem.FlightOutBounds = new List<GroupFlight>();
    //        blockItem.FlightInBounds = new List<GroupFlight>();
    //        if (alineBB != null && alineBB.data.Count > 0)
    //        {
    //            for (int i = 0; i < alineBB.data[0].trip_info.Count; i++)
    //            {
    //                var gf = BamBooAirWays.GetGroupFlightBamBoo(alineBB.data[0].trip_info[i], 0, alineBB.id);
    //                if (gf != null) { }
    //                    //blockItem.FlightOutBounds.Add(gf);
    //            }
    //            if (alineBB.data.Count > 1)
    //            {
    //                for (int i = 0; i < alineBB.data[1].trip_info.Count; i++)
    //                {
    //                    var gf = BamBooAirWays.GetGroupFlightBamBoo(alineBB.data[1].trip_info[i], 1, alineBB.id);
    //                    if (gf != null) { }
    //                        //blockItem.FlightInBounds.Add(gf);
    //                }
    //            }
    //            if (blockItem.FlightInBounds.Count > 0)
    //                blockItem.IsRoundTrip = true;
    //        }
    //        flightResultOutput.BlockItems.Add(blockItem);
    //        return flightResultOutput;
    //    }

    //}
}
