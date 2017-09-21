using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MeetingRoomBookingSystem.Models;

namespace MeetingRoomBookingSystem.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        // GET: Reservation
        public ActionResult Index()
        {
            return View();
        }

        //GET: Reservation/Create
        public ActionResult Create()
        {
            using (var database = new MeetingRoomBookingSystemDbContext())
            {
                var model = new ReservationViewModel
                {
                    MeetingRooms = database
                    .MeetingRooms
                    .OrderBy(m => m.Name)
                    .ToList()
                };

                return View(model);
            }
        }

        //POST: Reservation/Create
        [HttpPost]
        public ActionResult Create(ReservationViewModel model)
        {
            using (var database = new MeetingRoomBookingSystemDbContext())
            {
                if (ModelState.IsValid)
                {
                    var reservations = database
                        .Reservations
                        .Where(r => r.MeetingRoomId == model.MeetingRoomId)
                        .Where(r => r.StartDate < model.StartDate && model.StartDate < r.EndDate)
                        .ToList();
                    //TO DO
                        //if(reservations
                        //&& r.StartDate.Hour >= model.StartDate.Hour
                        //&& r.StartDate.Minute >)

                    var userId = database
                        .Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .First()
                        .Id;

                    var reservation = new Reservation(userId, model.MeetingRoomId, model.StartDate, model.EndDate, model.Description);

                    database.Reservations.Add(reservation);
                    database.SaveChanges();

                    return RedirectToAction("Details", new { id = reservation.Id });
                }

                model.MeetingRooms = database
                    .MeetingRooms
                    .OrderBy(m => m.Name)
                    .ToList();

                return View(model);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new MeetingRoomBookingSystemDbContext())
            {
                var reservation = database
                    .Reservations
                    .Where(r => r.Id == id)
                    .First();

                if (reservation == null)
                {
                    return HttpNotFound();
                }

                return View(reservation);
            }
        }

        private bool IsUserAuthorizedToEdit(Reservation reservation)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = reservation.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }
    }
}