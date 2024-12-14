using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ContactRepository(ApplicationDbContext context) : IContactRepository
{
    public async Task AddContactAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        await context.Contacts.AddAsync(contact, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}