using Domain.Entities;
using MediatR;

namespace Application.Features.Contacts.Queries;

public record GetAllContactsQuery : IRequest<List<Contact>>;