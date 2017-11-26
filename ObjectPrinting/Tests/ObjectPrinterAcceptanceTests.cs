using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class ObjectPrinterAcceptanceTests
    {
        [Test]
        public void Demo()
        {
            var person = new Person { Name = "Alex", Age = 19 };

            var printer = ObjectPrinter.For<Person>()
                //1. Исключить из сериализации свойства определенного типа
                .Excluding<Guid>()
                //2. Указать альтернативный способ сериализации для определенного типа
                .Printing<int>().Using(i => i.ToString())
                //3. Для числовых типов указать культуру
                .Printing<int>().Using(CultureInfo.CurrentCulture)
                //4. Настроить сериализацию конкретного свойства
                .Printing(p => p.Age).Using(age => age.ToString())
                //5. Настроить обрезание строковых свойств (метод должен быть виден только для строковых свойств)
                .Printing(p => p.Name).CutToLength(10)
                //6. Исключить из сериализации конкретного свойства
                .Excluding(p => p.Age);

            //string s1 = printer.PrintToString(person);

            //7. Синтаксический сахар в виде метода расширения, сериализующего по-умолчанию
            //string s2 = person.PrintToString();

            //8. ...с конфигурированием
            string s3 = person.PrintToString(s => s.Excluding(p => p.Age));
        }

        [Test]
        public void ExcludeType()
        {
            var person = new Person { Name = "Alex", Age = 19 };

            var printer = ObjectPrinter.For<Person>()
                .Excluding<Guid>()
                .Excluding<string>()
                .PrintToString(person);
            printer.Should().Be(string.Join(Environment.NewLine, 
                "Person", "\tHeight = 0", "\tAge = 19") 
                + Environment.NewLine);
        }

        [Test]
        public void SetSerializationForType()
        {
            var person = new Person { Name = "Alex", Age = 19, Height = 180 };

            var printer = ObjectPrinter.For<Person>()
                .Printing<double>().Using(i => i + "sm")
                .PrintToString(person);
            printer.Should().Be(string.Join(Environment.NewLine, 
                "Person", "\tId = Guid", "\tName = Alex", "\tHeight = 180sm", "\tAge = 19") 
                + Environment.NewLine);
        }
        [Test]
        public void SetCultureForNumericalType()
        {
            var person = new Person { Name = "Alex", Age = 19, Height = 180 };

            var printer = ObjectPrinter.For<Person>()
                .Excluding<Guid>()
                .Excluding<int>()
                .Printing<double>().Using(CultureInfo.CurrentCulture)
                .PrintToString(person);
            printer.Should().Be(string.Join(Environment.NewLine, 
                "Person", "\tName = Alex", "\tHeight = 180") 
                + Environment.NewLine);
        }

        [Test]
        public void SetSerializationForProperty()
        {
            var person = new Person { Name = "Alex", Age = 19, Height = 180 };

            var printer = ObjectPrinter.For<Person>()
                .Excluding<Guid>()
                .Printing(p => p.Age).Using(age => "None")
                .PrintToString(person);
            printer.Should().Be(string.Join(Environment.NewLine, 
                "Person", "\tName = Alex", "\tHeight = 180", "\tAge = None") 
                + Environment.NewLine);
        }

        [Test]
        public void CutStringToLength()
        {
            var person = new Person { Name = "Alex", Age = 19, Height = 180 };

            var printer = ObjectPrinter.For<Person>()
                .Excluding<Guid>()
                .Printing(p => p.Name).CutToLength(1)
                .PrintToString(person);

            printer.Should().Be(string.Join(Environment.NewLine, 
                "Person", "\tName = A", "\tHeight = 180", "\tAge = 19") 
                + Environment.NewLine);
        }

        [Test]
        public void ExcludeProperty()
        {
            var person = new Person { Name = "Alex", Age = 19 };

            var printer = ObjectPrinter.For<Person>()
                .Excluding(p => p.Age)
                .Excluding(p => p.Id)
                .PrintToString(person);
            printer.Should().Be(string.Join(Environment.NewLine,
                "Person", "\tName = Alex", "\tHeight = 0")
                + Environment.NewLine);
        }
    }
}