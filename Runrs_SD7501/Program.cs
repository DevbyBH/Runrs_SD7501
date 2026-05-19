using Microsoft.EntityFrameworkCore;
using Runrs.DataAccess.Repository.IRepository;
using Runrs.DataAccess.Repository;
using Runrs.Models;
using Runrs.DataAccess.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IClubRepository, ClubRepository>(); // <------ Byron 18/04/2026 - Registered the IClubRepository and ClubRepository services for dependency injection in the ClubController
builder.Services.AddScoped<IUserRepository, UserRepository>(); // <------ Byron 18/04/2026 - Registered the IUserRepository and UserRepository services for dependency injection in the LoginController
builder.Services.AddScoped<IMembershipRepository, MembershipRepository>(); // <------ Byron 18/04/2026 - Registered the IMembership and MembershipRepository services for dependency injection in the ClubController (for future use)
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // <------ Byron 18/04/2026 - Registered the IUnitOfWork and UnitOfWork services for dependency injection in the Club & Login Controllers  
builder.Services.AddScoped<IEventRepository, EventRepository>(); // <------ Byron 16/05/2026 - Registered the IEventRepository and EventRepository services for dependency injection in the EventController 
builder.Services.AddScoped<IEventRegistrationRepository, EventRegistrationRepository>(); // <------ Byron 16/05/2026 - Registered the IEventRegistrationRepository and EventRegistrationRepository services for dependency injection in the EventController 


builder.Services.AddSession(); // <------ Byron 10/04/2026 - Registered session services for Mo's LoginController
builder.Services.AddHttpContextAccessor(); // <------ Byron 10/04/2026 - Registered HttpContextAccessor services for Mo's LoginController

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession(); // <------ Byron 10/04/2026 - Added session "middleware" for Mo's LoginController
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
