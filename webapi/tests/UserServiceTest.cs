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

public class UserServiceTest
{
    private readonly Mock<IMongoCollection<User>> _usersCollectionMock;
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly UserService _service;

    public UserServiceTest()
    {
        _usersCollectionMock = new Mock<IMongoCollection<User>>();
        _contextMock = new Mock<IApplicationDbContext>();
        _contextMock.Setup(c => c.Users).Returns(_usersCollectionMock.Object);
        _loggerMock = new Mock<ILogger<UserService>>();
        _service = new UserService(_contextMock.Object, _loggerMock.Object);
    }

    // ----------------- GetAllAsync -----------------
    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosUsuarios()
    {
        var expectedUsers = new List<User>
    {
        new User { Id = "1", Name = "Waldisclaison" },
        new User { Id = "2", Name = "Maria" }
    };

        var cursorMock = new Mock<IAsyncCursor<User>>();
        cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(c => c.Current).Returns(expectedUsers);

        _usersCollectionMock
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                     It.IsAny<FindOptions<User, User>>(),
                                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _service.GetAllAsync();

        Assert.Equal(2, ((List<User>)result).Count);
        Assert.Equal("Waldisclaison", ((List<User>)result)[0].Name);
    }

    // ----------------- GetFilteredAsync -----------------
    [Fact]
    public async Task GetFilteredAsync_DeveFiltrarUsuarios()
    {
        var expectedUsers = new List<User>
    {
        new User { Id = "1", Name = "Waldisclaison", Email = "edu@test.com" }
    };

        var cursorMock = new Mock<IAsyncCursor<User>>();
        cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(c => c.Current).Returns(expectedUsers);

        _usersCollectionMock
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                     It.IsAny<FindOptions<User, User>>(),
                                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        _usersCollectionMock
            .Setup(c => c.CountDocumentsAsync(It.IsAny<FilterDefinition<User>>(), null, default))
            .ReturnsAsync(1);

        var (resultUsers, totalCount) = await _service.GetFilteredAsync("Waldisclaison", "waldi@test.com");

        Assert.Single(resultUsers);
        Assert.Equal(1, totalCount);
        Assert.Equal("Waldisclaison", ((List<User>)resultUsers)[0].Name);
    }

    // ----------------- AddAsync -----------------
    [Fact]
    public async Task AddAsync_DeveInserirUsuario()
    {
        var user = new User { Id = "1", Name = "Waldisclaison" };

        _usersCollectionMock
            .Setup(c => c.InsertOneAsync(user, null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var result = await _service.AddAsync(user);

        Assert.Equal(user, result);
        _usersCollectionMock.Verify();
    }

    // ----------------- UpdateAsync -----------------
    [Fact]
    public async Task UpdateAsync_DeveChamarReplaceOneAsync()
    {
        var user = new User { Id = "1", Name = "NovoNome" };

        _usersCollectionMock
            .Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<User>>(), user, It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ReplaceOneResult.Acknowledged(1, 1, ObjectId.GenerateNewId()))
            .Verifiable();

        await _service.UpdateAsync(user);

        _usersCollectionMock.Verify();
    }

    // ----------------- GetByIdAsync -----------------
    [Fact]
    public async Task GetByIdAsync_DeveRetornarUsuario()
    {
        var user = new User { Id = "1", Name = "Waldisclaison" };

        var cursorMock = new Mock<IAsyncCursor<User>>();
        cursorMock.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        cursorMock.SetupGet(_ => _.Current).Returns(new List<User> { user });

        _usersCollectionMock
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<User>>(),
                It.IsAny<FindOptions<User, User>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _service.GetByIdAsync("1");

        Assert.NotNull(result);
        Assert.Equal("Waldisclaison", result!.Name);
    }

    // ----------------- DeleteAsync -----------------
    [Fact]
    public async Task DeleteAsync_DeveChamarDeleteOneAsync()
    {
        var user = new User { Id = "1" };

        _usersCollectionMock
            .Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<User>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteResult.Acknowledged(1))
            .Verifiable();

        await _service.DeleteAsync(user);

        _usersCollectionMock.Verify();
    }

    // ----------------- GetByEmailAsync -----------------
    [Fact]
    public async Task GetByEmailAsync_DeveRetornarUsuario()
    {
        var user = new User { Id = "1", Email = "teste@teste.com" };

        var cursorMock = new Mock<IAsyncCursor<User>>();
        cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        cursorMock.SetupGet(c => c.Current).Returns(new List<User> { user });

        _usersCollectionMock
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(), It.IsAny<FindOptions<User, User>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _service.GetByEmailAsync("teste@teste.com");

        Assert.NotNull(result);
        Assert.Equal("teste@teste.com", result!.Email);
    }

}
