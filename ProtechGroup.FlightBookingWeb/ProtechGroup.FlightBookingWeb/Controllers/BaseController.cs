using ProtechGroup.FlightBookingWeb.Helpers;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace ProtechGroup.FlightBookingWeb.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
       public NameValueCollection GetValueParaEncryptHelper(string data)
       {
            var decrypted = UrlEncryptHelper.Decrypt(data);
            return  HttpUtility.ParseQueryString(decrypted);
       }
       public string GetEncryptQuery(string plainText)
       {
            return UrlEncryptHelper.Encrypt(plainText);
       }
        [HttpPost]
        public ActionResult EncryptQuery(string plainText)
        {
            if (!string.IsNullOrEmpty(plainText))
                return Content(GetEncryptQuery(plainText));
            return Content(string.Empty);
        }
    }
}