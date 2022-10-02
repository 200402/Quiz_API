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
    .WithName("Get all users")
    .WithTags("User");

app.MapPut("/QuizAPI/AuthorizationUser", async ([FromBody] User user, UserRepository repository) =>
{
    await repository.AuthorizationUser(user.Login,user.Password);
    await repository.SaveAsync();
    return Results.Created($"/QuizAPI/Users/{user.Id}", user);
}).Accepts<User>("application/json")
    .Produces<User>(StatusCodes.Status200OK)
    .WithName("AuthorizationUser")
    .WithTags("User");

app.MapPut("/QuizAPI/Users", async ([FromBody] User user, UserRepository repository) =>
{
    await repository.UpdateUserAsync(user);
    await repository.SaveAsync();
    return Results.Created($"/QuizAPI/Users/{user.Id}", user);
}).Accepts<User>("application/json")
    .WithName("UpdateUser")
    .WithTags("User");

app.MapPost("/QuizAPI/CreateRandomUser", async (UserRepository repository) =>
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

app.UseHttpsRedirection();

app.Run();
