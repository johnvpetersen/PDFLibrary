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
        public static ImmutableArray<T> FromJSON<T>(ImmutableArray<string> json)
        {

            return  ImmutableArray.Create<T>(JsonConvert.DeserializeObject<T>(json[0]));
        }

        public static ImmutableArray<string> ToJson<T>(ImmutableArray<T> value)
        {
            return ImmutableArray.Create<string>(JsonConvert.SerializeObject(value));
        }

        public static ImmutableArray<byte[]> Write(ImmutableArray<string> path, ImmutableArray<byte[]> bytes)
        {
            File.WriteAllBytes(path[0], bytes[0]);

            return Read(path);
        }

        public static ImmutableArray<byte[]> Read(ImmutableArray<string> path)
        {
            return  ImmutableArray.Create<byte[]>(File.ReadAllBytes(path[0]));
        }

        public static ImmutableArray<bool> IsPDF(ImmutableArray<byte[]> file)
        {

                using (var ms = new MemoryStream(file[0]))
                {
                    using (var reader = new PdfReader(ms))
                    {
                        return ImmutableArray.Create<bool>(true);
                    }
                }

        }

        public static ImmutableArray<string>  MissingFields(ImmutableArray<string> fields, ImmutableArray<byte[]>  pdf)
        {

            return getFormFields(pdf).Keys.ToArray().Except(fields).ToImmutableArray();
        }

        public static ImmutableArray<string> ExtraFields(ImmutableArray<string> fields, ImmutableArray<byte[]> pdf)
        {
            return  fields.Except(getFormFields(pdf).Keys).ToImmutableArray();
        }

        public static ImmutableArray<bool> ValidateFields(ImmutableArray<string> fields, ImmutableArray<byte[]> pdf)
        {

            return  ImmutableArray.Create<bool>(!fields.Except(getFormFields(pdf).Keys.ToArray()).Any());
        }

        public static ImmutableDictionary<string,string>  GetData(ImmutableArray<byte[]> pdf)
        {


            return getFormFields(pdf).ToDictionary(x => x.Key, x => x.Value.GetValueAsString()).ToImmutableDictionary();
        }

        public static ImmutableArray<byte[]> SetData(ImmutableArray<PdfField>  fields, ImmutableArray<byte[]> pdf)
        {



            using (var ms = new MemoryStream())
            {
                using (var doc = new PdfDocument(new PdfReader(new MemoryStream(pdf[0])), new PdfWriter(ms)))

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
                    return  ImmutableArray.Create<byte[]>(ms.ToArray());
                }
            }
        }

        static ImmutableDictionary<string, PdfFormField> getFormFields(ImmutableArray<byte[]> pdf)
        {
            using (var ms = new MemoryStream(pdf[0]))
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