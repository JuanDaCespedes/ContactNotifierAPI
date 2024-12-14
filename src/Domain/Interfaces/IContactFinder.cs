using Domain.Entities;

namespace Domain.Interfaces;

public interface IContactFinder
{
    Task<List<Contact>> GetAllContactsAsync(CancellationToken cancellationToken = default);
}