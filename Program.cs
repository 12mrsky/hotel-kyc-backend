using Hotel_KYC_Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Services to the container
builder.Services.AddControllers();

// 2. Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// 4. Database Context (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(3); // retry if connection fails
        }));

var app = builder.Build();


// ✅ 5. SAFE DATABASE MIGRATION (IMPORTANT FIX)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        Console.WriteLine("Connecting to database...");

        db.Database.Migrate();

        Console.WriteLine("Database connected & migrated successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine(" DATABASE ERROR: " + ex.Message);
    }
}


// 6. Configure HTTP pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel KYC API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

// 7. Routing & CORS
app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();