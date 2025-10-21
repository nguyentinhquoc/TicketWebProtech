using System.Web.Mvc;
using AutoMapper;
using ProtechGroup.Application.Services;
using ProtechGroup.Infrastructure.Mapping;
using ProtechGroup.Infrastructure.Contexts;
using ProtechGroup.Infrastructure.Repositories;

namespace ProtechGroup.FlightBookingAdmin.Controllers
{
	public class HotelController : Controller
	{
		public ActionResult Index()
		{
			var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
			IMapper mapper = mapperConfig.CreateMapper();
			using (var db = new ApplicationDbContext())
			{
				var repo = new HotelRepository(db, mapper);
				var service = new HotelService(repo);
				var hotels = service.GetAllHotels();
				return Json(hotels, JsonRequestBehavior.AllowGet);
			}
		}
	}
}


