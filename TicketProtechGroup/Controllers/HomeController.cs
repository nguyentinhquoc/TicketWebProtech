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

            List<RootVietJets> rootDataListVietJet = JsonConvert.DeserializeObject<List<RootVietJets>>(jsonContentVietJet);
            //List<RootBamboo> rootDataListBamboo = JsonConvert.DeserializeObject<List<RootBamboo>>(jsonContentBamboo);
            RootBamboo rootDataListBamboo = JsonConvert.DeserializeObject<RootBamboo>(jsonContentBamboo);

            List<GroupFlight> allGroupFlights = new List<GroupFlight>();
            //foreach (var flight in rootDataListVietJet)
            //{
            //    // Gọi hàm lấy GroupFlight theo từng phần tử (thay các tham số nếu cần)
            //    GroupFlight groupFlight = VietjetAir.GetGroupFlightVietJets(flight, fareId: 123, waytype: 0, countPax: 1);
            //    // Thêm kết quả vào danh sách
            //    allGroupFlights.Add(groupFlight);
            //}

            foreach (var flight in rootDataListBamboo.data[0].trip_info)
            {
                // Gọi hàm lấy GroupFlight theo từng phần tử (thay các tham số nếu cần)
                GroupFlight groupFlight = BamBooAirWays.GetGroupFlightBamBoo(flight, waytype: 0, keyroot: "KEYROOT");
                // Thêm kết quả vào danh sách
                allGroupFlights.Add(groupFlight);
            }

            // Trả về JSON gồm tất cả group flight
            return Json(new { message = "all flights", data = allGroupFlights }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Search()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

    }
}