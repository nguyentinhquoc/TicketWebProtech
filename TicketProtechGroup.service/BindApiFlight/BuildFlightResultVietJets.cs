using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProtechGroup.service.BindApiFlight.RootFlight;

namespace TicketProtechGroup.service.BindApiFlight
{
    internal class BuildFlightResultVietJets
    {
        internal static FlightResultOutput Build(RootVietJets[] alineVJ, int countPax)
        {
            var flightResultOutput = new FlightResultOutput();
            flightResultOutput.IsFlightDomestic = true;
            flightResultOutput.BlockItems = new List<BlockItem>();
            flightResultOutput.Airlines = new List<FlightResultOutput.Airline>();
            var airline = new FlightResultOutput.Airline();
            airline.AirlineName = "VietJetAir";
            airline.AirlineCode = "VJ";

            flightResultOutput.Airlines.Add(airline);
            BlockItem blockItem = new BlockItem();
            blockItem.FlightOutBounds = new List<GroupFlight>();
            blockItem.FlightInBounds = new List<GroupFlight>();
            if (alineVJ != null && alineVJ.Length > 0)
            {
                string departureAirport = alineVJ[0].cityPair.identifier.Split('-')[0];
                string ArrivalAirport = alineVJ[0].cityPair.identifier.Split('-')[1];
                for (int i = 0; i < alineVJ.Length; i++)
                {

                    if (alineVJ[i].cityPair.identifier.Equals(departureAirport + "-" + ArrivalAirport))
                    {
                        var gf = VietjetAir.GetGroupFlightVietJets(alineVJ[i], i, 0, countPax);
                        if (gf != null)
                            blockItem.FlightOutBounds.Add(gf);
                    }
                    else
                    {
                        var gf = VietjetAir.GetGroupFlightVietJets(alineVJ[i], i, 1, countPax);
                        if (gf != null)
                            blockItem.FlightInBounds.Add(gf);
                    }
                }
                if (blockItem.FlightInBounds.Count > 0)
                    blockItem.IsRoundTrip = true;
            }
            flightResultOutput.BlockItems.Add(blockItem);
            return flightResultOutput;
        }


    }
}
