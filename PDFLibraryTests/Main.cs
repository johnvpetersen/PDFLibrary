using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ImmutableClassLibrary.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PDFLibrary;

namespace PDFLibraryTests
{
    [TestClass]
    public class Main
    {



        [TestMethod]
        public void MissingFields()
        {
            var result = PDFLibrary.Main.MissingFields(new[] { "FirstName"}, File.ReadAllBytes("Pdfs\\Test.pdf"));

            Assert.AreEqual("[\"MiddleInitial\",\"LastName\",\"Street\",\"City\",\"Zip\",\"State\",\"CustomerSince\",\"PointBalance\",\"Active\",\"TIN\"]",JsonConvert.SerializeObject(result));

        }



        [TestMethod]
        public void ExtraFields()
        {
          var  result = PDFLibrary.Main.ExtraFields(new[] { "FirstName", "ExtraField" }, File.ReadAllBytes("Pdfs\\Test.pdf"));

          Assert.AreEqual("ExtraField",result[0]);
        }


        [TestMethod]

        public void CanValidatePDF()
        {
           var  result = PDFLibrary.Main.IsPDF(File.ReadAllBytes("Pdfs\\Test.pdf"));

           Assert.IsTrue(result);

           result = PDFLibrary.Main.IsPDF(File.ReadAllBytes("Pdfs\\NotAPDF.txt"));

           Assert.IsFalse(result);



        }

        [TestMethod]
        public void CanValidate()
        {
            bool? result;
            
            result =   PDFLibrary.Main.ValidateFields(new[] { "FirstNameX" }, File.ReadAllBytes("Pdfs\\Test.pdf"));

            Assert.AreEqual(false,result);

            result = PDFLibrary.Main.ValidateFields(new[] { "FirstName" }, File.ReadAllBytes("Pdfs\\Test.pdf"));

            Assert.AreEqual(true, result);

            result = PDFLibrary.Main.ValidateFields(new string[0] , File.ReadAllBytes("Pdfs\\Test.pdf"));

            Assert.IsNull(result);

            result = PDFLibrary.Main.ValidateFields(null, File.ReadAllBytes("Pdfs\\Test.pdf"));

            Assert.IsNull(result);

            result = PDFLibrary.Main.ValidateFields(new string[1], null);

            Assert.IsNull(result);

        }

        [TestMethod]
        public void CanGetFields()
        {
         var data = PDFLibrary.Main.GetData(File.ReadAllBytes("Pdfs\\Test.pdf"));

         Assert.IsNotNull(data);
        }

        [TestMethod]
        public void CanSetFields()
        {

            var data = new PdfFields()
            {
                new PdfField("FirstName","John"),
                new PdfField("MiddleInitial","V"),
                new PdfField("LastName","Petersen"),
                new PdfField("Street","269 Vincent Road"),
                new PdfField("City","Paoli"),
                new PdfField("State","PA"),
                new PdfField("Zip","19301"),
                new PdfField("Active","Yes"),
                new PdfField("CustomerSince","01/01/2000"),
                new PdfField("PointBalance","100000","100,000"),
                new PdfField("TIN","111111111","111-11-1111")

            };
            var newPDF = PDFLibrary.Main.SetData(data.ToArray(), File.ReadAllBytes("Pdfs\\Test.pdf"));

            File.WriteAllBytes($"c:\\temp\\{Guid.NewGuid()}.pdf",newPDF);


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
