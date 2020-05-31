using System;
using System.Collections.Generic;

namespace gendey.Models
{
    public partial class User
    {
        public User()
        {
            AttendantServiceRelAttendant = new HashSet<AttendantServiceRel>();
            AttendantServiceRelService = new HashSet<AttendantServiceRel>();
            ScheduleAttendant = new HashSet<Schedule>();
            ScheduleClient = new HashSet<Schedule>();
            ScheduleConfig = new HashSet<ScheduleConfig>();
            Session = new HashSet<Session>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public bool Active { get; set; }
        public int FkContact { get; set; }
        public int FkAdress { get; set; }
        public int? FkConfClient { get; set; }
        public int? FkConfAttendant { get; set; }
        public DateTime RegisterDate { get; set; }

        public virtual Adress FkAdressNavigation { get; set; }
        public virtual ConfAttendant FkConfAttendantNavigation { get; set; }
        public virtual ConfClient FkConfClientNavigation { get; set; }
        public virtual Contact FkContactNavigation { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<AttendantServiceRel> AttendantServiceRelAttendant { get; set; }
        public virtual ICollection<AttendantServiceRel> AttendantServiceRelService { get; set; }
        public virtual ICollection<Schedule> ScheduleAttendant { get; set; }
        public virtual ICollection<Schedule> ScheduleClient { get; set; }
        public virtual ICollection<ScheduleConfig> ScheduleConfig { get; set; }
        public virtual ICollection<Session> Session { get; set; }
    }
}
