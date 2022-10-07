using Microsoft.Data.SqlClient;
using QuizWebAPI.Data;
using System;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var qwer = "https://einka.ru/swagger/index.html";

//builder.Services.AddDbContext<QuizDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MsSQLLocal")));
//builder.Services.AddDbContext<QuizDb>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MsSQLServer")));
SqlConnection myConn = new SqlConnection(builder.Configuration.GetConnectionString("MsSQLServer"));
//SqlConnection myConn = new SqlConnection(builder.Configuration.GetConnectionString("MsSQLLocal"));
UserRepository userRepository = new UserRepository(myConn);

//builder.Services.AddScoped<UserRepository>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => c.DefaultModelsExpandDepth(-1));
app.MapGet("/", () => qwer).ExcludeFromDescription();

app.MapGet("/QuizAPI/Users", async () =>
    Results.Ok(await userRepository.GetAllUsersAsync()))
    .Produces<List<User>>(StatusCodes.Status200OK)
    .WithName("Get all users")
    .WithTags("User");

app.MapGet("/QuizAPI/Users/{id}", async (int id) =>
    await userRepository.GetUserByIdAsync(id) is User user
    ? Results.Ok(user)
    : Results.NotFound())
    .WithTags("User");

app.MapPost("/QuizAPI/CreateUser", async ([FromBody] NewUser newUser) =>
{
    if (userRepository.GetUserByLoginAsync(newUser.Login).Id > 0)
    {
        var user = await userRepository.InsertUserAsync(newUser);
        user = await userRepository.GetUserByTokenAsync(user.Token);
        return Results.Created($"/QuizAPI/Users/{user.Id}", user);
    }
    return Results.StatusCode(401);
}).Accepts<NewUser>("application/json")
    .WithName("CreateRandomUser")
    .WithTags("User");

//app.MapPost("/QuizAPI/UpdateUser", async ([FromBody] User user, UserRepository repository) =>
//{
//    return user;
//    //await repository.UpdateUserAsync(user);
//    //await repository.SaveAsync();
//    //return Results.Created($"/QuizAPI/Users/{user.Id}", user);
//}).Accepts<User>("application/json")
//    .WithName("UpdateUser")
//    .WithTags("User");

app.MapPost("/QuizAPI/CreateRandomUser", async () =>
{
    var user = await userRepository.CreateRandomUser();
    user = await userRepository.GetUserByTokenAsync(user.Token);
    return Results.Created($"/QuizAPI/Users/{user.Id}", user);
}).WithName("Create new User")
    .WithTags("User");

//app.MapPost("/QuizAPI/AuthorizationUser", async ([FromBody] AuthorizationUser userLogPass, UserRepository repository) =>
//{
//    var user = await repository.AuthorizationUser(userLogPass.Login, userLogPass.Password);
//    return userLogPass;
//    //if (user.Token != "")
//    //    Results.Ok(user);
//    //else if (user.Login == "")
//    //    Results.StatusCode(404);
//    //else Results.StatusCode(401);
//}).Accepts<AuthorizationUser>("application/json")
//    .WithName("AuthorizationUser")
//    .WithTags("User");

static User AuthorizationUserToUser(NewUser userLogPass)
{
    User user = new()
    {
        Login = userLogPass.Login,
        Password = userLogPass.Password
    };
    return user;
}

app.UseHttpsRedirection();

app.Run();
