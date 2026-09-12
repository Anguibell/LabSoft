namespace LabSoft.Application.Interfaces;

public interface ICurrentUserService
{
    Guid? GetUserId();
}