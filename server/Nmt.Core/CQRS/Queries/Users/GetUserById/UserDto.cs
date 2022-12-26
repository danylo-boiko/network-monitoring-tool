namespace Nmt.Core.CQRS.Queries.Users.GetUserById;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}