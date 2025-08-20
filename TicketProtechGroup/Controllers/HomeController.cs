using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketProtechGroup.service.BindApiFlight;
using TicketProtechGroup.service.BindApiFlight.RootFlight;


namespace TicketProtechGroup.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            // Đọc json từ file => Json
            string jsonContentVietJet = System.IO.File.ReadAllText(Server.MapPath("~/Session/Response.FlightVietJet.txt"));
            string jsonContentBamboo = System.IO.File.ReadAllText(Server.MapPath("~/Session/Response.FlightBamBoo.txt"));

            // Convert sang Obj
            List<RootVietJets> rootDataListVietJet = JsonConvert.DeserializeObject<List<RootVietJets>>(jsonContentVietJet);
            RootBamboo rootDataListBamboo = JsonConvert.DeserializeObject<RootBamboo>(jsonContentBamboo);

            List<FlightResultOutput> outPutFlight = new List<FlightResultOutput>();

            // Gọi 1 lần cho toàn bộ Vietjet data
            FlightResultOutput vietjetGroupFlight = VietjetAir.BuildRootVietJets(rootDataListVietJet.ToArray(), 1);
            outPutFlight.Add(vietjetGroupFlight);

            // Lặp Bamboo như cũ

            // Trả về View
            return View(outPutFlight);
        }


        [HttpPost]
        [Route("Search/Index")]
        public ActionResult Search()
        {
            List<FlightResultOutput> outPutFlight = new List<FlightResultOutput>();
            // Doc file
           string jsonContentVietJet = System.IO.File.ReadAllText(Server.MapPath("~/Session/Response.FlightVietJet.txt"));
            string jsonContentBamboo = System.IO.File.ReadAllText(Server.MapPath("~/Session/Response.FlightBamBoo.txt"));
            string jsonContentVietNamA = System.IO.File.ReadAllText(Server.MapPath("~/Session/Response.FlightVNA.txt"));

            // Convert sang Obj
            List<RootVietJets> rootDataListVietJet = JsonConvert.DeserializeObject<List<RootVietJets>>(jsonContentVietJet);
            RootBamboo rootDataListBamboo = JsonConvert.DeserializeObject<RootBamboo>(jsonContentBamboo);

            // Gọi 1 lần cho toàn bộ Vietjet data
            FlightResultOutput vietjetGroupFlight = VietjetAir.BuildRootVietJets(rootDataListVietJet.ToArray(), 1);
            outPutFlight.Add(vietjetGroupFlight);
            // Lặp Bamboo như cũ
            FlightResultOutput bambooGroupFlight = BamBooAirWays.BuildRootBamboo(rootDataListBamboo, 1);
            outPutFlight.Add(bambooGroupFlight);
            //return Json(outPutFlight, JsonRequestBehavior.AllowGet);
            // Trả về View
            return View(outPutFlight);
        }

    }
}