using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingRoomBookingSystem.Models
{
    public class MeetingRoom
    {
        public MeetingRoom()
        {

        }

        public MeetingRoom(string name, int capacity, bool hasWorkstations, bool hasMultimedia, bool hasWhiteboard, int officeId)
        {
            this.Name = name;
            this.Capacity = capacity;
            this.HasWhiteboard = hasWhiteboard;
            this.HasMultimedia = hasMultimedia;
            this.HasWorkstations = hasWorkstations;
            this.OfficeId = officeId;
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Capacity { get; set; }

        public bool HasMultimedia { get; set; }

        public bool HasWorkstations { get; set; }

        public bool HasWhiteboard { get; set; }

        [ForeignKey("Office")]
        public int OfficeId { get; set; }

        public virtual Office Office { get; set; }
    }
}