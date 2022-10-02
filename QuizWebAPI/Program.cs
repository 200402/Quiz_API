using QuizWebAPI.Data;
using System;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var qwer = "1";

builder.Services.AddDbContext<QuizDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MsSQLServer")));
builder.Services.AddScoped<UserRepository>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => c.DefaultModelsExpandDepth(-1));
app.MapGet("/", () => qwer).ExcludeFromDescription();

app.MapGet("/QuizAPI/Users", async (UserRepository repository) =>
    Results.Ok(await repository.GetAllUsersAsync()))
    .Produces<List<User>>(StatusCodes.Status200OK)
    .WithName("GetAllUsers")
    .WithTags("User");

app.MapGet("/QuizAPI/Users/{id}", async (int id, UserRepository repository) => 
    await repository.GetUserByIdAsync(id) is User user
    ? Results.Ok(user)
    : Results.NotFound())
    .Produces<User>(StatusCodes.Status200OK)
    .WithName("GetUserById")
    .WithTags("User");

app.MapGet("/QuizAPI/CreateRandomUser", async (UserRepository repository) =>
{
    var user = repository.CreateRandomUser();
    await repository.SaveAsync();
    return Results.Created($"/QuizAPI/Users/{user.Id}", user);
}).Accepts<User>("application/json")
    .Produces<User>(StatusCodes.Status201Created)
    .WithName("CreateRandomUser")
    .WithTags("User");

app.MapPost("/QuizAPI/Users", async ([FromBody] User user, UserRepository repository) =>
{
    await repository.InsertUserAsync(user);
    await repository.SaveAsync();
    return Results.Created($"/QuizAPI/Users/{user.Id}", user);
}).Accepts<User>("application/json")
    .Produces<User>(StatusCodes.Status201Created)
    .WithName("CreateUser")
    .WithTags("User");

app.MapPut("/QuizAPI/Users", async ([FromBody] User user, UserRepository repository) =>
{
    await repository.UpdateUserAsync(user);
    await repository.SaveAsync();
    return Results.NoContent();
}).Accepts<User>("application/json")
    .WithName("UpdateUser")
    .WithTags("User");

app.MapDelete("/QuizAPI/Users", async (int id, UserRepository repository) =>
{
    await repository.DeleteUserAsync(id);
    await repository.SaveAsync();
    return Results.NoContent();
}).Accepts<User>("application/json")
    .WithName("DeleteUser")
    .WithTags("User");

app.UseHttpsRedirection();

app.Run();
