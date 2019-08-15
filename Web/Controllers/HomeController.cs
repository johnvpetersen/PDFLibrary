using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private PDFLibraryCaller _pdfCaller;

        public HomeController(PDFLibraryCaller pdfCaller)
        {
            _pdfCaller = pdfCaller;
        }

        [HttpPost]
        public ActionResult UploadPDF(HttpPostedFileBase file)
        {

            var pdf = _pdfCaller.GetFileBytes(file);
            if (pdf == null)
                return View();


            return  RedirectToAction("DisplayPDFData", _pdfCaller.GetData(pdf[0]));

        }

        public ActionResult UploadPDF()
        {
            return View();
        }

        public ActionResult DisplayPDFData(PDFData pdfData)
        {
            if (pdfData == null)
                return RedirectToAction("CreatePDF");

            return View("CreatePDF", pdfData);
        }

        public ActionResult CreatePDF()
        {

            return  View(new PDFData());
        }

        [HttpPost]
        public ActionResult CreatePDF(PDFData pdfData, HttpPostedFileBase file)
        {

            var pdf = _pdfCaller.GetFileBytes(file);
            if (pdf == null)
                return View(pdfData);



            return new FileContentResult(_pdfCaller.GetPDF(pdfData, pdf[0]), "application/pdf");
        }

    }
}