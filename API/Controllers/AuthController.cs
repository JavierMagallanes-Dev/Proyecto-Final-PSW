using System.Security.Cryptography;
using System.Text;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly StoreContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(StoreContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
{
    if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
    {
        return BadRequest(new { message = "El correo ya está registrado." });
    }

    using var hmac = new System.Security.Cryptography.HMACSHA512();

    var user = new User
    {
        Username = dto.Username,
        Email = dto.Email,
        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
        PasswordSalt = hmac.Key,
        Role = dto.Role
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return new UserDto
    {
        Username = user.Username,
        Email = user.Email,
        Role = user.Role,
        Token = _tokenService.CreateToken(user)
    };
}

    [HttpPost("login")]
public async Task<ActionResult<string>> Login(LoginDto dto)
{
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

    if (user == null)
        return Unauthorized(new { message = "Usuario no encontrado." });

    using var hmac = new HMACSHA512(user.PasswordSalt);
    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

    for (int i = 0; i < computedHash.Length; i++)
    {
        if (computedHash[i] != user.PasswordHash[i])
            return Unauthorized(new { message = "Contraseña incorrecta." });
    }

    var token = _tokenService.CreateToken(user);
    return Ok(new { token });
}

}
