using System;
using System.IO;
using System.Reflection;
using ImmutableClassLibrary.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace PDFLibraryTests
{
    [TestClass]
    public class Main
    {
        [TestMethod]
        public void CanGetFields()
        {

         var stream = new MemoryStream(File.ReadAllBytes("Pdfs\\Test.pdf"));

         var json = PDFLibrary.Main.GetData(stream);

         var expected = json.IndexOf("\"Active\":\"Yes\"",StringComparison.Ordinal) > 0;

         var cust = ImmutableClass.Create<Customer>(json, new JsonConverter[] {new BoolConverter()});
 
         Assert.AreEqual(expected,cust.Active);

        }
    }


    public class Customer : ImmutableClass
    {
        private string _firstName;

        public string FirstName
        {
            get => _firstName;
            set => Setter(
                MethodBase
                    .GetCurrentMethod()
                    .Name
                    .Substring(4),
                value,
                ref _firstName);
        }

        private string _middleInitial;

        public string MiddleInitial
        {
            get => _middleInitial;
            set => Setter(
                MethodBase
                    .GetCurrentMethod()
                    .Name
                    .Substring(4),
                value,
                ref _middleInitial);
        }


        private string _lastName;

        public string LastName
        {
            get => _lastName;
            set => Setter(
                MethodBase
                    .GetCurrentMethod()
                    .Name
                    .Substring(4),
                value,
                ref _lastName);
        }

        private string _street;

        public string Street
        {
            get => _street;
            set => Setter(
                MethodBase
                    .GetCurrentMethod()
                    .Name
                    .Substring(4),
                value,
                ref _street);
        }

        private string _state;

        public string State
        {
            get => _state;
            set => Setter(
                MethodBase
                    .GetCurrentMethod()
                    .Name
                    .Substring(4),
                value,
                ref _state);
        }

        private string _zip;

        public string Zip
        {
            get => _zip;
            set => Setter(
                MethodBase
                    .GetCurrentMethod()
                    .Name
                    .Substring(4),
                value,
                ref _zip);
        }


        private DateTime _customerSince;

        public DateTime CustomerSince
        {
            get => _customerSince;
            set => Setter(
                MethodBase
                    .GetCurrentMethod()
                    .Name
                    .Substring(4),
                value,
                ref _customerSince);
        }

        private bool _active;

        public bool Active
        {
            get => _active;
            set => Setter(
                MethodBase
                    .GetCurrentMethod()
                    .Name
                    .Substring(4),
                value,
                ref _active);
        }

        private Int32 _pointBalance;

        public Int32 PointBalance
        {
            get => _pointBalance;
            set => Setter(
                MethodBase
                    .GetCurrentMethod()
                    .Name
                    .Substring(4),
                value,
                ref _pointBalance);
        }



    }


    public class BoolConverter : JsonConverter
    {
        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return  reader.Value.ToString().ToLower().Trim() == "yes";



        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Boolean));
        }
    }


}
