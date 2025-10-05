using MongoDB.Driver;
using WebApi.Database;
using WebApi.Models;

namespace WebApi.Services;

public class AppointmentRatingService : IBaseService<AppointmentRating>
{

    private readonly IApplicationDbContext _context;
    private readonly ILogger<AppointmentRatingService> _logger;

    public AppointmentRatingService(IApplicationDbContext context, ILogger<AppointmentRatingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<AppointmentRating>> GetAllAsync()
    {
        try
        {
            var list = await _context.AppointmentRatings
                    .Find(_ => true)
                    .SortByDescending(u => u.CreatedAt)
                    .ToListAsync();

            if (list.Count > 0)
            {
                var appointmentsId = list.Select(d => d.AppointmentId).Distinct().ToList();
                var usersId = list.Select(d => d.UserId).Distinct().ToList();

                var appointment = await _context.Appointments
                    .Find(s => appointmentsId.Contains(s.Id))
                    .ToListAsync();

                var users = await _context.Users
                    .Find(s => usersId.Contains(s.Id))
                    .ToListAsync();

                foreach (var doctor in list)
                {
                    doctor.Appointment = appointment.FirstOrDefault(s => s.Id == doctor.AppointmentId);
                    doctor.User = users.FirstOrDefault(s => s.Id == doctor.UserId);
                }
            }

            return list;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erro ao buscar todas models");
            throw;
        }
    }

    public async Task<AppointmentRating?> GetByIdAsync(string id)
    {
        try
        {
            var filter = Builders<AppointmentRating>.Filter.Eq(u => u.Id, id);
            var model = await _context.AppointmentRatings.Find(filter).FirstOrDefaultAsync();

            if (model != null && !string.IsNullOrEmpty(model.AppointmentId))
            {
                model.Appointment = await _context.Appointments
                    .Find(s => s.Id == model.AppointmentId)
                    .FirstOrDefaultAsync();
            }

            if (model != null && !string.IsNullOrEmpty(model.UserId))
            {
                model.User = await _context.Users
                    .Find(s => s.Id == model.UserId)
                    .FirstOrDefaultAsync();
            }

            return model;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error ao buscar model");
            throw;
        }
    }

    public async Task<AppointmentRating?> AddAsync(AppointmentRating data)
    {
        try
        {
            await _context.AppointmentRatings.InsertOneAsync(data);
            return data;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erro ao adicionar model");
            throw;
        }
    }

    public async Task UpdateAsync(AppointmentRating data)
    {
        try
        {
            var filter = Builders<AppointmentRating>.Filter.Eq(_ => _.Id, data.Id);
            await _context.AppointmentRatings.ReplaceOneAsync(filter, data);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Erro ao atualizar model");
            throw;
        }
    }

    public async Task DeleteAsync(AppointmentRating data)
    {
        try
        {
            var filter = Builders<AppointmentRating>.Filter.Eq(_ => _.Id, data.Id);
            await _context.AppointmentRatings.DeleteOneAsync(filter);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"Erro ao deletar model: {data.Id}");
            throw;
        }
    }

}