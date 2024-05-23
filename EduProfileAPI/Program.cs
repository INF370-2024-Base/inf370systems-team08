using Microsoft.EntityFrameworkCore;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Repositories.Implementation;
using EduProfileAPI.Repositories.Interfaces;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Database connection
builder.Services.AddDbContext<EduProfileDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the repositories
builder.Services.AddScoped<IGradeRepository, GradeRepository>(); // add this for all the repositories created.
builder.Services.AddScoped<IClass, ClassRepository>();
builder.Services.AddScoped<IEducationPhaseRepository, EducationPhaseRepository>(); 
builder.Services.AddScoped<IMeritRepository, MeritRepository>();
builder.Services.AddScoped<IStudentDocRepository, StudentDocRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();