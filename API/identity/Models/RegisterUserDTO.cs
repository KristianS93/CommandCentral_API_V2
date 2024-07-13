namespace API.identity.Models;

public record RegisterUserDTO(string email, string username, string firstname, string lastname, string password, string verifyPassword);