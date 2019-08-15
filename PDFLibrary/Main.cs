using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Newtonsoft.Json;

namespace PDFLibrary
{
    public class PdfMethods

    {
        public static T FromJSON<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static byte[] Write(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);

            return Read(path);
        }

        public static byte[] Read(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static bool IsPDF(byte[] file)
        {
            if (file == null || file.Length == 0)
                return false;

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
            catch (iText.IO.IOException)
            {
                return false;
            }
        }

        public static string[] MissingFields(ImmutableArray<string> fields, byte[] pdf)
        {
            return !validParameters(fields, pdf) ? null : getFormFields(pdf).Keys.ToArray().Except(fields).ToArray();
        }

        public static string[] ExtraFields(ImmutableArray<string> fields, byte[] pdf)
        {
            return !validParameters(fields, pdf) ? null : fields.Except(getFormFields(pdf).Keys).ToArray();
        }


        public static bool? ValidateFields(ImmutableArray<string> fields, byte[] pdf)
        {
            if (!validParameters(fields, pdf))
                return null;

            return !fields.Except(getFormFields(pdf).Keys.ToArray()).Any();
        }


        public static Dictionary<string, string> GetData(byte[] pdf)
        {
            if (!IsPDF(pdf))
                return null;


            return getFormFields(pdf).ToDictionary(x => x.Key, x => x.Value.GetValueAsString());
        }


        public static byte[] SetData(ImmutableArray<PdfField>  fields, byte[] pdf)
        {
            if (!IsPDF(pdf))
                return null;

            using (var ms = new MemoryStream())
            {
                using (var doc = new PdfDocument(new PdfReader(new MemoryStream(pdf)), new PdfWriter(ms)))

                {
                    var form = PdfAcroForm.GetAcroForm(doc, true);
                    var formFields = form.GetFormFields();
                    foreach (var field in fields.Where(x => !string.IsNullOrEmpty(x.Value)))
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

        static ImmutableDictionary<string, PdfFormField> getFormFields(byte[] pdf)
        {
            using (var ms = new MemoryStream(pdf))
            {
                using (var reader = new PdfReader(ms))
                {
                    using (var doc = new PdfDocument(reader))
                    {
                        var builder = ImmutableDictionary.CreateBuilder<string, PdfFormField>();

                        PdfAcroForm.GetAcroForm(doc, false).GetFormFields().ToList().ForEach(x => builder.Add(x.Key, x.Value));

                        return builder.ToImmutable();
                    }
                }
            }
        }



        static bool validParameters(ImmutableArray<string> fields, byte[] pdf)
        {
            if (!IsPDF(pdf))
                return false;

            return !((fields == null || fields.Length == 0));
        }

        public static ImmutableList<PdfField> GetFields(List<PdfField> fields)
        {
            return ImmutableList.Create(fields.ToArray());
        }


    }




    public class PdfField
    {
        public PdfField(string name, string value, string displayValue = null)
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