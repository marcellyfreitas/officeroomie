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
    public class MedicalExamServiceTest
    {
        private readonly Mock<IMongoCollection<MedicalExam>> _medicalExamsCollectionMock;
        private readonly Mock<IApplicationDbContext> _contextMock;

        private readonly Mock<ILogger<MedicalExamService>> _loggerMock;
        private readonly MedicalExamService _service;

        public MedicalExamServiceTest()
        {
            _medicalExamsCollectionMock = new Mock<IMongoCollection<MedicalExam>>();
            _contextMock = new Mock<IApplicationDbContext>();
            _contextMock.Setup(c => c.MedicalExams).Returns(_medicalExamsCollectionMock.Object);
            _loggerMock = new Mock<ILogger<MedicalExamService>>();
            _service = new MedicalExamService(_contextMock.Object, _loggerMock.Object);
        }

        // ----------------- GetAllAsync -----------------
        [Fact]
        public async Task GetAllAsync_DeveRetornarMedicalExams()
        {
            var exams = new List<MedicalExam>
            {
                new MedicalExam { Id = "1", Name = "Hemograma" },
                new MedicalExam { Id = "2", Name = "ECG" }
            };

            var cursor = new Mock<IAsyncCursor<MedicalExam>>();
            cursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
            cursor.SetupGet(c => c.Current).Returns(exams);

            _medicalExamsCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<MedicalExam>>(),
                                       It.IsAny<FindOptions<MedicalExam, MedicalExam>>(),
                                       It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursor.Object);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, ((List<MedicalExam>)result).Count);
            Assert.Equal("Hemograma", ((List<MedicalExam>)result)[0].Name);
        }

        // ----------------- GetByIdAsync -----------------
        [Fact]
        public async Task GetByIdAsync_DeveRetornarMedicalExam()
        {
            var exam = new MedicalExam { Id = "1", Name = "Hemograma" };

            var cursor = new Mock<IAsyncCursor<MedicalExam>>();
            cursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
            cursor.SetupGet(c => c.Current).Returns(new List<MedicalExam> { exam });

            _medicalExamsCollectionMock
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<MedicalExam>>(),
                                       It.IsAny<FindOptions<MedicalExam, MedicalExam>>(),
                                       It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursor.Object);

            var result = await _service.GetByIdAsync("1");

            Assert.NotNull(result);
            Assert.Equal("Hemograma", result.Name);
        }

        // ----------------- AddAsync -----------------
        [Fact]
        public async Task AddAsync_DeveInserirMedicalExam()
        {
            var exam = new MedicalExam { Id = "1", Name = "Hemograma" };

            _medicalExamsCollectionMock
                .Setup(c => c.InsertOneAsync(exam, null, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var result = await _service.AddAsync(exam);

            Assert.Equal(exam, result);
            _medicalExamsCollectionMock.Verify();
        }

        // ----------------- UpdateAsync -----------------
        [Fact]
        public async Task UpdateAsync_DeveChamarReplaceOneAsync()
        {
            var exam = new MedicalExam { Id = "1", Name = "Hemograma" };

            _medicalExamsCollectionMock
                .Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<MedicalExam>>(),
                                              exam,
                                              It.IsAny<ReplaceOptions>(),
                                              It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReplaceOneResult.Acknowledged(1, 1, ObjectId.GenerateNewId()))
                .Verifiable();

            await _service.UpdateAsync(exam);

            _medicalExamsCollectionMock.Verify();
        }

        // ----------------- DeleteAsync -----------------
        [Fact]
        public async Task DeleteAsync_DeveChamarDeleteOneAsync()
        {
            var exam = new MedicalExam { Id = "1", Name = "Hemograma" };

            _medicalExamsCollectionMock
                .Setup(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<MedicalExam>>(),
                                             It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeleteResult.Acknowledged(1))
                .Verifiable();

            await _service.DeleteAsync(exam);

            _medicalExamsCollectionMock.Verify();
        }
    }
}
