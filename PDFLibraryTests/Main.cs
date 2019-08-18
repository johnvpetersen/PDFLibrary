﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using ImmutableClassLibrary;
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
        public static void ClassInitialize(TestContext context)
        {

            Directory.CreateDirectory(_output);
        }


        [TestInitialize]
        public void TestInit()
        {
            _file = $"{_output}\\{Guid.NewGuid()}.pdf";
        }







        [TestMethod]
        public void CanGetFields()

        {
            var expected =
                "[\"State\",\"City\",\"Active\",\"TIN\",\"Street\",\"Zip\",\"MiddleInitial\",\"LastName\",\"FirstName\",\"PointBalance\",\"CustomerSince\"]";

            var pdf = ImmutableArray.Create<byte>(File.ReadAllBytes(_testPDF));
            var fields = PdfMethods.Fields(pdf);

            Assert.AreEqual(expected, JsonConvert.SerializeObject(fields));



        }


        [TestMethod]
        public void CanDetectExtraFields()
        {


          var  result = PdfMethods.ExtraFields(ImmutableArray.Create<string>("FirstName", "ExtraField"), PdfMethods.Read(  new ImmutableString(_testPDF) ));

          Assert.AreEqual("ExtraField",result[0]);
        }


        [TestMethod]

        public void CanValidateIsAPDF()
        {
           var  result = PdfMethods.IsPDF(PdfMethods.Read(new ImmutableString(_testPDF)));

           Assert.IsTrue(result.Value);

        }

        [TestMethod]
        public void CanValidateFields()
        {
            bool result;
            
            result =   PdfMethods.ValidateFields(ImmutableArray.Create<string>("FirstNameX"), PdfMethods.Read(new ImmutableString(_testPDF))).Value;

            Assert.AreEqual(false,result);

            result = PdfMethods.ValidateFields(ImmutableArray.Create<string>("FirstName"), PdfMethods.Read(new ImmutableString(_testPDF))).Value;

            Assert.AreEqual(true, result);




        }

        [TestMethod]
        public void CanGetFields2()
        {
         var data = PdfMethods.GetData(PdfMethods.Read( new ImmutableString(_testPDF)));

         

         Assert.IsNotNull(data);
        }
        [TestMethod]
        [DataRow("Yes")]
        [DataRow(null)]
        public void CanSetFields(string active)
        {










            var data = new List<PdfField>()
            {
                new PdfField("FirstName","John"),
                new PdfField("MiddleInitial","V"),
                new PdfField("LastName","Petersen"),
                new PdfField("Street","269 Vincent Road"),
                new PdfField("City","Paoli"),
                new PdfField("State","PA"),
                new PdfField("Zip","19301"),
                new PdfField("Active",active),
                new PdfField("CustomerSince","01/01/2000"),
                new PdfField("PointBalance","100000","100,000"),
                new PdfField("TIN","111111111","111-11-1111")

            };




            var newPDF = PdfMethods.SetData( data.ToImmutableArray() , PdfMethods.Read(new ImmutableString(_testPDF)));


          var bytesWritten =   PdfMethods.Write( new ImmutableString(_file), newPDF);



             Assert.AreEqual(newPDF.ToArray().Length , bytesWritten.ToArray().Length);

        }

    }




  
}
