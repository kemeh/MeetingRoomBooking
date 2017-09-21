using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;
using MeetingRoomBookingSystem.Models;
using Microsoft.Ajax.Utilities;

namespace MeetingRoomBookingSystem.Controllers
{
    [Authorize]
    public class MeetingRoomController : Controller
    {
        // GET: MeetinRoom
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var databse = new MeetingRoomBookingSystemDbContext())
            {
                var meetingRoom = databse
                    .MeetingRooms
                    .Where(m => m.Id == id)
                    .Include(m => m.Office)
                    .FirstOrDefault();

                if (meetingRoom == null)
                {
                    return HttpNotFound();
                }

                return View(meetingRoom);
            }
        }

        //GET: MeetinRoom/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            using (var database = new MeetingRoomBookingSystemDbContext())
            {
                var model = new MeetingRoomViewModel
                {
                    Offices = database
                        .Offices
                        .OrderBy(o => o.Name)
                        .ToList()
                };

                return View(model);
            }
        }

        //POST: MeetingRoom/Create
        [HttpPost]
        public ActionResult Create(MeetingRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new MeetingRoomBookingSystemDbContext())
                {
                    var meetingRoom = new MeetingRoom(model.Name,
                        model.Capacity,
                        model.HasWorkstations,
                        model.HasMultimedia,
                        model.HasWhiteboard,
                        model.OfficeId);

                    database.MeetingRooms.Add(meetingRoom);
                    database.SaveChanges();

                    return RedirectToAction("Details", new { id = meetingRoom.Id });
                }
            }

            return View(model);
        }

        //GET: MeetingRoom/Edit
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new MeetingRoomBookingSystemDbContext())
            {
                var meetingRoom = database
                        .MeetingRooms
                        .Where(m => m.Id == id)
                        .First();

                if (meetingRoom == null)
                {
                    return HttpNotFound();
                }

                var model = new MeetingRoomViewModel
                {
                    Id = meetingRoom.Id,
                    Name = meetingRoom.Name,
                    Capacity = meetingRoom.Capacity,
                    HasWhiteboard = meetingRoom.HasWhiteboard,
                    HasWorkstations = meetingRoom.HasWorkstations,
                    HasMultimedia = meetingRoom.HasMultimedia,
                    OfficeId = meetingRoom.OfficeId,
                    Offices = database
                        .Offices
                        .OrderBy(o => o.Name)
                        .ToList()
                };

                return View(model);
            }
        }

        //POST: MeetingRoom/Edit
        [HttpPost]
        public ActionResult Edit(MeetingRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new MeetingRoomBookingSystemDbContext())
                {
                    var meetingRoom = database
                        .MeetingRooms
                        .Where(m => m.Id == model.Id)
                        .First();

                    meetingRoom.Name = model.Name;
                    meetingRoom.Capacity = model.Capacity;
                    meetingRoom.OfficeId = model.OfficeId;
                    meetingRoom.HasWhiteboard = model.HasWhiteboard;
                    meetingRoom.HasWorkstations = model.HasWorkstations;
                    meetingRoom.HasMultimedia = model.HasMultimedia;

                    database.Entry(meetingRoom).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Details", new { id = meetingRoom.Id });
                }
            }

            return View(model);
        }

        public ActionResult Backend()
        {
            return new Dpc().CallBack(this);
        }

        private class Dpc : DayPilotCalendar
        {
            private MeetingRoomBookingSystemDbContext database = new MeetingRoomBookingSystemDbContext();
            private int meetingRoomId = int.Parse(System
                .Web
                .HttpContext
                .Current
                .Request
                .UrlReferrer
                .ToString()
                .Split('/')
                .Last());

            protected override void OnInit(InitArgs e)
            {
                Update(CallBackUpdateType.Full);

                var db = new MeetingRoomBookingSystemDbContext();
            }

            protected override void OnEventResize(EventResizeArgs e)
            {
                var reservationId = int.Parse(e.Id);

                var toBeResized = database
                    .Reservations
                    .Where(r => r.Id == reservationId)
                    .First();

                var oldStart = toBeResized.StartDate;
                var oldEnd = toBeResized.EndDate;

                toBeResized.StartDate = e.NewStart;
                toBeResized.EndDate = e.NewEnd;

                var reservations = database
                        .Reservations
                        .Where(r => r.MeetingRoomId == meetingRoomId)
                        .Where(r => r.StartDate < toBeResized.EndDate && toBeResized.StartDate < r.EndDate)
                        .ToList();               

                if (reservations.Any())
                {
                    if ((reservations.Count == 1 && reservations.First().Id != toBeResized.Id) || reservations.Count > 1)
                    {
                        toBeResized.StartDate = oldStart;
                        toBeResized.EndDate = oldEnd;
                    }
                }
                else
                {                    
                    database.Entry(toBeResized).State = EntityState.Modified;
                    database.SaveChanges();
                }
                Update();
            }

            protected override void OnEventMove(EventMoveArgs e)
            {
                var reservationId = int.Parse(e.Id);

                var toBeResized = database
                    .Reservations
                    .Where(r => r.Id == reservationId)
                    .First();

                var oldStart = toBeResized.StartDate;
                var oldEnd = toBeResized.EndDate;

                toBeResized.StartDate = e.NewStart;
                toBeResized.EndDate = e.NewEnd;

                var reservations = database
                        .Reservations
                        .Where(r => r.MeetingRoomId == meetingRoomId)
                        .Where(r => r.StartDate < toBeResized.EndDate && toBeResized.StartDate < r.EndDate)
                        .ToList();

                if (reservations.Any())
                {
                    if ((reservations.Count == 1 && reservations.First().Id != toBeResized.Id) || reservations.Count > 1)
                    {
                        toBeResized.StartDate = oldStart;
                        toBeResized.EndDate = oldEnd;
                    }                    
                }
                else
                {                   
                    database.Entry(toBeResized).State = EntityState.Modified;
                    database.SaveChanges();
                }
                Update();
            }

            protected override void OnTimeRangeSelected(TimeRangeSelectedArgs e)
            {
                var toBeCreated = new Reservation("aabad246-7656-47b6-a60a-7fd18ddd2fe3"
                    , meetingRoomId
                    , e.Start
                    , e.End
                    , (string)e.Data["name"]);

                var reservations = database
                        .Reservations
                        .Where(r => r.MeetingRoomId == meetingRoomId)
                        .Where(r => r.StartDate < toBeCreated.EndDate && toBeCreated.StartDate < r.EndDate)
                        .ToList();

                if (reservations.Any())
                {

                }
                else
                {
                    database.Reservations.Add(toBeCreated);
                    database.SaveChanges();
                    reservations.Clear();
                }

                Update();
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                Events = database
                    .Reservations
                    .Where(r => r.MeetingRoomId == meetingRoomId)
                    .ToList();

                DataIdField = "Id";
                DataTextField = "Description";
                DataStartField = "StartDate";
                DataEndField = "EndDate";

                Update();
            }
        }
    }
}