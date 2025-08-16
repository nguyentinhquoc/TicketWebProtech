
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketProtechGroup.data;
namespace TicketProtechGroup.Controllers
{
    
    public class BaseController : Controller
    {
       public SkytourWebEntities2 db;
       public BaseController() {
        db = new SkytourWebEntities2();
        }
    }
}