
using Booking.Data;
using Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Room.Data;
using Room.DTOs;
using System.Security.Cryptography;
using System.Text;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    // POST: /api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto request, [FromHeader] string adminToken = null)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            return BadRequest(new { message = "Användarnamnet är redan taget." });

        int roleId = 3; // Standard: Gäst

        if (request.RoleID == 2) // Om anställd ska skapas
        {
            if (string.IsNullOrEmpty(adminToken) || !ValidateAdminToken(adminToken))
                return Unauthorized(new { message = "Endast administratörer kan skapa anställda." });

            roleId = 2; // Anställd
        }

        var user = new User
        {
            Username = request.Username,
            PasswordHash = HashPassword(request.PasswordHash),
            RoleID = roleId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Registrering lyckades!" });
    }

    // POST: /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null || !VerifyPassword(request.PasswordHash, user.PasswordHash))
            return Unauthorized(new { message = "Felaktigt användarnamn eller lösenord." });

        var sessionToken = Guid.NewGuid().ToString();
        var session = new Session
        {
            UserId = user.Id,
            SessionToken = sessionToken,
            ExpirationTime = DateTime.UtcNow.AddMinutes(30)
        };

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();

        Response.Cookies.Append("SessionID", sessionToken, new CookieOptions { HttpOnly = true });

        return Ok(new { message = "Inloggning lyckades!", sessionId = sessionToken });
    }

    // GET: /api/auth/protected
    [HttpGet("protected")]
    public async Task<IActionResult> CheckLoginStatus()
    {
        if (!Request.Cookies.TryGetValue("SessionID", out string sessionId))
            return Unauthorized(new { message = "Sessionen har gått ut, vänligen logga in igen." });

        var session = await _context.Sessions.Include(s => s.User).FirstOrDefaultAsync(s => s.SessionToken == sessionId);

        if (session == null || session.ExpirationTime < DateTime.UtcNow)
            return Unauthorized(new { message = "Sessionen har gått ut, vänligen logga in igen." });

        return Ok(new { message = "Du är inloggad!", userId = session.User.Id, userRole = session.User.RoleID });
    }

    // POST: /api/auth/logout
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (!Request.Cookies.TryGetValue("SessionID", out string sessionId))
            return BadRequest(new { message = "Ingen aktiv session hittades." });

        var session = await _context.Sessions.FirstOrDefaultAsync(s => s.SessionToken == sessionId);
        if (session != null)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        Response.Cookies.Delete("SessionID");
        return Ok(new { message = "Du har loggats ut och sessionen har rensats." });
    }

    // DELETE: /api/auth/delete/{userId}
    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId, [FromHeader] string adminToken)
    {
        if (string.IsNullOrEmpty(adminToken) || !ValidateAdminToken(adminToken))
            return Unauthorized(new { message = "Endast administratörer kan ta bort användare." });

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return NotFound(new { message = "Användaren hittades inte." });

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Användaren med ID {userId} har tagits bort." });
    }

    // Hasha lösenord (för registrering)
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    // Verifiera lösenord (för inloggning)
    private bool VerifyPassword(string enteredPassword, string storedHash)
    {
        return HashPassword(enteredPassword) == storedHash;
    }

    // Validera AdminToken (hashat lösenord)
    private bool ValidateAdminToken(string adminToken)
    {
        var admin = _context.Users.FirstOrDefault(u => u.RoleID == 1);
        return admin != null && HashPassword(admin.PasswordHash) == adminToken;
    }
}