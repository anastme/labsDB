using lab3.Utilities.Domain;

namespace lab3.Utilities.Middleware
{
    public class CookieApartmentSearchMiddleware
    {
        private readonly RequestDelegate _next;

        public CookieApartmentSearchMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UtilitiesContext dbcontext)
        {
            if (context.Request.Path == "/cookiesSearchApartments")
            {
                string html = "<html><head>" +
                "<Title>Поиск квартиры</Title></head>" +
                "<meta http-equiv='Content-Type' content='text/html; charset=utf-8 />'" +
                "<body><h1>Поиск квартиры</h1>";

                string firstname = "", lastname = "", middlename = "";

                if (context.Request.Query["searchfirstname"].Count > 0)
                    firstname = context.Request.Query["searchfirstname"];
                else if (context.Request.Cookies.ContainsKey("firstname"))
                    firstname = context.Request.Cookies["firstname"];
                else
                    context.Response.Cookies.Append("firstname", firstname);

                if (context.Request.Query["searchlastname"].Count > 0)
                    lastname = context.Request.Query["searchlastname"];
                else if (context.Request.Cookies.ContainsKey("lastname"))
                    lastname = context.Request.Cookies["lastname"];
                else
                    context.Response.Cookies.Append("lastname", lastname);

                if (context.Request.Query["searchmiddlename"].Count > 0)
                    middlename = context.Request.Query["searchmiddlename"];
                else if (context.Request.Cookies.ContainsKey("middlename"))
                    middlename = context.Request.Cookies["middlename"];
                else
                    context.Response.Cookies.Append("middlename", middlename);

                html += $"<form><h2>Имя: </h2><input type='text' name='searchfirstname' value='{firstname}'/><br/>";
                html += $"<form><h2>Фамилия: </h2><input type='text' name='searchlastname' value='{lastname}'/><br/>";
                html += $"<form><h2>Отчество: </h2><input type='text' name='searchmiddlename' value='{middlename}'/><br/>";

                html += "<button name='searchbutton'>Искать</button></form>";
                html += "</body></html>";


                var apartments = dbcontext.Apartments.Where(a => a.FirstName.Contains(firstname))
                                                     .Where(a => a.LastName.Contains(lastname))
                                                     .Where(a => a.MiddleName.Contains(middlename))
                                                     .ToList();
                html += "<table border=1>";
                html += "<th><td>Имя</td><td>Фамилия</td><td>Отчество</td><td>Кол-во людей</td><td>Площадь</td></th>";

                foreach (Apartment a in apartments)
                {
                    html += $"<tr><td>{a.Id}</td><td>{a.FirstName}</td><td>{a.LastName}</td><td>{a.MiddleName}</td><td>{a.HumanCount}</td><td>{a.Square}</td></tr>";
                }
                html += "</table>";
                string newCookieFirstName = context.Request.Query["searchfirstname"];
                if (newCookieFirstName != null)
                    context.Response.Cookies.Append("firstname", newCookieFirstName);

                string newCookieLastName = context.Request.Query["searchlastname"];
                if (newCookieLastName != null)
                    context.Response.Cookies.Append("lastname", newCookieLastName);

                string newCookieMiddleName = context.Request.Query["searchmiddlename"];
                if (newCookieLastName != null)
                    context.Response.Cookies.Append("middlename", newCookieMiddleName);

                context.Response.WriteAsync(html);
            }
            await _next(context);

        }
    }
}
