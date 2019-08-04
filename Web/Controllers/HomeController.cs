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

            if (file != null && file.ContentLength > 0)
            {
                return  RedirectToAction("DisplayPDFData", _pdfCaller.GetData(file));
            }

            return View();
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
          return new FileContentResult(_pdfCaller.GetPDF(pdfData, Server.MapPath("~/PDFs/Target.pdf")), "application/pdf");
        }

    }
}