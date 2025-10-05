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
    public class ScheduleServiceTest
    {
        private readonly Mock<IMongoCollection<Schedule>> _schedulesCollectionMock;
        private readonly Mock<IMongoCollection<Medical>> _medicalsCollectionMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly Mock<ILogger<ScheduleService>> _loggerMock;
        private readonly ScheduleService _service;

        public ScheduleServiceTest()
        {
            _schedulesCollectionMock = new Mock<IMongoCollection<Schedule>>();
            _medicalsCollectionMock = new Mock<IMongoCollection<Medical>>();

            _contextMock = new Mock<IApplicationDbContext>();
            _contextMock.Setup(c => c.Schedules).Returns(_schedulesCollectionMock.Object);
            _contextMock.Setup(c => c.Medicals).Returns(_medicalsCollectionMock.Object);

            _loggerMock = new Mock<ILogger<ScheduleService>>();
            _service = new ScheduleService(_contextMock.Object, _loggerMock.Object);
        }

        // ----------------- GetAllAsync -----------------
        [Fact]
        public async Task GetAllAsync_DeveRetornarSchedulesComRelacionamento()
        {
            var schedules = new List<Schedule>
            {
                new Schedule { Id = "1", MedicalId = "10" },
                new Schedule { Id = "2", MedicalId = "20" }
            };

            var medicals = new List<Medical>
            {
                new Medical { Id = "10", Name = "Dr. João" },
                new Medical { Id = "20", Name = "Dra. Maria" }
            };

            var cursorSchedules = new Mock<IAsyncCursor<Schedule>>();
            cursorSchedules.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(true)
                           .ReturnsAsync(false);
            cursorSchedules.SetupGet(c => c.Current).Returns(schedules);

            var cursorMedicals = new Mock<IAsyncCursor<Medical>>();
            cursorMedicals.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(true)
                           .ReturnsAsync(false);
            cursorMedicals.SetupGet(c => c.Current).Returns(medicals);

            _schedulesCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Schedule>>(),
                                         It.IsAny<FindOptions<Schedule, Schedule>>(),
                                         It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorSchedules.Object);

            _medicalsCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Medical>>(),
                                         It.IsAny<FindOptions<Medical, Medical>>(),
                                         It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorMedicals.Object);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, ((List<Schedule>)result).Count);
            Assert.Equal("Dr. João", ((List<Schedule>)result)[0].Medical!.Name);
        }

        // ----------------- GetByIdAsync -----------------
        [Fact]
        public async Task GetByIdAsync_DeveRetornarScheduleComMedical()
        {
            var schedule = new Schedule { Id = "1", MedicalId = "10" };
            var medical = new Medical { Id = "10", Name = "Dr. João" };

            var cursorSchedule = new Mock<IAsyncCursor<Schedule>>();
            cursorSchedule.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true)
                          .ReturnsAsync(false);
            cursorSchedule.SetupGet(c => c.Current).Returns(new List<Schedule> { schedule });

            var cursorMedical = new Mock<IAsyncCursor<Medical>>();
            cursorMedical.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                          .ReturnsAsync(true)
                          .ReturnsAsync(false);
            cursorMedical.SetupGet(c => c.Current).Returns(new List<Medical> { medical });

            _schedulesCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Schedule>>(),
                                         It.IsAny<FindOptions<Schedule, Schedule>>(),
                                         It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorSchedule.Object);

            _medicalsCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Medical>>(),
                                         It.IsAny<FindOptions<Medical, Medical>>(),
                                         It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorMedical.Object);

            var result = await _service.GetByIdAsync("1");

            Assert.NotNull(result);
            Assert.Equal("Dr. João", result!.Medical!.Name);
        }

        // ----------------- AddAsync -----------------
        [Fact]
        public async Task AddAsync_DeveInserirSchedule()
        {
            var schedule = new Schedule { Id = "1" };

            _schedulesCollectionMock
                .Setup(c => c.InsertOneAsync(schedule, null, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _service.AddAsync(schedule);

            Assert.Equal(schedule, result);
            _schedulesCollectionMock.Verify();
        }

        // ----------------- UpdateAsync -----------------
        [Fact]
        public async Task UpdateAsync_DeveChamarReplaceOneAsync()
        {
            var schedule = new Schedule { Id = "1" };

            _schedulesCollectionMock
                .Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<Schedule>>(),
                                              schedule,
                                              It.IsAny<ReplaceOptions>(),
                                              It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReplaceOneResult.Acknowledged(1, 1, ObjectId.GenerateNewId()))
                .Verifiable();

            await _service.UpdateAsync(schedule);

            _schedulesCollectionMock.Verify();
        }

        // ----------------- DeleteAsync -----------------
        [Fact]
        public async Task DeleteAsync_DeveChamarDeleteOneAsync()
        {
            var schedule = new Schedule { Id = "1" };

            _schedulesCollectionMock
                .Setup(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<Schedule>>(),
                                             It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeleteResult.Acknowledged(1))
                .Verifiable();

            await _service.DeleteAsync(schedule);

            _schedulesCollectionMock.Verify();
        }
    }
}
