

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<QuizDb>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<QuizDb>();
    db.Database.EnsureCreated();
}

app.MapGet("/QuizAPI/Users", async (QuizDb db) => await db.Users.ToListAsync());

app.MapGet("/QuizAPI/Users/{id}", async (int id, QuizDb db) => 
    await db.Users.FirstOrDefaultAsync(h => h.Id == id) is User user
    ? Results.Ok(user)
    : Results.NotFound());

app.MapPost("/QuizAPI/Users", async ([FromBody] User user, QuizDb db) =>
{
    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return Results.Created($"/QuizAPI/Users/{user.Id}", user);
});

app.MapPut("/QuizAPI/Users", async ([FromBody] User user, QuizDb db) =>
{
    var userFromDb = await db.Users.FindAsync(new object[] { user.Id });
    if (userFromDb == null)
        return Results.NotFound();
    userFromDb.Login = user.Login;
    userFromDb.Password = user.Password;
    userFromDb.Token = user.Token;
    await db.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/QuizAPI/Users", async (int id, QuizDb db) =>
{
    var userFromDb = await db.Users.FindAsync(new object[] { id });
    if (userFromDb == null)
        return Results.NotFound();
    db.Users.Remove(userFromDb);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.Run();
