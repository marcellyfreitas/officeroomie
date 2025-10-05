using Xunit;
using Moq;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using WebApi.Services;
using WebApi.Database;
using WebApi.Models;
using WebApi.Models.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace webapi.testes;

public class AuthenticationServiceTest
{
    private readonly Mock<IMongoCollection<User>> _usersCollectionMock;
    private readonly Mock<IMongoCollection<Administrator>> _adminsCollectionMock;
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthenticationService _service;

    public AuthenticationServiceTest()
    {
        _usersCollectionMock = new Mock<IMongoCollection<User>>();
        _adminsCollectionMock = new Mock<IMongoCollection<Administrator>>();
        _contextMock = new Mock<IApplicationDbContext>();
        _contextMock.Setup(c => c.Users).Returns(_usersCollectionMock.Object);
        _contextMock.Setup(c => c.Administrators).Returns(_adminsCollectionMock.Object);

        _configMock = new Mock<IConfiguration>();
        var jwtSectionMock = new Mock<IConfigurationSection>();
        jwtSectionMock.Setup(x => x["UserSecretKey"]).Returns("user_secret_key_1234567890123456");
        jwtSectionMock.Setup(x => x["AdminSecretKey"]).Returns("admin_secret_key_1234567890123456");
        jwtSectionMock.Setup(x => x["Issuer"]).Returns("TestIssuer");
        jwtSectionMock.Setup(x => x["Audience"]).Returns("TestAudience");
        _configMock.Setup(x => x.GetSection("JwtSettings")).Returns(jwtSectionMock.Object);

        _service = new AuthenticationService(_contextMock.Object, _configMock.Object);
    }

    // ----------------- LoginUserAsync -----------------
    [Fact]
    public async Task LoginUserAsync_DeveRetornarToken_QuandoCredenciaisCorretas()
    {
        var dto = new LoginDto { Email = "user@test.com", Password = "1234" };
        var user = new User { Id = "1", Email = dto.Email, Name = "User1", Password = WebApi.Helpers.PasswordHelper.HashPassword("1234") };

        var cursorMock = new Mock<IAsyncCursor<User>>();
        cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(c => c.Current).Returns(new List<User> { user });

        _usersCollectionMock
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                     It.IsAny<FindOptions<User, User>>(),
                                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var token = await _service.LoginUserAsync(dto);

        Assert.False(string.IsNullOrEmpty(token));
    }

    [Fact]
    public async Task LoginUserAsync_DeveRetornarNull_QuandoSenhaIncorreta()
    {
        var dto = new LoginDto { Email = "user@test.com", Password = "wrongpassword" };
        var user = new User { Id = "1", Email = dto.Email, Name = "User1", Password = WebApi.Helpers.PasswordHelper.HashPassword("1234") };

        var cursorMock = new Mock<IAsyncCursor<User>>();
        cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(c => c.Current).Returns(new List<User> { user });

        _usersCollectionMock
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                     It.IsAny<FindOptions<User, User>>(),
                                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var token = await _service.LoginUserAsync(dto);

        Assert.Null(token);
    }

    // ----------------- RegisterUserAsync -----------------
    [Fact]
    public async Task RegisterUserAsync_DeveInserirUsuario_E_RetornarToken()
    {
        var user = new User { Id = "1", Email = "newuser@test.com", Name = "NewUser", Password = "1234" };

        _usersCollectionMock
            .Setup(c => c.InsertOneAsync(user, null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var token = await _service.RegisterUserAsync(user);

        Assert.False(string.IsNullOrEmpty(token));
        _usersCollectionMock.Verify();
    }

    // ----------------- LoginAdminAsync -----------------
    [Fact]
    public async Task LoginAdminAsync_DeveRetornarToken_QuandoCredenciaisCorretas()
    {
        var dto = new LoginDto { Email = "admin@test.com", Password = "admin123" };
        var admin = new Administrator { Id = "1", Email = dto.Email, Name = "Admin1", Password = WebApi.Helpers.PasswordHelper.HashPassword("admin123") };

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

        var token = await _service.LoginAdminAsync(dto);

        Assert.False(string.IsNullOrEmpty(token));
    }

    // ----------------- GetUserAsync -----------------
    [Fact]
    public async Task GetUserAsync_DeveRetornarUsuario()
    {
        var user = new User { Id = "1", Name = "User1" };

        var cursorMock = new Mock<IAsyncCursor<User>>();
        cursorMock.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(_ => _.Current).Returns(new List<User> { user });

        _usersCollectionMock
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                     It.IsAny<FindOptions<User, User>>(),
                                     It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _service.GetUserAsync("1");

        Assert.NotNull(result);
        Assert.Equal("User1", result!.Name);
    }

    // ----------------- GetAdminAsync -----------------
    [Fact]
    public async Task GetAdminAsync_DeveRetornarAdministrador()
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

        var result = await _service.GetAdminAsync("1");

        Assert.NotNull(result);
        Assert.Equal("Admin1", result!.Name);
    }
}
