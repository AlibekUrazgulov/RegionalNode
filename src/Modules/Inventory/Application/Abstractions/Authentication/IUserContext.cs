namespace Inventory.Application.Abstractions.Authentication;

public interface IUserContext
{
    long PostId { get; }
    long MedicalOrganId { get; }
    long UserId { get; }
    string? Token { get; }
}
