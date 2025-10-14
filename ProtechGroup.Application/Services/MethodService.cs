using Newtonsoft.Json;
using ProtechGroup.Application.Common;
using ProtechGroup.Application.Interfaces;
using ProtechGroup.Domain;
using ProtechGroup.Domain.DTOs;
using ProtechGroup.Domain.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;


namespace ProtechGroup.Application.Services
{
   
    public class MethodService: IMethodService
    {
        private readonly ISearchInputRepository _searchInputRepository;
        private readonly IServiceFeeService _serviceFeeService;
        private readonly IBamBooAirWaysMethod _bamBooAirWaysMethod;
        private readonly IBambooAirwaysService _bamBooAirWaysService;
        private readonly IVietJetsMethod _vietJetsMethod;
        private readonly IVietJetsService _vietJetsService;
        private readonly IVietNamAirLinesMethod _vietNamAirLinesMethod;
        private readonly IVietNamAirLinesService _vietNamAirLinesService;
        
        public MethodService(ISearchInputRepository searchInputRepository,
                             IServiceFeeService serviceFeeService,
                             IBamBooAirWaysMethod bamBooAirWaysMethod, 
                             IBambooAirwaysService bamBooAirWaysService,
                             IVietJetsMethod vietJetsMethod,
                             IVietJetsService vietJetsService,
                             IVietNamAirLinesService vietNamAirLinesService,
                             IVietNamAirLinesMethod vietNamAirLinesMethod
                             )
        {
            _searchInputRepository = searchInputRepository;
            _bamBooAirWaysMethod = bamBooAirWaysMethod;
            _bamBooAirWaysService = bamBooAirWaysService;
            _vietJetsMethod = vietJetsMethod;
            _vietJetsService = vietJetsService;
            _vietNamAirLinesMethod = vietNamAirLinesMethod;
            _vietNamAirLinesService = vietNamAirLinesService;
            _serviceFeeService = serviceFeeService;
        }

