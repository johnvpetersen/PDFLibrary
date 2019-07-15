using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.StyledXmlParser.Jsoup.Nodes;
using Newtonsoft.Json;

namespace PDFLibrary
{
    public class Main
    {

        public static string GetData(Stream file)
        {
            using (var reader = new PdfReader(file))
            {
                using (var doc = new PdfDocument(reader))
                {
                    return JsonConvert.SerializeObject(PdfAcroForm.GetAcroForm(doc,false).GetFormFields().ToDictionary(x => x.Key, x => x.Value.GetValueAsString()));
                }
            }

        }



        public static void Test(byte[] sourcePDF)
        {


            using (var ms = new MemoryStream(sourcePDF))
            {
                using (var reader = new PdfReader(ms))
                {
                    using (var doc = new PdfDocument(reader))
                    {
                        var x = PdfAcroForm.GetAcroForm(doc, false).GetFormFields();

                        foreach (var field in x)
                        {
                           var format =  field.Value.GetPdfObject().KeySet();
                        }
                    }
                }
            }

        }




        public static byte[] UpdatePDF(byte[] sourcePDF, byte[] targetPDF)
        {

            var fields = new Dictionary<string, string>();


            using (var ms = new MemoryStream(sourcePDF))
            {
                using (var reader = new PdfReader(ms))
                {
                    using (var doc = new PdfDocument(reader))
                    {
                      fields =   PdfAcroForm.GetAcroForm(doc, false).GetFormFields().ToDictionary(x => x.Key, x => x.Value.GetValueAsString());
                    }

                }

            }


            return UpdatePDF(fields, targetPDF);

        }
        public static byte[] UpdatePDF(Dictionary<string,string> fields, byte[] pdf)
        {

          byte[] newPDF; 

          using (var ms = new MemoryStream()) 
          {
              
              var doc = new PdfDocument(new PdfReader(new MemoryStream(pdf)), new PdfWriter(ms));
              var form = PdfAcroForm.GetAcroForm(doc, true);
              var formFields = form.GetFormFields();
              foreach (var field in fields)
              {
                  formFields[field.Key].SetValue(field.Value);

              }

              doc.Close();

              newPDF = ms.ToArray();


            }

            return newPDF;

        }
    }
}
