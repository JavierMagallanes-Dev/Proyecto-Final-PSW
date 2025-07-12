using System;

namespace Core.Entities;

public class User : Entities
{
  
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required string Role { get; set; } // "Admin" o "Client"
    
}
