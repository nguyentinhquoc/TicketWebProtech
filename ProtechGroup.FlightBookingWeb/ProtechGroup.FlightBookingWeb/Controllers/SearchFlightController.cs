using ProtechGroup.FlightBookingWeb.Helpers;
using System;
using System.Web;
using System.Web.Mvc;
using ProtechGroup.Application.Interfaces;
using System.Threading.Tasks;

namespace ProtechGroup.FlightBookingWeb.Controllers
{
    public class SearchFlightController : Controller
    {
        private readonly IMethodService _methodServie;
        private readonly ISearchInputService _searchInputService;
        public SearchFlightController(IMethodService methodServie,
                                      ISearchInputService searchInputService
                                     )
        {
            _methodServie = methodServie;
            _searchInputService = searchInputService;
        }
        // GET: SearchFlight
        public async Task<ActionResult> ResultsFlightDomestic(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    ViewBag.ErrorMessage = "Truy cập không hợp lệ";
                    return View("Error");
                }
                var decrypted = UrlEncryptHelper.Decrypt(data);
                var values = HttpUtility.ParseQueryString(decrypted);
                var sessionId = values["sessionId"];
                var searchInput = _searchInputService.GetByKeySessionId(Convert.ToInt32(sessionId));
                if (searchInput.IsSearchDomestic)
                {
                    var flightSearch = await _methodServie.GetFlightDomestic(Convert.ToInt32(sessionId));
                    ViewBag.SearchInput = searchInput;
                    return View(flightSearch);
                }
                else
                {
                    return RedirectToAction("/Ketquachuyenbayquocte");
                }
            }
            catch
            {

                ViewBag.ErrorMessage = "Truy cập không hợp lệ";
                return View("Error");
            }
        }
    }
}