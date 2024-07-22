namespace API.identity.Models;

public record UserDto(string UserId, string Email, string FirstName, string LastName, string Role);