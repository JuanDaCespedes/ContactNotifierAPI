using Application.Features.Contacts.Handlers;
using Application.Features.Contacts.Queries;
using Infrastructure.DI;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging with Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration));

// Add MediatR
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(CreateContactCommand).Assembly); });

// Add infrastructure (DbContext, Repositories, finders)
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints minimal api
app.MapPost("/api/contacts", async (CreateContactCommand command, IMediator mediator) =>
{
    await mediator.Send(command);
    return Results.Ok(new
        { message = $"Datos recibidos correctamente del usuario: {command.Name}, con número: {command.PhoneNumber}" });
});
app.MapGet("/api/contacts", async (IMediator mediator) =>
{
    var contacts = await mediator.Send(new GetAllContactsQuery());
    return Results.Ok(contacts);
});

app.Run();