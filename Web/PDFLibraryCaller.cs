using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PDFLibrary;
using Web.Models;

namespace Web
{
    public class PDFLibraryCaller
    {
        public PDFData GetData(byte[] pdf)
        {


            var data = PdfMethods.GetData(ImmutableArray.Create<byte[]>(pdf)).ToDictionary(x => x.Key, x => x.Value);


            data["Active"] = data["Active"] == "Yes" ? "true" : "false";

          return  JsonConvert.DeserializeObject<PDFData>(JsonConvert.SerializeObject(data));

        }


        public ImmutableArray<byte[]> GetFileBytes(HttpPostedFileBase file)
        {

            int fileSizeInBytes = file.ContentLength;
            byte[] pdf = null;
            using (var br = new BinaryReader(file.InputStream))
            {
                pdf = br.ReadBytes(fileSizeInBytes);
            }


            return ImmutableArray.Create<byte[]>(pdf) ;

        }


        public byte[] GetPDF(PDFData pdfData, byte[] pdf)
        {

            var _data = new List<PdfField>()
            {
                new PdfField("FirstName",pdfData.FirstName),
                new PdfField("MiddleInitial",pdfData.MiddleInitial),
                new PdfField("LastName",pdfData.LastName),
                new PdfField("Street",pdfData.Street),
                new PdfField("City",pdfData.City),
                new PdfField("State",pdfData.State),
                new PdfField("Zip",pdfData.Zip),

                new PdfField("Active",pdfData.Active ? "Yes": null),
                new PdfField("CustomerSince", stripDelimter(pdfData.CustomerSince,"/"),pdfData.CustomerSince),
                new PdfField("PointBalance",stripDelimter(pdfData.PointBalance,","),pdfData.PointBalance),
                new PdfField("TIN", stripDelimter(pdfData.TIN,"-"),pdfData.TIN)

            };

            return  PdfMethods.SetData(  ImmutableArray.Create<PdfField>(_data.ToArray())    , ImmutableArray.Create<byte[]>(pdf))[0];

        }

        string stripDelimter(string value, string delimiter)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return value.Replace(delimiter,string.Empty);
        }



    }
}