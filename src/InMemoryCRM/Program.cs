using System.Collections.Concurrent;
using InMemoryCRM;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var usersDict = new ConcurrentDictionary<string, CRMUser>();
var tripsDict = new ConcurrentDictionary<string, CRMTrip>();

app.MapGet("/users/", () => Results.Ok(usersDict.Values)).WithOpenApi();

app.MapGet("/users/{id}", (string id) =>
{
    if (usersDict.TryGetValue(id, out var user))
        return Results.Ok(user);

    return Results.NotFound();
}).WithOpenApi();

app.MapPost("/users/", ([FromBody] CRMUser user) =>
{
    if (usersDict.TryAdd(user.Id, user))
        return Results.Created("/users", user);

    return Results.Conflict();
}).WithOpenApi();

app.MapPost("/users/{id}", (string id, [FromBody] CRMUser user) =>
{
    if (!usersDict.ContainsKey(id))
        return Results.NotFound();

    usersDict[id] = user;
    return Results.Ok(user);
}).WithOpenApi();

app.MapGet("/bookings/", () => Results.Ok(usersDict.Values)).WithOpenApi();

app.MapGet("/bookings/{id}", (string id) =>
{
    if (usersDict.TryGetValue(id, out var user))
        return Results.Ok(user);

    return Results.NotFound();
}).WithOpenApi();

app.MapPost("/bookings/", ([FromBody] CRMTrip trip) =>
{
    if (tripsDict.TryAdd(trip.Id, trip))
        return Results.Created("/bookings", trip);

    return Results.Conflict();
}).WithOpenApi();

app.MapPost("/bookings/{id}", (string id, [FromBody] CRMTrip trip) =>
{
    if (!tripsDict.ContainsKey(id))
        return Results.NotFound();

    tripsDict[id] = trip;
    return Results.Ok(trip);
}).WithOpenApi();

app.Run();