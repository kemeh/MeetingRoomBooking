﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeetingRoomBookingSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Office");
        }
        
    }
}