using System.Security.Cryptography;
using System.Text;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
       [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly StoreContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(StoreContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST: api/Account/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("Este correo ya está registrado.");

            using var hmac = new HMACSHA512();

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
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

        // POST: api/Account/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null) return Unauthorized("Usuario no encontrado");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash))
                return Unauthorized("Contraseña incorrecta");

            return new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
