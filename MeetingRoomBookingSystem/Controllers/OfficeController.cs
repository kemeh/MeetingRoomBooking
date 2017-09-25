using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MeetingRoomBookingSystem.Models;
using Microsoft.AspNet.Identity;

namespace MeetingRoomBookingSystem.Controllers
{
    [Authorize]
    public class OfficeController : Controller
    {
        // GET: Office
        [Authorize]
        public ActionResult Index()
        {
            using (var database = new MeetingRoomBookingSystemDbContext())
            {
                var user = database
                    .Users
                    .First(u => u.UserName == this.User.Identity.Name);


                if (this.User.IsInRole("Admin"))
                {
                    return RedirectToAction("List");
                }

                var userOfficeId = user.Office.Id;

                return RedirectToAction("Details", new {id = userOfficeId});

            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            using (var database = new MeetingRoomBookingSystemDbContext())
            {
                var offices = database
                    .Offices
                    .ToList();

                return View(offices);
            }
        }

        //GET: Office/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new MeetingRoomBookingSystemDbContext())
            {
                var office = database
                    .Offices
                    .Where(o => o.Id == id)
                    .Include(o => o.MeetingRooms)
                    .First();

                if (office == null)
                {
                    return HttpNotFound();
                }

                return View(office);
            }
        }

        //GET: Office/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var model = new OfficeCreateModel();

            return View(model);
        }

        //POST: Office/Create
        [HttpPost]
        public ActionResult Create(Office model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new MeetingRoomBookingSystemDbContext())
                {
                    model.IsActive = true;

                    var office = new Office
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Address = model.Address,
                        IsActive = true,
                        PhoneNumber = model.PhoneNumber,
                        TimeZone = model.TimeZone
                    };

                    database.Offices.Add(office);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);

        }

        //GET: Office/Edit
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new MeetingRoomBookingSystemDbContext())
            {
                var office = database
                    .Offices
                    .Where(o => o.Id == id)
                    .Include(o => o.MeetingRooms)
                    .First();

                if (office == null)
                {
                    return HttpNotFound();
                }

                var model = new Office();
                model.Id = office.Id;
                model.Name = office.Name;
                model.Address = office.Address;
                model.PhoneNumber = office.PhoneNumber;
                model.MeetingRooms = office.MeetingRooms;
                model.IsActive = office.IsActive;

                return View(model);
            }
        }

        //POST: Office/Edit
        [HttpPost]
        public ActionResult Edit(Office model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new MeetingRoomBookingSystemDbContext())
                {
                    var office = database
                        .Offices
                        .FirstOrDefault(o => o.Id == model.Id);

                    office.Name = model.Name;
                    office.Address = model.Address;
                    office.PhoneNumber = model.PhoneNumber;
                    office.MeetingRooms = model.MeetingRooms;
                    office.IsActive = model.IsActive;

                    database.Entry(office).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }
    }
}