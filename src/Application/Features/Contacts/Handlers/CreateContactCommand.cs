using MediatR;

namespace Application.Features.Contacts.Handlers;

public record CreateContactCommand(string Name, string PhoneNumber) : IRequest<Unit>;