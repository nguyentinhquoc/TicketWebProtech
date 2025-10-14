using ProtechGroup.Application.Interfaces;
using ProtechGroup.Application.Services;
using ProtechGroup.Domain.Entities;
using ProtechGroup.Infrastructure.Contexts; 
using ProtechGroup.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProtechGroup.FlightBookingWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;
        public HomeController(INewsService newsService) {
            _newsService = newsService;
        }
        public ActionResult Index()
        {
            var modelNew = _newsService.GetAllNews();
            return View(modelNew);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}