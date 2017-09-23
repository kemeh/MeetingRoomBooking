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
            var dpc = new Dpc();

            return dpc.CallBack(this);
        }       
    }
}