using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingRoomBookingSystem.Models
{
    public class MeetingRoomViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public bool HasMultimedia { get; set; }

        public bool HasWorkstations { get; set; }

        public bool HasWhiteboard { get; set; }

        public ICollection<Office> Offices { get; set; }

        public int OfficeId { get; set; }
    }
}