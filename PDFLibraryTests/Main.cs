using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var result = Directory.CreateDirectory(_output);
            if (!result.Exists)
                throw new DirectoryNotFoundException("Error creating test output directory.");

            if (!File.Exists(_notAPDF))
                throw new FileNotFoundException("File pdfs\\notapdf.txt does not exist.");

            if (!File.Exists(_testPDF))
                throw new FileNotFoundException("File pdfs\\test.pdf does not exist.");

        }

        [TestInitialize]
        public void TestInit()
        {
            _file = $"{_output}\\{Guid.NewGuid()}.pdf";
        }


        [TestMethod]
        public void MissingFields()
        {
            var result = PdfMethods.MissingFields(new[] { "FirstName"}, File.ReadAllBytes(_testPDF));

            Assert.AreEqual("[\"MiddleInitial\",\"LastName\",\"Street\",\"City\",\"Zip\",\"State\",\"CustomerSince\",\"PointBalance\",\"Active\",\"TIN\"]",JsonConvert.SerializeObject(result));
        }



        [TestMethod]
        public void ExtraFields()
        {
          var  result = PdfMethods.ExtraFields(new[] { "FirstName", "ExtraField" }, File.ReadAllBytes(_testPDF));

          Assert.AreEqual("ExtraField",result[0]);
        }


        [TestMethod]

        public void CanValidateIsAPDF()
        {
           var  result = PdfMethods.IsPDF(File.ReadAllBytes(_testPDF));

           Assert.IsTrue(result);

           result = PdfMethods.IsPDF(File.ReadAllBytes(_notAPDF));

           Assert.IsFalse(result);



        }

        [TestMethod]
        public void CanValidate()
        {
            bool? result;
            
            result =   PdfMethods.ValidateFields(new[] { "FirstNameX" }, File.ReadAllBytes(_testPDF));

            Assert.AreEqual(false,result);

            result = PdfMethods.ValidateFields(new[] { "FirstName" }, File.ReadAllBytes(_testPDF));

            Assert.AreEqual(true, result);

            result = PdfMethods.ValidateFields(new string[0] , File.ReadAllBytes(_testPDF));

            Assert.IsNull(result);

            result = PdfMethods.ValidateFields(null, File.ReadAllBytes(_testPDF));

            Assert.IsNull(result);

            result = PdfMethods.ValidateFields(new string[1], null);

            Assert.IsNull(result);

        }

        [TestMethod]
        public void CanGetFields()
        {
         var data = PdfMethods.GetData(File.ReadAllBytes(_testPDF));

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
            var newPDF = PdfMethods.SetData(data.ToArray(), File.ReadAllBytes(_testPDF));

            File.WriteAllBytes(_file,newPDF);

             File.ReadAllBytes(_file);

             Assert.AreEqual(newPDF.Length, File.ReadAllBytes(_file).Length);

        }

    }

}
