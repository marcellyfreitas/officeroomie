using MongoDB.Driver;
using WebApi.Database;
using WebApi.Models;

namespace WebApi.Services
{
    public class AppointmentService : IBaseService<Appointment>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(IApplicationDbContext context, ILogger<AppointmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            try
            {
                var list = await _context.Appointments
                    .Find(_ => true)
                    .SortByDescending(u => u.CreatedAt)
                    .ToListAsync();

                if (list.Count > 0)
                {
                    var userIds = list.Select(d => d.UserId).Distinct().ToList();
                    var scheduleIds = list.Select(d => d.ScheduleId).Distinct().ToList();

                    var user = await _context.Users
                    .Find(s => userIds.Contains(s.Id))
                    .ToListAsync();

                    var schedule = await _context.Schedules
                    .Find(s => scheduleIds.Contains(s.Id))
                    .ToListAsync();

                    foreach (var model in list)
                    {
                        model.User = user.FirstOrDefault(s => s.Id == model.UserId);
                        model.Schedule = schedule.FirstOrDefault(s => s.Id == model.ScheduleId);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os models");
                throw;
            }
        }

        public async Task<Appointment?> GetByIdAsync(string id)
        {
            try
            {
                var filter = Builders<Appointment>.Filter.Eq(u => u.Id, id);
                var model = await _context.Appointments.Find(filter).FirstOrDefaultAsync();

                if (model == null)
                {
                    return null;
                }

                model.User = await _context.Users
                    .Find(s => s.Id == model.UserId)
                    .FirstOrDefaultAsync();

                model.Schedule = await _context.Schedules
                    .Find(s => s.Id == model.ScheduleId)
                    .FirstOrDefaultAsync();

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar model por Id: {id}");
                throw;
            }
        }

        public async Task<Appointment?> AddAsync(Appointment model)
        {
            try
            {
                await _context.Appointments.InsertOneAsync(model);
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir model");
                throw;
            }
        }

        public async Task UpdateAsync(Appointment model)
        {
            try
            {
                var filter = Builders<Appointment>.Filter.Eq(u => u.Id, model.Id);
                await _context.Appointments.ReplaceOneAsync(filter, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar model: {model.Id}");
                throw;
            }
        }

        public async Task DeleteAsync(Appointment model)
        {
            try
            {
                var filter = Builders<Appointment>.Filter.Eq(u => u.Id, model.Id);
                await _context.Appointments.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao deletar model: {model.Id}");
                throw;
            }
        }
    }
}
