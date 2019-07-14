using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
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
    }
}
