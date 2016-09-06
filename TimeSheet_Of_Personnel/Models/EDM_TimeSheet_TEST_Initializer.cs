namespace TimeSheet_Of_Personnel.Models
{
    using System;
    using System.Data.Entity;
    internal class EDM_TimeSheet_TEST_Initializer : DropCreateDatabaseAlways<EDM_TimeSheet>
    {
        protected override void Seed(EDM_TimeSheet context)
        {
            context.DayTypes.Add(new DayType { DayTypeID = 8, SymbolName = "8", SymbolNameLatin = "8", DayTypeName = "8 годин", WorkHours = 8 });
            context.DayTypes.Add(new DayType { DayTypeID = 7, SymbolName = "7", SymbolNameLatin = "7", DayTypeName = "7 годин", WorkHours = 7 });
            context.DayTypes.Add(new DayType { DayTypeID = 6, SymbolName = "6", SymbolNameLatin = "6", DayTypeName = "6 годин", WorkHours = 6 });
            context.DayTypes.Add(new DayType { DayTypeID = 4, SymbolName = "4", SymbolNameLatin = "4", DayTypeName = "4 години", WorkHours = 4 });
            context.DayTypes.Add(new DayType { DayTypeID = 25, SymbolName = "вд", SymbolNameLatin = "vd", DayTypeName = "Відрядження", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 26, SymbolName = "в", SymbolNameLatin = "vp", DayTypeName = "Bідпустка (Щорічна основна та додаткова)", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 27, SymbolName = "ч", SymbolNameLatin = "ch", DayTypeName = "Чорнобильськa відпустка", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 28, SymbolName = "н", SymbolNameLatin = "nv", DayTypeName = "Навчання", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 29, SymbolName = "нб", SymbolNameLatin = "nb", DayTypeName = "Навчання Без збереження заробітної плати", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 30, SymbolName = "бз", SymbolNameLatin = "bz", DayTypeName = "Відпустка Без Збереження заробітної плати", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 31, SymbolName = "зс", SymbolNameLatin = "zs", DayTypeName = "за Згодою Сторін (відпустка без збер. ЗП)", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 32, SymbolName = "дд", SymbolNameLatin = "dd", DayTypeName = "на Дітей Додаткова оплачувана відпустка", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 33, SymbolName = "вп", SymbolNameLatin = "vp", DayTypeName = "у зв. із Вагітністю та Пологами відпустка", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 34, SymbolName = "дв", SymbolNameLatin = "dv", DayTypeName = "День Відпочинку", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 35, SymbolName = "с", SymbolNameLatin = "sm", DayTypeName = "Семінар, конференція, підвищення кваліф.", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 36, SymbolName = "тн", SymbolNameLatin = "tn", DayTypeName = "Тимчасова Непрацездатність (оплачувана)", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 37, SymbolName = "нн", SymbolNameLatin = "nn", DayTypeName = "Неоплачувана Непрацездатність", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 38, SymbolName = "ін", SymbolNameLatin = "in", DayTypeName = "Інший невідпрацьований час, передбачений з-ном", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 39, SymbolName = "нз", SymbolNameLatin = "nz", DayTypeName = "НеЗ’ясовані причини", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 40, SymbolName = "п", SymbolNameLatin = "pr", DayTypeName = "Прогул", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 41, SymbolName = "по", SymbolNameLatin = "po", DayTypeName = "Відпустка по вагітності та Пологам", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 42, SymbolName = "до", SymbolNameLatin = "do", DayTypeName = "Відпустка для Догляду за дитиною до 3-го віку", WorkHours = 0 });
            context.DayTypes.Add(new DayType { DayTypeID = 0, SymbolName = "-", SymbolNameLatin = "xx", DayTypeName = "Вже не (ще не) працює у цей день в ДІУ", WorkHours = 0 });

            DateTime workStartDef = new DateTime(2016, 01, 01);

            context.Employees.Add(new Employee { EmployeeName = "Іванов І.І.", IsAWoman = false, EmployeeID = 101, EmployPosition = "Економіст", WorkStart = workStartDef });
            context.Employees.Add(new Employee { EmployeeName = "Сідорова С.С.", IsAWoman = true, EmployeeID = 102, EmployPosition = "Головний економіст", WorkStart = workStartDef });
            context.Employees.Add(new Employee { EmployeeName = "Петров П.П.", IsAWoman = false, EmployeeID = 103, EmployPosition = "Начальник  відділу", WorkStart = workStartDef, Comment = "Прийнято 15.07.2016" });
            context.Employees.Add(new Employee { EmployeeName = "Гривневкая Ю.А.", IsAWoman = true, EmployeeID = 104, EmployPosition = "Бухгалтер", WorkStart = workStartDef });

          
            context.SaveChanges();

            context.CalendRecords.Add(new CalendRecord { EmployeeID=104, DayTypeID=28, CalendRecordName = new DateTime(2016,7,6) });
            context.CalendRecords.Add(new CalendRecord { EmployeeID=103, DayTypeID=25, CalendRecordName = new DateTime(2016,7,15) });
            context.CalendRecords.Add(new CalendRecord { EmployeeID=102, DayTypeID=35, CalendRecordName = new DateTime(2016,7,18) });

            context.SaveChanges();

            base.Seed(context);
        }
        
    }
}