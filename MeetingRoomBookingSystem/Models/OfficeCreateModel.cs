using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeetingRoomBookingSystem.Models
{
    public class OfficeCreateModel
    {
        public OfficeCreateModel()
        {
            var tzs = TimeZoneInfo.GetSystemTimeZones();
            TimezoneList = tzs.Select(tz => new SelectListItem()
            {
                Text = tz.DisplayName,
                Value = tz.Id
            }).ToArray();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [DisplayName("Time Zone")]
        public string TimeZone { get; set; }
        public SelectListItem[] TimezoneList { get; set; }
    }
}