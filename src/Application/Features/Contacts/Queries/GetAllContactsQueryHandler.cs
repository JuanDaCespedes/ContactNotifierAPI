using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Contacts.Queries;

public class GetAllContactsQueryHandler(IContactFinder finder)
    : IRequestHandler<GetAllContactsQuery, List<Contact>>
{
    public async Task<List<Contact>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
    {
        return await finder.GetAllContactsAsync(cancellationToken);
    }
}