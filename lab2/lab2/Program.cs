using ConsoleTables;
using lab2.Models;

using (UtilitiesContext context = new UtilitiesContext())
{
    bool isEnd = false;
    while (!isEnd)
    {
        Menu();
        int numOfMenu = InputIntValue("Введите номер меню(int): ");

        switch (numOfMenu)
        {
            //1.Выборка всех данных из таблицы, стоящей в схеме базы данных нас стороне отношения «один» – 1 шт.
            case 1:
                ListOutput(context.Apartments);
                break;
            //2.Выборку данных из таблицы, стоящей в схеме базы данных на стороне отношения «один», отфильтрованные по определенному условию, налагающему ограничения на одно или несколько полей – 1 шт.
            case 2:
                IEnumerable<Rate> rateList = context.Rates.Where(x => x.Price > 70);
                ListOutput(rateList);
                break;
            //3. Выборка данных, сгруппированных по любому из полей данных с выводом какого-либо итогового результата (min, max, avg, сount или др.) по выбранному полю из таблицы, стоящей в схеме базы данных нас стороне отношения «многие» – 1 шт.
            case 3:
                int count = context.Rates.Count(x => x.Price < 70);
                Console.WriteLine("Количество тарифов дешевле 70: " + count);
                break;
            //4. Выборку данных из двух полей двух таблиц, связанных между собой отношением «один-ко-многим» – 1 шт.
            case 4:
                var rates = context.Rates.Join(context.PaymentTypes,
                                               x => x.PaymentTypeId,
                                               y => y.Id,
                                               (x, y) => new
                                               {
                                                   Id = x.Id,
                                                   Name = y.Name,
                                                   Date = x.Date.ToShortDateString(),
                                                   Price = x.Price
                                               }).Take(50);
                ListOutput(rates);
                break;
            //5. Выборку данных из двух таблиц, связанных между собой отношением «один-ко-многим» и отфильтрованным по некоторому условию, налагающему ограничения на значения одного или нескольких полей – 1 шт.
            case 5:
                var date = new DateTime(2023, 08, 23);
                rates = context.Rates.Join(context.PaymentTypes,
                                               x => x.PaymentTypeId,
                                               y => y.Id,
                                               (x, y) => new
                                               {
                                                   Id = x.Id,
                                                   Name = y.Name,
                                                   Date = x.Date.ToShortDateString(),
                                                   Price = x.Price
                                               }).Where(x => x.Price == 50);
                ListOutput(rates);
                break;
            //6.	Вставку данных в таблицы, стоящей на стороне отношения «Один» – 1 шт.
            case 6:
                AddApartment();
                break;
            //7.	Вставку данных в таблицы, стоящей на стороне отношения «Многие» – 1 шт.: 
            case 7:
                AddRate();
                break;
            //8.	Удаление данных из таблицы, стоящей на стороне отношения «Один» – 1 шт.
            case 8:
                RemoveApartment();
                break;
            //9.	Удаление данных из таблицы, стоящей на стороне отношения «Многие» – 1 шт.
            case 9:
                RemoveRate();
                break;
            //10.	Обновление удовлетворяющих определенному условию записей в любой из таблиц базы данных – 1 шт.
            case 10:
                UpdateApartment();
                break;
            case 11:
                isEnd = true;
                break;
            default:
                Console.WriteLine("There is no such number in the menu");
                break;
        }
        Console.Write("Press any key to continue");
        Console.ReadKey();
        Console.Clear();
    }

    void Menu()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("1. Список квартир");
        Console.WriteLine("2. Список тарифов, которые дороже 70");
        Console.WriteLine("3. Количество тарифов, дешевле 70");
        Console.WriteLine("4. Список тарифов");
        Console.WriteLine("5. Список тарифов, цена которых равна 50");
        Console.WriteLine("6. Добавление квартиры");
        Console.WriteLine("7. Добавление тарифа");
        Console.WriteLine("8. Удаление квартиры");
        Console.WriteLine("9. Удаление тарифа");
        Console.WriteLine("10. Обновление кол-ва человек в квартире");
        Console.WriteLine("11. Exit");
    }

    void UpdateApartment()
    {
        int id = InputIntValue("Введите номер квартиры: ");
        Apartment? apartment = context.Apartments.FirstOrDefault(x => x.Id == id);
        if (apartment == null)
            Console.WriteLine("Квартиры с таким номером нет.");
        else
        {
            Console.WriteLine(apartment);
            int humanCount = InputIntValue("Введите новое кол-во людей: ");

            apartment.HumanCount = humanCount;
            context.SaveChanges();
            Console.WriteLine("Информация по квартире обновлена");

        }
    }

    void ListOutput<T>(IEnumerable<T> list) where T : class
    {
        string[] properties = typeof(T).GetProperties().Where(x => !x.PropertyType.IsGenericType).Select(x => x.Name).ToArray();
        ConsoleTable table = new ConsoleTable(properties);

        foreach (var item in list)
        {
            table.AddRow(item.GetType().GetProperties().Where(x => !x.PropertyType.IsGenericType).Select(x => x.GetValue(item)).ToArray());
        }
        table.Write();
    }

    void AddApartment()
    {
        Console.Clear();
        Console.WriteLine("Добавление квартиры");
        Console.Write("Введите имя: ");
        string firstname = Console.ReadLine() ?? string.Empty;
        Console.Write("Введите фамилию: ");
        string lastname = Console.ReadLine() ?? string.Empty;
        Console.Write("Введите отчество: ");
        string middlename = Console.ReadLine() ?? string.Empty;
        int humanCount = InputIntValue("Введите кол-во человек: ");
        double square = InputDoubleValue("Введите площадь квартиры: ");

        Apartment newApartment = new Apartment()
        {
            FirstName = firstname,
            LastName = lastname,
            MiddleName = middlename,
            HumanCount = humanCount,
            Square = square
        };

        context.Apartments.Add(newApartment);
        context.SaveChanges();
        Console.WriteLine($"Квартира под номером {newApartment.Id} добавлена.");
    }

    void RemoveApartment()
    {
        Console.WriteLine("Удаление квартиры");
        int id = InputIntValue("Введите номер квартиры для удаления: ");
        Apartment apartment;
        if (GetApartmentById(id, out apartment))
        {
            context.Apartments.Remove(apartment);
            Console.WriteLine(apartment.Id + " удалена");
            context.SaveChanges();
        }
        else
        {
            Console.WriteLine("Нет квартиры с таким номером");
        }
    }

    void RemoveRate()
    {
        Console.WriteLine("Удаление тарифа");
        int id = InputIntValue("Введите id тарифа для удаления: ");
        Rate? rate = context.Rates.FirstOrDefault(x => x.Id == id);
        if (rate == null)
            Console.WriteLine("Нет таких тарифов");
        else
        {
            context.Rates.Remove(rate);
            context.SaveChanges();
            Console.WriteLine("Тариф " + rate.Id + " удален");
        }
    }

    void AddRate()
    {
        Console.Clear();
        Console.WriteLine("Добваление тарифа");
        int price = InputIntValue("Введите цену тарифа: ");
        Console.Write("Введите название типа платежа(газ, вода): ");
        string typeName = Console.ReadLine() ?? string.Empty;
        PaymentType? type = context.PaymentTypes.FirstOrDefault(x => x.Name == typeName);
        if (type == null)
        {
            Console.WriteLine("Нет такого типа платежа");
        }
        else
        {
            Rate newRate = new Rate()
            {
                Price = price,
                PaymentTypeId = type.Id,
                Date = DateTime.Now
            };
            context.Rates.Add(newRate);
            context.SaveChanges();
            Console.WriteLine("Тариф добавлен.");
        }

    }

    bool GetApartmentById(int id, out Apartment? apartment)
    {
        apartment = context.Apartments.FirstOrDefault(x => x.Id == id);
        if (apartment == null)
            return false;
        else
            return true;
    }

    double InputDoubleValue(string message)
    {
        Console.Write(message);
        bool isEnd = false;
        double value = 0;
        while (!isEnd)
        {
            string str = Console.ReadLine() ?? string.Empty;
            isEnd = double.TryParse(str, out value);
            if (isEnd == false)
                Console.WriteLine("Нужно ввести число типа double. Попробуйте еще раз: ");
        }

        return value;
    }

    int InputIntValue(string message)
    {
        Console.Write(message);
        bool isEnd = false;
        int value = 0;
        while (!isEnd)
        {
            string str = Console.ReadLine() ?? string.Empty;
            isEnd = int.TryParse(str, out value);
            if (isEnd == false)
                Console.WriteLine("Нужно ввести число. Попробуйте еще раз: ");
        }

        return value;
    }
}