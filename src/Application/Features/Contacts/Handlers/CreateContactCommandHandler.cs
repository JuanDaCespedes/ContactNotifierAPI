using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Contacts.Handlers;

public class CreateContactCommandHandler(IContactRepository repository, ILogger<CreateContactCommandHandler> logger)
    : IRequestHandler<CreateContactCommand, Unit>
{
    public async Task<Unit> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = new Contact(request.Name, request.PhoneNumber);
        await repository.AddContactAsync(contact, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        // Simulate message sending by logging
        logger.LogInformation("Mensaje de bienvenida enviado a {Name} al número {PhoneNumber}", request.Name,
            request.PhoneNumber);

        return Unit.Value;
    }
}