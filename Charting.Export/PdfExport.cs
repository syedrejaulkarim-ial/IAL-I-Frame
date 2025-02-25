using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ImageTools;
using ImageTools.IO.Bmp;
using ImageTools.IO.Jpeg;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Charting.Pdf
{
    public class PdfExport
    {
        public void ExportToPdf(SaveFileDialog dlg1, UIElement oControl)
        {
            var image = oControl.ToImage();
            var mstream = new MemoryStream();
            var encoder = new JpegEncoder();
            encoder.Encode(image, mstream);
            mstream.Seek(0, SeekOrigin.Begin);
            var pdfImg = XImage.FromStream(mstream);
            var document = new PdfDocument();
            var page = document.AddPage();
            page.Height = pdfImg.PointHeight + 10 + 25;
            page.Width = pdfImg.PointWidth + 10;
            var gfx = XGraphics.FromPdfPage(page);
            var diff = Math.Abs(page.Width - Math.Min(pdfImg.PointWidth, pdfImg.PixelWidth)) / 2;
            gfx.DrawImage(pdfImg, diff, 25);
            document.Save(dlg1.OpenFile());
        }
    }
}
