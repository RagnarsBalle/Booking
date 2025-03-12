using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // POST: /api/auth/register
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel model)
    {
        // Logik för att registrera en ny användare
        return Ok(new { message = "Registrering lyckades!" });
    }

    // POST: /api/auth/login
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        // Logik för att logga in en användare
        return Ok(new { message = "Inloggning lyckades!", sessionId = "uniktSessionsId" });
    }

    // GET: /api/auth/protected
    [HttpGet("protected")]
    public IActionResult Protected()
    {
        // Logik för att kontrollera om en användare är inloggad
        return Ok(new { message = "Du är inloggad!", userId = 1, userRole = 2 });
    }

    // POST: /api/auth/logout
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Logik för att logga ut en användare
        return Ok(new { message = "Du har loggats ut och sessionen har rensats." });
    }

    // DELETE: /api/auth/delete/{userId}
    [HttpDelete("delete/{userId}")]
    public IActionResult Delete(int userId, [FromHeader] string adminToken)
    {
        // Logik för att radera en användare (endast admin)
        return Ok(new { message = $"Användaren med ID {userId} har tagits bort." });
    }
}