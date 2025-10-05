using Xunit;
using Moq;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Services;
using WebApi.Database;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace webapi.testes;

public class AdministratorServiceTest
{
    private readonly Mock<IMongoCollection<Administrator>> _adminsCollectionMock;
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ILogger<AdministratorService>> _loggerMock;
    private readonly AdministratorService _service;

    public AdministratorServiceTest()
    {
        _adminsCollectionMock = new Mock<IMongoCollection<Administrator>>();
        _contextMock = new Mock<IApplicationDbContext>();
        _contextMock.Setup(c => c.Administrators).Returns(_adminsCollectionMock.Object);
        _loggerMock = new Mock<ILogger<AdministratorService>>();
        _service = new AdministratorService(_contextMock.Object, _loggerMock.Object);
    }

    // ----------------- GetAllAsync -----------------
    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosAdministradores()
    {
        var expectedAdmins = new List<Administrator>
        {
            new Administrator { Id = "1", Name = "Admin1" },
            new Administrator { Id = "2", Name = "Admin2" }
        };

        var cursorMock = new Mock<IAsyncCursor<Administrator>>();
        cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(c => c.Current).Returns(expectedAdmins);

        _adminsCollectionMock
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Administrator>>(),
                                     It.IsAny<FindOptions<Administrator, Administrator>>(),
                                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, ((List<Administrator>)result).Count);
        Assert.Equal("Admin1", ((List<Administrator>)result)[0].Name);
    }

    // ----------------- AddAsync -----------------
    [Fact]
    public async Task AddAsync_DeveInserirAdministrador()
    {
        var admin = new Administrator { Id = "1", Name = "Admin1" };

        _adminsCollectionMock
            .Setup(c => c.InsertOneAsync(admin, null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var result = await _service.AddAsync(admin);

        Assert.Equal(admin, result);
        _adminsCollectionMock.Verify();
    }

    // ----------------- UpdateAsync -----------------
    [Fact]
    public async Task UpdateAsync_DeveChamarReplaceOneAsync()
    {
        var admin = new Administrator { Id = "1", Name = "NovoAdmin" };

        _adminsCollectionMock
            .Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<Administrator>>(), admin, It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReplaceOneResult.Acknowledged(1, 1, ObjectId.GenerateNewId()))
            .Verifiable();

        await _service.UpdateAsync(admin);

        _adminsCollectionMock.Verify();
    }

    // ----------------- GetByIdAsync -----------------
    [Fact]
    public async Task GetByIdAsync_DeveRetornarAdministrador()
    {
        var admin = new Administrator { Id = "1", Name = "Admin1" };

        var cursorMock = new Mock<IAsyncCursor<Administrator>>();
        cursorMock.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(_ => _.Current).Returns(new List<Administrator> { admin });

        _adminsCollectionMock
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Administrator>>(),
                                     It.IsAny<FindOptions<Administrator, Administrator>>(),
                                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _service.GetByIdAsync("1");

        Assert.NotNull(result);
        Assert.Equal("Admin1", result!.Name);
    }

    // ----------------- DeleteAsync -----------------
    [Fact]
    public async Task DeleteAsync_DeveChamarDeleteOneAsync()
    {
        var admin = new Administrator { Id = "1" };

        _adminsCollectionMock
            .Setup(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<Administrator>>(),
                                         It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteResult.Acknowledged(1))
            .Verifiable();

        await _service.DeleteAsync(admin);

        _adminsCollectionMock.Verify();
    }

    // ----------------- GetByEmailAsync -----------------
    [Fact]
    public async Task GetByEmailAsync_DeveRetornarAdministrador()
    {
        var admin = new Administrator { Id = "1", Email = "admin@test.com" };

        var cursorMock = new Mock<IAsyncCursor<Administrator>>();
        cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(c => c.Current).Returns(new List<Administrator> { admin });

        _adminsCollectionMock
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Administrator>>(),
                                     It.IsAny<FindOptions<Administrator, Administrator>>(),
                                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _service.GetByEmailAsync("admin@test.com");

        Assert.NotNull(result);
        Assert.Equal("admin@test.com", result!.Email);
    }
}
