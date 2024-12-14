using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Finders;

public class ContactFinder(IDbConnection dbConnection) : IContactFinder
{
    public async Task<List<Contact>> GetAllContactsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """ SELECT "Id", "Name", "PhoneNumber" FROM "Contacts"; """;
        var result = await dbConnection.QueryAsync<Contact>(sql);

        return result.AsList();
    }
}