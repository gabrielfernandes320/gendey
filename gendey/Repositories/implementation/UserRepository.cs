using gendey.Models;
using gendey.Repositories.contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gendey.Repositories.implementation
{
    public class UserRepository : IGendeyRepository<User>
    {
        readonly gendeyContext _gendeyContext; 
        readonly IAuthRepository<User> _authRepository;

        public UserRepository(gendeyContext context, IAuthRepository<User> authRepository)
        {
            _gendeyContext = context;
            _authRepository = authRepository;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _gendeyContext.User.ToListAsync();
        }

        public async Task<User> Get(int id)
        {
            var user = await _gendeyContext.User.FindAsync(id);
            return user;
        }

        public async Task<User> Update(int id, object obj)
        {
            _gendeyContext.Entry(obj).State = EntityState.Modified;

            try
            {
                await _gendeyContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return await Get(id);

        }

        public bool Exists(int id)
        {
            return _gendeyContext.User.Any(e => e.Id == id);
        }

        public async Task<User> Add(object obj)
        {
            var user = (User) obj;
            user.RegisterDate = DateTime.Now;
            user.Password = _authRepository.GetEncryptedPassword(user.Password);

            _gendeyContext.User.Add(user);

            try
            {
                await _gendeyContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return (User)obj;
        }

        public async Task<User> Delete(object obj)
        {
            _gendeyContext.User.Remove((User)obj);

            try
            {
                await _gendeyContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return (User)obj;
        }

        public async Task<List<DateTime?>> GetDayAvaiableAppointments(int id, DateTime day, int dayOfWeek)
        {
            var user = await Get(id);
            var confSchedule = await _gendeyContext.ScheduleConfig.Where(x => x.UserId == id && x.DayOfWeek == dayOfWeek).FirstOrDefaultAsync();
            var availableAppointmentsList = new List<DateTime?>();
            IEnumerable<Schedule> occupedAppointments = _gendeyContext.Schedule.Where(x => x.AppointmentDate == day && x.AttendantId == id);

            DateTime dt = new DateTime(day.Year, day.Day, day.Month);
            DateTime StartTime = (DateTime)(dt + confSchedule.StartTime);
            DateTime EndTime = (DateTime)(dt + confSchedule.EndTime);
            while (StartTime != EndTime)
            {
                double minuts = (double)+confSchedule.Duration;
                StartTime = StartTime.AddMinutes(minuts);
                availableAppointmentsList.Add(StartTime);
            }

            foreach (var item in occupedAppointments)
            {
                availableAppointmentsList.RemoveAll(x => x == dt + item.StartTime);
            }

            return availableAppointmentsList;
        }
    }
}
