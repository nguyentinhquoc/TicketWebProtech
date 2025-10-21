using AutoMapper;
using ProtechGroup.Application.Interfaces;
using ProtechGroup.Application.Services;
using ProtechGroup.Domain.Entities;
using ProtechGroup.Infrastructure.Contexts; 
using ProtechGroup.Infrastructure.Entities;
using ProtechGroup.Infrastructure.Mapping;
using ProtechGroup.Infrastructure.Repositories;
using ProtechGroup.FlightBookingWeb.ViewModel;
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
        private readonly IHotelService _hotelService;
        
        public HomeController(INewsService newsService, IHotelService hotelService)
        {
            _newsService = newsService;
            _hotelService = hotelService;
        }
        
        public ActionResult Index()
        {
            // Lấy dữ liệu hotels và news
            var hotels = _hotelService.GetAllHotels();
            var modelNew = _newsService.GetAllNews();
            
            // Tạo ViewModel
            var viewModel = new HomeViewModel
            {
                Hotels = hotels,
                News = modelNew
            };
            
            return View(viewModel);
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