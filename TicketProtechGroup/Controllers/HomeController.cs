using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketProtechGroup.service;
using TicketProtechGroup.service.BindApiFlight;
using TicketProtechGroup.service.BindApiFlight.RootFlight;
using TicketProtechGroup.service.DbService;
using TicketProtechGroup.service.Request;
using TicketProtechGroup.ViewModels;
using DataAirport = TicketProtechGroup.data.Airport;


namespace TicketProtechGroup.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AirportRepository airportRepository;

        public HomeController()
        {
            airportRepository = new AirportRepository();
        }
        public ActionResult Index()
        {
            var vm = new HomeViewModel()
            {
                SearchViewModel = new SearchViewModel()
                {
                    Airports = airportRepository.GetAirportByCountryCode("VN")
                } 
            };
            return View(vm);
        }


        [HttpPost]
        [Route("Search/Index")]
        public ActionResult Search(FlightSearchRequest request)
        {
            var form = Request.Form;
            var searchRequest = new FlightSearchRequest
            {
                Type = form["type"],
                SeatClass = form["seatClass"],
                Airline = form["airline"],
                NguoiLon = int.Parse(form["nguoiLon"] ?? "0"),
                TreEm = int.Parse(form["treEm"] ?? "0"),
                EmBe = int.Parse(form["emBe"] ?? "0"),
                Chang = new List<ChangModel>()
            };
            // Tìm các chang gửi lên

            int i = 1;
            while (form[$"chang{i}[diemDi]"] != null)
            {
                var chang = new ChangModel
                {
                    DiemDi = form[$"chang{i}[diemDi]"],
                    DiemDen = form[$"chang{i}[diemDien]"],
                    NgayDi = DateTime.ParseExact(form[$"chang{i}[ngayDi]"], "dd/MM/yyyy", null),
                    NgayVe = string.IsNullOrEmpty(form[$"chang{i}[ngayVe]"]) ? (DateTime?)null :
                              DateTime.ParseExact(form[$"chang{i}[ngayVe]"], "dd/MM/yyyy", null)
                };
                searchRequest.Chang.Add(chang);
                i++;
            }

            // Tính tông người
            var countPax = searchRequest.TreEm + searchRequest.NguoiLon + searchRequest.EmBe;
            List<FlightResultOutput> outPutFlight = new List<FlightResultOutput>();
            // CALL API
            if(searchRequest.Airline == "all")
            {
                //Call cả 3 hãng
            }
            if (searchRequest.Airline == "VJ" || searchRequest.Airline == "all")
            {
                string jsonContentVietJet = System.IO.File.ReadAllText(Server.MapPath("~/Session/Response.FlightVietJet.txt")); //Call API VIETJET
                List<RootVietJets> rootDataListVietJet = JsonConvert.DeserializeObject<List<RootVietJets>>(jsonContentVietJet);
                FlightResultOutput vietjetGroupFlight = VietjetAir.BuildRootVietJets(rootDataListVietJet.ToArray(), countPax);
                outPutFlight.Add(vietjetGroupFlight);
            }
            if (searchRequest.Airline == "VN" || searchRequest.Airline == "all")
            {
                //Call API VIETNAMAIRLINE
            }
            if (searchRequest.Airline == "QH" || searchRequest.Airline == "all")
            {
                
                string jsonContentBamboo = System.IO.File.ReadAllText(Server.MapPath("~/Session/Response.FlightBamBoo.txt"));  //Call API BAMBOO
                RootBamboo rootDataListBamboo = JsonConvert.DeserializeObject<RootBamboo>(jsonContentBamboo); 
                FlightResultOutput bambooGroupFlight = BamBooAirWays.BuildRootBamboo(rootDataListBamboo, countPax);
                outPutFlight.Add(bambooGroupFlight);
            }
            string jsonContentVietNamA = System.IO.File.ReadAllText(Server.MapPath("~/Session/Response.FlightVNA.txt"));
            // CALL API

            // Duyệt qua tất cả BlockItems
            foreach (var flightItem in outPutFlight)
            {
                if (flightItem.BlockItems != null)
                {
                    foreach (var block in flightItem.BlockItems)
                    {
                        if (block.FlightOutBounds != null)
                        {
                            block.FlightOutBounds = block.FlightOutBounds
                                .Where(fb => fb.ListHangVes != null && fb.ListHangVes.Any())
                                .ToList();
                        }
                    }
                }
            }



            return View(outPutFlight);
        }

    }
}