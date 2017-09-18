using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeetingRoomBookingSystem.Models
{
    public class ReservationViewModel
    {
        [Key]
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int MeetingRoomId { get; set; }

        public ICollection<MeetingRoom> MeetingRooms { get; set; }

        public virtual MeetingRoom MeetingRoom { get; set; }
    }
}