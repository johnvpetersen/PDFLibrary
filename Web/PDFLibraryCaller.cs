using System.IO;
using System.Web;
using Newtonsoft.Json;
using PDFLibrary;
using Web.Models;

namespace Web
{
    public class PDFLibraryCaller
    {
        public PDFData GetData(HttpPostedFileBase file)
        {
            int fileSizeInBytes = file.ContentLength;
            byte[] pdf = null;
            using (var br = new BinaryReader(file.InputStream))
            {
                pdf = br.ReadBytes(fileSizeInBytes);
            }

            if (!PdfMethods.IsPDF(pdf))
                return new PDFData();


            var data = PdfMethods.GetData(pdf);

            data["Active"] = data["Active"] == "Yes" ? "true" : "false";

          return  JsonConvert.DeserializeObject<PDFData>(JsonConvert.SerializeObject(data));

        }

        public byte[] GetPDF(PDFData pdfData, string pdfPath)
        {
            var file = System.IO.File.ReadAllBytes(pdfPath);

            if (!PdfMethods.IsPDF(file))
                return null;


            var _data = new PdfFields()
            {
                new PdfField("FirstName",pdfData.FirstName),
                new PdfField("MiddleInitial",pdfData.MiddleInitial),
                new PdfField("LastName",pdfData.LastName),
                new PdfField("Street",pdfData.Street),
                new PdfField("City",pdfData.City),
                new PdfField("State",pdfData.State),
                new PdfField("Zip",pdfData.Zip),

                new PdfField("Active",pdfData.Active ? "Yes": "No"),
                new PdfField("CustomerSince",pdfData.CustomerSince.Replace("/",""),pdfData.CustomerSince),
                new PdfField("PointBalance",pdfData.PointBalance.Replace(",",""),pdfData.PointBalance),
                new PdfField("TIN",pdfData.TIN.Replace("-",""),pdfData.TIN)

            };

            return  PdfMethods.SetData(_data.ToArray(), file);

        }
    }
}