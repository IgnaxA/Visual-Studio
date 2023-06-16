using Practice;
using Practice.Data.Interface;
using Practice.Data.Mocks;
using Practice.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();

builder.Services.AddTransient<ITeachers, MockTeachers>();
builder.Services.AddTransient<IThemes, MockThemes>();
builder.Services.AddTransient<IStudents, MockStudents>();
builder.Services.AddTransient<IFaculties, MockFaculties>();
builder.Services.AddTransient<ICourses, MockCourses>();
builder.Services.AddTransient<IRoles, MockRoles>();
builder.Services.AddTransient<ITeams, MockTeams>();
builder.Services.AddTransient<IDeadlines, MockDeadline>();
builder.Services.AddTransient<IConsultations, MockConsultations>();
builder.Services.AddTransient<IDegrees, MockDegrees>();
builder.Services.AddTransient<IEmploymentTypes, MockEmploymentTypes>();

builder.Services.AddSingleton<ReportService>();

builder.Services.AddDbContext<InformationSystemToRecordProjectActivitiesDatabaseContext>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Teachers}/{action=TeachersList}"); 

app.Run();