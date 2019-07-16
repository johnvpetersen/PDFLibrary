using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Forms;
using iText.Kernel.Pdf;

namespace PDFLibrary
{
    public class Main
    {
        public static bool IsPDF(byte[] file)
        {
            try
            {
                using (var ms = new MemoryStream(file))
                {
                    using (var reader = new PdfReader(ms))
                    {
                        return true;
                    }
                }
            }
            catch (iText.IO.IOException e)
            {
                return false;

            }
        }


            public static bool? Validate(string[] fields, byte[] pdf)
        {

            if (fields == null || pdf == null || fields.Length == 0 || pdf.Length == 0  )
                return null;


            using (var ms = new MemoryStream(pdf))
            {
                using (var reader = new PdfReader(ms))
                {
                    using (var doc = new PdfDocument(reader))
                    {
                        return !fields.Except(PdfAcroForm.GetAcroForm(doc, false).GetFormFields().Keys.ToArray()).Any();
                    }
                }

            }
        }


        public static Dictionary<string,string> GetData(byte[] pdf)
        {

            using (var ms = new MemoryStream(pdf))
            {
                using (var reader = new PdfReader(ms))
                {
                    using (var doc = new PdfDocument(reader))
                    {
                        return PdfAcroForm.GetAcroForm(doc, false).GetFormFields().ToDictionary(x => x.Key, x => x.Value.GetValueAsString());
                    }
                }

            }


        }



        public static byte[] SetData(PdfField[] fields, byte[] pdf)
        {
            using (var ms = new MemoryStream())
            {
                using (var doc = new PdfDocument(new PdfReader(new MemoryStream(pdf)), new PdfWriter(ms))) 
                
                {
                    var form = PdfAcroForm.GetAcroForm(doc, true);
                    var formFields = form.GetFormFields();
                    foreach (var field in fields)
                    {
                        if (string.IsNullOrEmpty(field.DisplayValue))
                        {
                            formFields[field.Name].SetValue(field.Value);
                        }
                        else
                        {
                            formFields[field.Name].SetValue(field.Value, field.DisplayValue);
                        }
                    }
                    doc.Close();
                    return ms.ToArray();
                }
            }
        }
    }


    public class PdfFields : List<PdfField>
    {

    }


    public class PdfField
    {
        public PdfField(string name, string value,string displayValue = null)
        {
            DisplayValue = displayValue;
            Value = value;
            Name = name;
        }

        public string Name { get; }
        public string Value { get; }
        public string DisplayValue { get; }
    }


}
