using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MeetingRoomBookingSystem.Models
{
    public class Reservation
    {
        public Reservation()
        {
            
        }

        public Reservation(string userId, int meetingRoomId, DateTime start, DateTime end)
        {
            this.UserId = userId;
            this.MeetingRoomId = meetingRoomId;
            this.StartDate = start;
            this.EndDate = end;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd-hh-mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

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
    }    
}