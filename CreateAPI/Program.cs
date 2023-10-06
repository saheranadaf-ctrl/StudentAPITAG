using CreateAPI.DataAccess;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StudentDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CS"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

CreateWebHostBuilder(args).Build().Run();


    static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.AddConsole(); // Log to the console
                logging.AddDebug();   // Log to debug output window in Visual Studio Code
            })
            .UseStartup<Program>() // Specify your startup class here if you have one
            .UseUrls("http://localhost:5000") // Set the URL of your API
            ;

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

 void ConfigureServices(IServiceCollection services)
{
    // ... other configurations

    // Register DbContext and DbSeeder as services
    services.AddDbContext<StudentDataContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
    services.AddScoped<DbSeeder>();

    // ... other configurations
}

 void Configure(IApplicationBuilder app, IHostingEnvironment env, DbSeeder dbSeeder)
{
    // ... other configurations

    // Seed the database
    dbSeeder.SeedDatabase();

    // ... other configurations
}



