using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MeetingRoomBookingSystem.Models
{
    public class MeetingRoomBookingSystemDbContext : IdentityDbContext<ApplicationUser>
    {
        public MeetingRoomBookingSystemDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            
        }

        public virtual IDbSet<Office> Offices { get; set; }
        public virtual IDbSet<MeetingRoom> MeetingRooms { get; set; }
        public virtual IDbSet<Reservation> Reservations { get; set; }

        public static MeetingRoomBookingSystemDbContext Create()
        {
            return new MeetingRoomBookingSystemDbContext();
        }
    }
}