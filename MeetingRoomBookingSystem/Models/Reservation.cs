using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Web.Mvc;

namespace MeetingRoomBookingSystem.Models
{
    public class Reservation
    {
        public Reservation()
        {
            
        }

        public Reservation(string userId, int meetingRoomId, DateTime start, DateTime end, string description)
        {
            this.UserId = userId;
            this.MeetingRoomId = meetingRoomId;
            this.StartDate = start;
            this.EndDate = end;
            this.Description = description;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        [ForeignKey("MeetingRoom")]
        public int MeetingRoomId { get; set; }

        public virtual MeetingRoom MeetingRoom { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public bool IsAuthor(string name)
        {
            return this.User.UserName.Equals(name);
        }

        public JavaScriptResult Invalid()
        {
            return new JavaScriptResult { Script = "alert('Successfully registered');" };
        }
    }    
}