        public async Task<FlightResultOutput> GetFlightDomestic(int sessionId)
        {
            var result = new FlightResultOutput();
            RootBamBoo alinesBB = new RootBamBoo();
            RootVietJets[] alineVJ = new RootVietJets[100];
            RootVNA alinesVN = new RootVNA();
            var searchInput = _searchInputRepository.GetByKeySessionId(sessionId);
            if (CoreUtils.IsFileEmptyOrDoesntExist(CoreUtils.GetBamBooResponseFilePath(sessionId)))
            {
                string bodypost = _bamBooAirWaysMethod.GetBodyAirAvailability(searchInput);
                alinesBB = await _bamBooAirWaysService.GetAlinesBamBoo(bodypost);
                CoreUtils.WriteToFile(CoreUtils.GetBamBooResponseFilePath(sessionId), JsonConvert.SerializeObject(alinesBB));
            }
            else
            {
                alinesBB = JsonConvert.DeserializeObject<RootBamBoo>(CoreUtils.GetContentFromFile(CoreUtils.GetBamBooResponseFilePath(sessionId)));
            }
            if (CoreUtils.IsFileEmptyOrDoesntExist(CoreUtils.GetVietJetResponseFilePath(sessionId)))
            {
                string strRequest ="?cityPair=" + searchInput.DepartureAirport + "-" + searchInput.ArrivalAirport;
                strRequest += "&departure=" + searchInput.DepartureDate.ToString("yyyy-MM-dd");
                strRequest += "&cabinClass=Y";
                strRequest += "&currency=VND";
                strRequest += "&adultCount=" + searchInput.AdultNumber;
                strRequest += "&childCount=" + searchInput.ChildNumber;
                strRequest += "&infantCount=" + searchInput.InfantNumber;
                if (searchInput.IsRoundTrip)
                    strRequest += "&return=" + searchInput.ReturnDate?.ToString("yyyy-MM-dd");
                strRequest += "&company=i5M1ALKO4jozmgXmq3Cp8cSS56eS3V1GxLk1n¥I69CE=";
                alineVJ = await _vietJetsService.GetAlinesVietJets(strRequest);
                CoreUtils.WriteToFile(CoreUtils.GetVietJetResponseFilePath(sessionId), JsonConvert.SerializeObject(alineVJ));
            }
            else
            {
                alineVJ = JsonConvert.DeserializeObject<RootVietJets[]>(CoreUtils.GetContentFromFile(CoreUtils.GetVietJetResponseFilePath(sessionId)));
            }
            if (CoreUtils.IsFileEmptyOrDoesntExist(CoreUtils.GetVietNamAirLinesResponseFilePath(sessionId)))
            {
                string bodyPost = "{" +
                                       "\"adt\": " + searchInput.AdultNumber + "," +
                                       "\"chd\": " + searchInput.ChildNumber + "," +
                                       "\"inf\": " + searchInput.InfantNumber + "," +
                                       "\"listRoute\": [" +
                                               "{" +
                                                   "\"leg\": 0," +
                                                   "\"startPoint\": \"" + searchInput.DepartureAirport + "\"," +
                                                   "\"endPoint\": \"" + searchInput.ArrivalAirport + "\"," +
                                                   "\"departDate\": \"" + searchInput.DepartureDate.ToString("ddMMyyyy") + "\"" +
                                                "}";
                if (searchInput.IsRoundTrip)
                {
                    bodyPost += ",{" +
                            "\"leg\": 1," +
                            "\"startPoint\": \"" + searchInput.ArrivalAirport + "\"," +
                            "\"endPoint\": \"" + searchInput.DepartureAirport + "\"," +
                            "\"departDate\": \"" + searchInput.ReturnDate?.ToString("ddMMyyyy") + "\"" +
                        "}";
                }
                bodyPost += "]," +
                        "\"Option\": { " +
                        "\"DirectOnly\": true " +
                        "}" +
                "}";
                alinesVN = await _vietNamAirLinesService.SearchFlightVietNamAirLines(bodyPost);
                CoreUtils.WriteToFile(CoreUtils.GetVietNamAirLinesResponseFilePath(sessionId), JsonConvert.SerializeObject(alinesVN));
                
            }
            else
            {
                alinesVN = JsonConvert.DeserializeObject<RootVNA>(CoreUtils.GetContentFromFile(CoreUtils.GetVietNamAirLinesResponseFilePath(sessionId)));
            }
            var flightResultBB = _bamBooAirWaysMethod.BuildFlightResultBamBoo(alinesBB, searchInput.TotalPax, searchInput.IsSearchDomestic);
            var flightResultVJ = _vietJetsMethod.BuildFlightResultVietJets(alineVJ, searchInput.TotalPax, searchInput.IsSearchDomestic);
            var flightResultVN = _vietNamAirLinesMethod.BuildFlightResultVietNamAirLines(alinesVN, searchInput.TotalPax, searchInput.IsSearchDomestic);
            result.BlockItems = new List<BlockItem>();
            result.Airlines = new List<FlightResultOutput.Airline>();
            result.BlockItems.Add(new BlockItem());
            result.BlockItems[0].FlightOutBounds = new List<GroupFlight>();
            if (flightResultBB != null && flightResultBB.BlockItems.Count > 0)
            {
                var airline = new FlightResultOutput.Airline();
                airline.AirlineName = "BambooAirWays";
                airline.AirlineCode = "QH";
                result.Airlines.Add(airline);
                flightResultBB.BlockItems[0].FlightOutBounds.ForEach(f => {
                    result.BlockItems[0].FlightOutBounds.Add(f);
                });
            }
            if(flightResultVJ != null && flightResultVJ.BlockItems.Count > 0)
            {
                var airline = new FlightResultOutput.Airline();
                airline.AirlineName = "Vietjet Air";
                airline.AirlineCode = "VJ";
                result.Airlines.Add(airline);
                flightResultVJ.BlockItems[0].FlightOutBounds.ForEach(f => {
                    result.BlockItems[0].FlightOutBounds.Add(f);
                });
            }
            if(flightResultVN != null && flightResultVN.BlockItems.Count > 0)
            {
                var airline = new FlightResultOutput.Airline();
                airline.AirlineName = "VietNam AirLines";
                airline.AirlineCode = "VN";
                result.Airlines.Add(airline);
                flightResultVN.BlockItems[0].FlightOutBounds.ForEach(f => {
                    result.BlockItems[0].FlightOutBounds.Add(f);
                });
            }
            return result;
        }
    }
}
