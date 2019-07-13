using System.Collections.Generic;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace PDFLibrary
{
    public class Main
    {

        public IDictionary<string, PdfFormField> GetData(string file)
        {

            IDictionary<string, PdfFormField> fields;

            using (var reader = new PdfReader(file))
            {
                using (var doc = new PdfDocument(reader))
                {
                    fields = PdfAcroForm.GetAcroForm(doc,false).GetFormFields();
                }
            }

            return fields;
        }
    }
}
