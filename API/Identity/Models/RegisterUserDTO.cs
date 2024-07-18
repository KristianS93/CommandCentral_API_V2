namespace API.identity.Models;

public record RegisterUserDto(string Email, string FirstName, string LastName, string Password, string VerifyPassword);