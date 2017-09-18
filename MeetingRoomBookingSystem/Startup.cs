using System.Data.Entity;
using MeetingRoomBookingSystem.Migrations;
using MeetingRoomBookingSystem.Models;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MeetingRoomBookingSystem.Startup))]
namespace MeetingRoomBookingSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<MeetingRoomBookingSystemDbContext, Configuration>());

            ConfigureAuth(app);
        }
    }
}
