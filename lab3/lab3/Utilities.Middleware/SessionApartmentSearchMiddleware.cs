using lab3.Utilities.Domain;
using lab3.Utilities.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3.Utilities.Middleware
{
    public class SessionApartmentSearchMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionApartmentSearchMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UtilitiesContext dbcontext)
        {
            if (context.Request.Path == "/sessionSearchApartments")
            {
                string html = "<html><head>" +
                "<Title>Поиск квартиры</Title></head>" +
                "<meta http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                "<body><h1>Поиск квартиры</h1>";
                ApartmentSearchModel apartmentSearchModel = new ApartmentSearchModel("", "", "");
                if (context.Request.Query["searchfirstname"].Count > 0 ||
                    context.Request.Query["searchlastname"].Count > 0 ||
                    context.Request.Query["searchmiddlename"].Count > 0)
                {
                    apartmentSearchModel = new ApartmentSearchModel(context.Request.Query["searchfirstname"],
                                                                    context.Request.Query["searchlastname"],
                                                                    context.Request.Query["searchmiddlename"]);
                }
                else if (context.Session.Keys.Contains("apartment"))
                {
                    apartmentSearchModel = context.Session.Get<ApartmentSearchModel>("apartment");
                }
                else
                {
                    context.Session.Set("apartment", apartmentSearchModel);
                }

                context.Session.Set("apartment", apartmentSearchModel);
                html += $"<form><h2>Имя: </h2><input type='text' name='searchfirstname' value='{apartmentSearchModel.firstname}'/><br/>";
                html += $"<form><h2>Фамилия: </h2><input type='text' name='searchlastname' value='{apartmentSearchModel.lastname}'/><br/>";
                html += $"<form><h2>Отчество: </h2><input type='text' name='searchmiddlename' value='{apartmentSearchModel.middlename}'/><br/>";

                html += "<button name='searchbutton'>Искать</button></form>";
                html += "</body></html>";


                var apartments = dbcontext.Apartments.Where(x => x.FirstName.Contains(apartmentSearchModel.firstname))
                                                 .Where(x => x.LastName.Contains(apartmentSearchModel.lastname))
                                                 .Where(x => x.MiddleName.Contains(apartmentSearchModel.middlename))
                                                 .ToList();

                html += "<table border=1>";
                html += "<th><td>Имя</td><td>Фамилия</td><td>Отчество</td><td>Кол-во людей</td><td>Площадь</td></th>";

                foreach (Apartment a in apartments)
                {
                    html += $"<tr><td>{a.Id}</td><td>{a.FirstName}</td><td>{a.LastName}</td><td>{a.MiddleName}</td><td>{a.HumanCount}</td><td>{a.Square}</td></tr>";
                }
                html += "</table>";
                string newFirstName = context.Request.Query["searchfirstname"];
                string newLastName = context.Request.Query["searclasthname"];
                string newMiddleName = context.Request.Query["searchmiddlename"];
                if (newFirstName != null && newLastName != null && newMiddleName != null)
                    context.Session.Set("apartment", new ApartmentSearchModel(newFirstName, newLastName, newMiddleName));

                context.Response.WriteAsync(html);
            }
            await _next(context);

        }
    }
}
