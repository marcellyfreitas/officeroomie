using Xunit;
using Moq;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Services;
using WebApi.Database;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace webapi.testes
{
    public class AppointmentServiceTest
    {
        private readonly Mock<IMongoCollection<Appointment>> _appointmentsCollectionMock;
        private readonly Mock<IMongoCollection<User>> _usersCollectionMock;
        private readonly Mock<IMongoCollection<Schedule>> _schedulesCollectionMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly Mock<ILogger<AppointmentService>> _loggerMock;
        private readonly AppointmentService _service;

        public AppointmentServiceTest()
        {
            _appointmentsCollectionMock = new Mock<IMongoCollection<Appointment>>();
            _usersCollectionMock = new Mock<IMongoCollection<User>>();
            _schedulesCollectionMock = new Mock<IMongoCollection<Schedule>>();

            _contextMock = new Mock<IApplicationDbContext>();
            _contextMock.Setup(c => c.Appointments).Returns(_appointmentsCollectionMock.Object);
            _contextMock.Setup(c => c.Users).Returns(_usersCollectionMock.Object);
            _contextMock.Setup(c => c.Schedules).Returns(_schedulesCollectionMock.Object);

            _loggerMock = new Mock<ILogger<AppointmentService>>();
            _service = new AppointmentService(_contextMock.Object, _loggerMock.Object);
        }

        // ----------------- GetAllAsync -----------------
        [Fact]
        public async Task GetAllAsync_DeveRetornarAppointmentsComRelacionamentos()
        {
            var appointments = new List<Appointment>
            {
                new Appointment { Id = "1", UserId = "U1", ScheduleId = "S1" },
                new Appointment { Id = "2", UserId = "U2", ScheduleId = "S2" }
            };

            var users = new List<User>
            {
                new User { Id = "U1", Name = "João" },
                new User { Id = "U2", Name = "Maria" }
            };

            var schedules = new List<Schedule>
            {
                new Schedule { Id = "S1" },
                new Schedule { Id = "S2" }
            };

            // Cursor de Appointments
            var cursorAppointments = new Mock<IAsyncCursor<Appointment>>();
            cursorAppointments.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(true)
                              .ReturnsAsync(false);
            cursorAppointments.SetupGet(c => c.Current).Returns(appointments);

            _appointmentsCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Appointment>>(),
                                        It.IsAny<FindOptions<Appointment, Appointment>>(),
                                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorAppointments.Object);

            // Cursor de Users
            var cursorUsers = new Mock<IAsyncCursor<User>>();
            cursorUsers.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(true)
                       .ReturnsAsync(false);
            cursorUsers.SetupGet(c => c.Current).Returns(users);

            _usersCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                        It.IsAny<FindOptions<User, User>>(),
                                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorUsers.Object);

            // Cursor de Schedules
            var cursorSchedules = new Mock<IAsyncCursor<Schedule>>();
            cursorSchedules.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(true)
                           .ReturnsAsync(false);
            cursorSchedules.SetupGet(c => c.Current).Returns(schedules);

            _schedulesCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Schedule>>(),
                                        It.IsAny<FindOptions<Schedule, Schedule>>(),
                                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorSchedules.Object);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, ((List<Appointment>)result).Count);
            Assert.Equal("João", ((List<Appointment>)result)[0].User!.Name);
            Assert.NotNull(((List<Appointment>)result)[0].Schedule);
        }

        // ----------------- GetByIdAsync -----------------
        [Fact]
        public async Task GetByIdAsync_DeveRetornarAppointmentComRelacionamentos()
        {
            var appointment = new Appointment { Id = "1", UserId = "U1", ScheduleId = "S1" };
            var user = new User { Id = "U1", Name = "João" };
            var schedule = new Schedule { Id = "S1" };

            // Cursor Appointment
            var cursorAppointment = new Mock<IAsyncCursor<Appointment>>();
            cursorAppointment.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                             .ReturnsAsync(true)
                             .ReturnsAsync(false);
            cursorAppointment.SetupGet(c => c.Current).Returns(new List<Appointment> { appointment });

            _appointmentsCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Appointment>>(),
                                        It.IsAny<FindOptions<Appointment, Appointment>>(),
                                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorAppointment.Object);

            // Cursor User
            var cursorUser = new Mock<IAsyncCursor<User>>();
            cursorUser.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true)
                      .ReturnsAsync(false);
            cursorUser.SetupGet(c => c.Current).Returns(new List<User> { user });

            _usersCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                        It.IsAny<FindOptions<User, User>>(),
                                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorUser.Object);

            // Cursor Schedule
            var cursorSchedule = new Mock<IAsyncCursor<Schedule>>();
            cursorSchedule.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true)
                          .ReturnsAsync(false);
            cursorSchedule.SetupGet(c => c.Current).Returns(new List<Schedule> { schedule });

            _schedulesCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Schedule>>(),
                                        It.IsAny<FindOptions<Schedule, Schedule>>(),
                                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorSchedule.Object);

            var result = await _service.GetByIdAsync("1");

            Assert.NotNull(result);
            Assert.Equal("João", result!.User!.Name);
            Assert.NotNull(result.Schedule);
        }

        // ----------------- AddAsync -----------------
        [Fact]
        public async Task AddAsync_DeveInserirAppointment()
        {
            var appointment = new Appointment { Id = "1" };

            _appointmentsCollectionMock
                .Setup(c => c.InsertOneAsync(appointment, null, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _service.AddAsync(appointment);

            Assert.Equal(appointment, result);
            _appointmentsCollectionMock.Verify();
        }

        // ----------------- UpdateAsync -----------------
        [Fact]
        public async Task UpdateAsync_DeveChamarReplaceOneAsync()
        {
            var appointment = new Appointment { Id = "1" };

            _appointmentsCollectionMock
                .Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<Appointment>>(),
                                              appointment,
                                              It.IsAny<ReplaceOptions>(),
                                              It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReplaceOneResult.Acknowledged(1, 1, ObjectId.GenerateNewId()))
                .Verifiable();

            await _service.UpdateAsync(appointment);

            _appointmentsCollectionMock.Verify();
        }

        // ----------------- DeleteAsync -----------------
        [Fact]
        public async Task DeleteAsync_DeveChamarDeleteOneAsync()
        {
            var appointment = new Appointment { Id = "1" };

            _appointmentsCollectionMock
                .Setup(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<Appointment>>(),
                                             It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeleteResult.Acknowledged(1))
                .Verifiable();

            await _service.DeleteAsync(appointment);

            _appointmentsCollectionMock.Verify();
        }
    }
}
