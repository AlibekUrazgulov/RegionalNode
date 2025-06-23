namespace Inventory.Application.Abstractions.Authentication;

public interface IUserContext
{
    long EmployeeId { get; }
    long MedicalOrganId { get; }
    long UserId { get; }
    string? Token { get; }
}
