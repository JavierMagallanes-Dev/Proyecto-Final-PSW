using System;

namespace Core.DTOs;

public class UserDto
{
    public string Email { get; set; }
    public string FullName { get; set; }  // si lo usas
    public string Username { get; set; }  // ✅ Agrega esta línea
    public string Role { get; set; }
    public string Token { get; set; }
}
