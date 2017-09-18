﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetingRoomBookingSystem.Models
{
    public class Office
    {
        public Office()
        {
            
        }

        public Office(string name, string address, string phoneNumber)
        {
            this.Name = name;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.IsActive = true;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<MeetingRoom> MeetingRooms { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }

    }
}