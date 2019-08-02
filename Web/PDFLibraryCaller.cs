using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PDFLibrary;
using Web.Models;

namespace Web
{
    public class PDFLibraryCaller
    {


        public PDFLibraryCaller()
        {

        }

        public byte[] GetPDF(PDFData pdfData, string pdfPath)
        {
            var _data = new PdfFields()
            {
                new PdfField("FirstName",pdfData.FirstName),
                new PdfField("MiddleInitial",pdfData.MiddleInitial),
                new PdfField("LastName",pdfData.LastName),
                new PdfField("Street",pdfData.Street),
                new PdfField("City",pdfData.City),
                new PdfField("State",pdfData.State),
                new PdfField("Zip","19301"),
                new PdfField("Active",pdfData.Active ? "Yes": "No"),
                new PdfField("CustomerSince",pdfData.CustomerSince,pdfData.GetFormatedValue("CustomerSince")),
                new PdfField("PointBalance",pdfData.PointBalance,pdfData.GetFormatedValue("PointBalance")),
                new PdfField("TIN",pdfData.TIN,pdfData.GetFormatedValue("TIN"))

            };

            return  Main.SetData(_data.ToArray(), System.IO.File.ReadAllBytes(pdfPath));

        }




    }
}