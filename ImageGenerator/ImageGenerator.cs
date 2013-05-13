using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RazorEngine;

namespace ImageGenerator
{
    public class ImageGenerator
    {
        private byte[] GenImageFileFromXaml(string xaml, string outputFileName)
        {
            var pngBytes = GenImageFromXaml(xaml);

            using (BinaryWriter binWriter = new BinaryWriter(File.Open(outputFileName, FileMode.Create)))
            {
                binWriter.Write(pngBytes);
            }
            return pngBytes;
        }

        private byte[] GenImageFromXaml(string xaml)
        {
            FrameworkElement element = XamlReader.Parse(xaml) as FrameworkElement;
            var pngBytes = GetPngImage(element);
            return pngBytes;
        }

        private byte[] GetPngImage(FrameworkElement element)
        {
            var size = new Size(double.PositiveInfinity, double.PositiveInfinity);
            element.Measure(size);
            element.Arrange(new Rect(element.DesiredSize));
            var renderTarget =
                new RenderTargetBitmap((int) element.RenderSize.Width,
                                       (int) element.RenderSize.Height,
                                       96, 96,
                                       PixelFormats.Pbgra32);
            var sourceBrush = new VisualBrush(element);
            var drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(
                    sourceBrush, null, new Rect(
                                           new Point(0, 0),
                                           new Point(element.RenderSize.Width,element.RenderSize.Height)));
            }
            renderTarget.Render(drawingVisual);
            var pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
            using (var outputStream = new MemoryStream())
            {
                pngEncoder.Save(outputStream);
                return outputStream.ToArray();
            }
        }

        public byte[] GenerateImage(string xamlString, object viewModel)
        {
            var parsedXaml = Razor.Parse(xamlString, viewModel, "Model");
            return GenImageFromXaml(parsedXaml);
        }

        public void GenerateImageFile(string outputFilePath, string xamlString, object viewModel)
        {
            var parsedXaml = Razor.Parse(xamlString, viewModel, "Model");
            GenImageFileFromXaml(parsedXaml, outputFilePath);
        }
    }
}