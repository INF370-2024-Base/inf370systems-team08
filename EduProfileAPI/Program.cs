using Microsoft.EntityFrameworkCore;
using EduProfileAPI.DataAccessLayer;
using EduProfileAPI.Repositories.Implementation;
using EduProfileAPI.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EduProfileAPI.PasswordValidator;
using EduProfileAPI.EmailService;
using EduProfileAPI.Repositories.Interfaces.Maintenance;
using EduProfileAPI.Repositories.Implementation.Maintenance;
using EduProfileAPI.WhatsApp;
using Microsoft.Data.SqlClient;
using EduProfileAPI.SmsService;
using EduProfileAPI.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using EduProfileAPI.AutoBackup;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
});



//email config
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.AddSingleton<IEmailService, MailKitEmailService>();
//Security
//configure the identity 
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<EduProfileDbContext>()
    .AddDefaultTokenProviders();

//Configure the JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

    };



});

// The custom password validation
builder.Services.AddTransient<IPasswordValidator<IdentityUser>, CustomPasswordValidator>();

//Custom password policy
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});

// Add Quartz.NET services
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var jobKey = new JobKey("DatabaseBackupJob");

    q.AddJob<DatabaseBackupJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("DatabaseBackupTrigger")
        .WithCronSchedule("0 0 2  ? * * *"));  // Default schedule for daily at 2 AM
});



// Add Quartz Hosted Service
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// Add services to the container.
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Database connection
builder.Services.AddDbContext<EduProfileDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Register the repositories
builder.Services.AddScoped<IGradeRepository, GradeRepository>(); // add this for all the repositories created.
builder.Services.AddScoped<IClass, ClassRepository>();
builder.Services.AddScoped<IEducationPhaseRepository, EducationPhaseRepository>(); 
builder.Services.AddScoped<IMeritRepository, MeritRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IStudentDocRepository, StudentDocRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentAnnouncementRepo, StudentAnnouncementRepo>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeStatusRepository, EmployeeStatusRepository>();
builder.Services.AddScoped<IAssignTeacherToClassRepository, AssignTeacherToClassRepository>();
builder.Services.AddScoped<IStudentIncidentRepository, StudentIncidentRepository>();
builder.Services.AddScoped<IAssignStudentRepo, AssignStudentRepo>();
builder.Services.AddScoped<IDisciplinaryRepository, DisciplinaryRepository>();
builder.Services.AddScoped<IDisciplinaryTypeRepository, DisciplinaryTypeRepository>();
builder.Services.AddScoped<IMaintenanceRequest, MaintenanceRequestRepo>();
builder.Services.AddScoped<IMaintenanceStatus, MaintenanceStatusRepo>();
builder.Services.AddScoped<IMaintenancePriority, MaintenancePriorityRepo>();
builder.Services.AddScoped<IMaintenanceType, MaintenanceTypeRepo>();
builder.Services.AddScoped<IMaintenanceProcedure, MaintenanceProcedureRepo>();
builder.Services.AddScoped<IAssesment, AssesmentRepo>();
builder.Services.AddScoped<IAssesmentMark, AssesmentMarkRepo>();
builder.Services.AddScoped<IReport, ReportRepo>();
builder.Services.AddScoped<IReportType, ReportTypeRepo>();
builder.Services.AddScoped<IStudentDocumentType, StudentDocumentTypeRepo>();
builder.Services.AddScoped<IStudentAttendanceRepo, StudentAttendanceRepo>();
builder.Services.AddScoped<IContactStudentParent, ContactStudentParent>();
builder.Services.AddScoped<IStudentReportRepository, StudentReportRepository>();
builder.Services.AddScoped<IEarlyReleasesRepo, EarlyReleasesRepo>();
builder.Services.AddScoped<ISchoolEventRepo, SchoolEventRepo>();
builder.Services.AddScoped<ITeacherClassListRepo,  TeacherClassListRepo>();
builder.Services.AddScoped<IRemedialFileRepository, RemedialFileRepository>();
builder.Services.AddScoped<IRemedialActivityRepository, RemedialActivityRepository>();
builder.Services.AddScoped<IAssessmentsReportsRepo, AssessmentsReportsRepo>();
builder.Services.AddScoped<IMeritType, MeritTypeRepo>();
builder.Services.AddScoped<IAuditTrail, AuditTrailRepo>();
builder.Services.AddScoped<IAssesmentTerm, AssesmentTermRepo>();


//WhatsApp
var whatsAppAccessToken = builder.Configuration["WhatsApp:AccessToken"];
builder.Services.AddSingleton(new WhatsAppHelper(whatsAppAccessToken));

// Register the SmsService
builder.Services.AddHttpClient();
builder.Services.AddTransient<ISmsService, SmsService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCors("AllowSpecificOrigin"); // Make sure this matches the name you gave your policy in ConfigureServices
app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});

app.UseAuthorization();

app.MapControllers();

app.Run();