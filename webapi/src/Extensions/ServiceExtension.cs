using WebApi.Models;
using WebApi.Services;

namespace WebApi.Extensions;

static class ServiceExtension
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService<User>, UserService>();
        services.AddTransient<IUserService<Administrator>, AdministratorService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IBaseService<Specialization>, SpecializationService>();
        services.AddTransient<IBaseService<MedicalExam>, MedicalExamService>();
        services.AddTransient<IBaseService<Appointment>, AppointmentService>();
        services.AddTransient<IMedicalService, MedicalService>();
        services.AddTransient<IBaseService<AppointmentRating>, AppointmentRatingService>();
        services.AddTransient<IBaseService<Schedule>, ScheduleService>();
        services.AddTransient<IBaseService<Convenio>, ConvenioService>();

        return services;
    }
}