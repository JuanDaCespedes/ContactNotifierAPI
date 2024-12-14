using Domain.Entities;

namespace Domain.Interfaces;

public interface IContactRepository
{
    Task AddContactAsync(Contact contact, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}