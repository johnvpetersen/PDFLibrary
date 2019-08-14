using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDFLibrary;

namespace PDFLibraryTests
{
    [TestClass]
    public class Main
    {

        private const string _notAPDF = "Pdfs\\NotAPDF.txt";
        private const string _testPDF = "Pdfs\\Test.pdf";
        private static string _output = $"Output\\{Guid.NewGuid().ToString()}";
        private string _file = string.Empty;


        [TestInitialize]
        public void TestInit()
        {
            _file = $"{_output}\\{Guid.NewGuid()}.pdf";
        }


        [TestMethod]
        public void CanDetectMissingFields()
        {
            var result = PdfMethods.MissingFields(new[] { "FirstName"}, PdfMethods.Read(_testPDF));

            Assert.AreEqual("[\"MiddleInitial\",\"LastName\",\"Street\",\"City\",\"Zip\",\"State\",\"CustomerSince\",\"PointBalance\",\"Active\",\"TIN\"]",PdfMethods.ToJson(result));
        }



        [TestMethod]
        public void CanDetectExtraFields()
        {
          var  result = PdfMethods.ExtraFields(new[] { "FirstName", "ExtraField" }, PdfMethods.Read(_testPDF));

          Assert.AreEqual("ExtraField",result[0]);
        }


        [TestMethod]

        public void CanValidateIsAPDF()
        {
           var  result = PdfMethods.IsPDF(PdfMethods.Read(_testPDF));

           Assert.IsTrue(result);

           result = PdfMethods.IsPDF(PdfMethods.Read(_notAPDF));

           Assert.IsFalse(result);



        }

        [TestMethod]
        public void CanValidateFields()
        {
            bool? result;
            
            result =   PdfMethods.ValidateFields(new[] { "FirstNameX" }, PdfMethods.Read(_testPDF));

            Assert.AreEqual(false,result);

            result = PdfMethods.ValidateFields(new[] { "FirstName" }, PdfMethods.Read(_testPDF));

            Assert.AreEqual(true, result);

            result = PdfMethods.ValidateFields(new string[0] , PdfMethods.Read(_testPDF));

            Assert.IsNull(result);

            result = PdfMethods.ValidateFields(null, PdfMethods.Read(_testPDF));

            Assert.IsNull(result);

            result = PdfMethods.ValidateFields(new string[1], null);

            Assert.IsNull(result);

        }

        [TestMethod]
        public void CanGetFields()
        {
         var data = PdfMethods.GetData(PdfMethods.Read(_testPDF));

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
            var newPDF = PdfMethods.SetData(data.ToArray(), PdfMethods.Read(_testPDF));


          var bytesWritten =   PdfMethods.Write(_file, newPDF);



             Assert.AreEqual(newPDF.Length, bytesWritten.Length);

        }

    }

}
