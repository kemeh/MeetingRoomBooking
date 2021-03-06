﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace MeetingRoomBookingSystem.Models
{
    public class Office
    {
        private string _timeZone;

        public Office()
        {
            
        }

        public Office(string name, string address, string phoneNumber, string timeZone)
        {
            this.Name = name;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.IsActive = true;
            this.TimeZone = timeZone;
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
        public string TimeZone
        {
            get { return _timeZone; }
            set
            {
                _timeZone = value;
            }
        }

        public virtual ICollection<MeetingRoom> MeetingRooms { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
}