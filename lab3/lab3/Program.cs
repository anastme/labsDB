using lab3;
using lab3.Utilities.Domain;
using lab3.Utilities.Middleware;
using lab3.Utilities.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UtilitiesContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddTransient<CachedApartmentsService>();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
var app = builder.Build();
app.UseSession();
app.Map("/", (context) =>
{
    string html = "";
    html += "<h1><a href='/info'>User info</a></h1>";
    html += "<h1><a href='/apartments'>Cashed apartments list</a></h1>";
    html += "<h1><a href='/cookiesSearchApartments'>Cookie apartment searching</a></h1>";
    html += "<h1><a href='/sessionSearchApartments'>Session apartment searching</a></h1>";
    return context.Response.WriteAsync(html);
});
app.Map("/info", (context) =>
{
    string html = "";
    html += $"<h1>Browser: {context.Request.Headers["sec-ch-ua"]}</h1>";
    html += $"<h1>Platform: {context.Request.Headers["sec-ch-ua-platform"]}</h1>";
    return context.Response.WriteAsync(html);
});

app.Map("/apartments", (context) =>
{
    CachedApartmentsService cachedApartmentsService = context.RequestServices.GetService<CachedApartmentsService>();
    string html = "<html><head>" +
                "<Title>Apartments</Title></head>" +
                "<meta http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                "<body><h1>Список квартир</h1>";

    html += "<table border=1>";

    html += "<th><td>Имя</td><td>Фамилия</td><td>Отчество</td><td>Кол-во людей</td><td>Площадь</td></th>";

    IEnumerable<Apartment> apartments = cachedApartmentsService.GetApartments("Apartments20");

    foreach (Apartment a in apartments)
    {
        html += $"<tr><td>{a.Id}</td><td>{a.FirstName}</td><td>{a.LastName}</td><td>{a.MiddleName}</td><td>{a.HumanCount}</td><td>{a.Square}</td></tr>";
    }

    html += "</table></body></html>";
    return context.Response.WriteAsync(html);
});

app.UseMiddleware<CookieApartmentSearchMiddleware>();
app.UseMiddleware<SessionApartmentSearchMiddleware>();
app.Run();