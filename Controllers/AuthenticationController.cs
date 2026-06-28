using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OfficeRoomie.Database;
using System.Security.Claims;
using OfficeRoomie.Models.ViewModels;
using Microsoft.IdentityModel.Tokens;
using OfficeRoomie.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OfficeRoomie.Helpers;
using OfficeRoomie.Security;

namespace OfficeRoomie.Controllers;

[Route("authentication")]
public class AuthenticationController : Controller
{
    private readonly AppDbContext _context;

    public AuthenticationController(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    [HttpGet("login")]
    public IActionResult Index()
    {
        return View("Login");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromForm] AuthenticationLoginDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Dados inválidos fornecidos.");
                return View("Login");
            }

            if (dto == null || string.IsNullOrEmpty(dto.email) || string.IsNullOrEmpty(dto.password))
            {
                ModelState.AddModelError(string.Empty, "O email e a senha são obrigatórios.");
                return View("Login", dto);
            }

            var user = await ValidateAdministradorAsync(dto.email, dto.password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha incorretos.");
                return View("Login", dto);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name, user.nome)
            };

            var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            var identity = new ClaimsIdentity(claims, authScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(authScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true
            });

            var returnUrl = Request.Form["ReturnUrl"].ToString();
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/dashboard");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro no servidor. Tente novamente mais tarde.");
            return View("Login");
        }
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect("/authentication/login");
    }

    public async Task<Administrador?> ValidateAdministradorAsync(string email, string password)
    {
        var administrador = await _context.Administradores.FirstOrDefaultAsync(x => x.email == email);

        if (administrador == null || !PasswordHelper.VerifyPassword(password, administrador.senha))
        {
            return null;
        }

        return administrador;
    }

    public string CreateToken(Administrador administrador)
    {
        var handler = new JwtSecurityTokenHandler();

        var privateKey = Encoding.UTF8.GetBytes(SecurityConfiguration.PrivateKey);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(privateKey),
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "yourIssuer",
            Audience = "yourAudience",
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(1),
            Subject = GenerateClaims(administrador)
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(Administrador administrador)
    {
        var ci = new ClaimsIdentity();

        ci.AddClaim(new Claim("id", administrador.id.ToString()));
        ci.AddClaim(new Claim(ClaimTypes.Name, administrador.nome));
        ci.AddClaim(new Claim(ClaimTypes.GivenName, administrador.nome));
        ci.AddClaim(new Claim(ClaimTypes.Email, administrador.email));

        return ci;
    }
}
