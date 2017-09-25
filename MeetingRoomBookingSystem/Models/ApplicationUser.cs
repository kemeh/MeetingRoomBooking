using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace MeetingRoomBookingSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private string _timeZone;
        private TimeZoneInfo _timeZoneInstance;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }


        [ForeignKey("Office")]
        public int? OfficeId { get; set; }
        public virtual Office Office { get; set; }

        [DisplayName("Time Zone")]
        public string TimeZone
        {
            get { return _timeZone; }
            set
            {
                //TimeZoneInstance = null;
                _timeZone = value;
            }
        }      

        //[JsonIgnore]
        //public TimeZoneInfo TimeZoneInstance
        //{
        //    get
        //    {
        //        if (_timeZoneInstance == null)
        //        {
        //            try
        //            {
        //                _timeZoneInstance = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
        //            }
        //            catch
        //            {
        //                TimeZone = "Hawaiian Standard Time";
        //                _timeZoneInstance = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
        //            }
        //        }

        //        return _timeZoneInstance;
        //    }

        //    private set { _timeZoneInstance = value; }
        //}        

        ///// <summary>
        ///// Returns a UTC time in the user's specified timezone.
        ///// </summary>
        ///// <param name="utcTime">The utc time to convert</param>
        ///// <param name="timeZoneName">Name of the timezone (Eastern Standard Time)</param>
        ///// <returns>New local time</returns>
        //public DateTime GetUserTime(DateTime? utcTime = null)
        //{
        //    TimeZoneInfo tzi = null;

        //    if (utcTime == null)
        //        utcTime = DateTime.UtcNow;

        //    return TimeZoneInfo.ConvertTimeFromUtc(utcTime.Value, TimeZoneInstance);
        //}

        ///// <summary>
        ///// Converts local server time to the user's timezone and
        ///// returns the UTC date.
        ///// 
        ///// Use this to convert user captured date inputs and convert
        ///// them to UTC.  
        ///// 
        ///// User input (their local time) comes in as local server time 
        ///// -> convert to user's timezone from server time
        ///// -> convert to UTC
        ///// </summary>
        ///// <param name="localServerTime"></param>
        ///// <returns></returns>
        //public DateTime GetUtcUserTime(DateTime? localServerTime)
        //{
        //    if (localServerTime == null)
        //        localServerTime = DateTime.Now;

        //    return TimeZoneInfo.ConvertTime(localServerTime.Value, TimeZoneInstance).ToUniversalTime();
        //}


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}