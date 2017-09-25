using System.Linq;
using System.Web.Mvc;
using MeetingRoomBookingSystem.Models;
using Microsoft.AspNet.Identity;


namespace MeetingRoomBookingSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var database = new MeetingRoomBookingSystemDbContext())
            {
                return RedirectToAction("Index", "Office");
            }
        }
    }
}