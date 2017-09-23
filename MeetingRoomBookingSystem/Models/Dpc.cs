using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MeetingRoomBookingSystem.Models
{
    public class Dpc : DayPilotCalendar
    {
        private MeetingRoomBookingSystemDbContext database;
        private string userId;
        private int meetingRoomId;

        public Dpc()
        {
            this.database = new MeetingRoomBookingSystemDbContext();
            this.meetingRoomId = int.Parse(System
            .Web
            .HttpContext
            .Current
            .Request
            .UrlReferrer
            .ToString()
            .Split('/')
            .Last());
            this.userId = HttpContext.Current.User.Identity.GetUserId();

        }

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
            var toBeCreated = new Reservation(this.userId
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