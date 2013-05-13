using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RazorEngine;

namespace ImageGeneration
{
    public class ImageGenerator
    {
        public static byte[] GenerateImage(string xamlTemplate, object viewModel)
        {
            var inputXaml = Razor.Parse(xamlTemplate, viewModel, "Model");
            byte[] pngBytes = new byte[] { };
            Thread pngCreationThread =
                new Thread(delegate() { pngBytes = GenImageFromXaml(inputXaml); });
            pngCreationThread.IsBackground = true;
            pngCreationThread.SetApartmentState(ApartmentState.STA);
            pngCreationThread.Start();
            pngCreationThread.Join();
            return pngBytes;
        }

        public static byte[] GenerateImageFile(string xamlTemplate, object viewModel, string outputFilePath)
        {
            var inputXaml = Razor.Parse(xamlTemplate, viewModel, "Model");
            byte[] pngBytes = new byte[] { };
            Thread pngCreationThread =
                new Thread(delegate() { pngBytes = GenImageFileFromXaml(inputXaml,outputFilePath); });
            pngCreationThread.IsBackground = true;
            pngCreationThread.SetApartmentState(ApartmentState.STA);
            pngCreationThread.Start();
            pngCreationThread.Join();
            return pngBytes;
        }
       
        private static byte[] GenImageFromXaml(string xaml)
        {
            FrameworkElement element = XamlReader.Parse(xaml) as FrameworkElement;
            var pngBytes = GetPngImage(element);
            return pngBytes;
        }
        private static byte[] GenImageFileFromXaml(string xaml, string outputFileName)
        {
            FrameworkElement element = XamlReader.Parse(xaml) as FrameworkElement;
            var pngBytes = GetPngImage(element);

            using (BinaryWriter binWriter =
            new BinaryWriter(File.Open(outputFileName, FileMode.Create)))
            {
                binWriter.Write(pngBytes);
            }
            return pngBytes;
        }

        private static byte[] GetPngImage(FrameworkElement element)
        {
            var size = new Size(double.PositiveInfinity, double.PositiveInfinity);
            element.Measure(size);
            element.Arrange(new Rect(element.DesiredSize));
            var renderTarget =
              new RenderTargetBitmap((int)element.RenderSize.Width,
                                     (int)element.RenderSize.Height,
                                     96, 96,
                                     PixelFormats.Pbgra32);
            var sourceBrush = new VisualBrush(element);
            var drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(
                    sourceBrush, null, new Rect(
                                           new Point(0, 0),
                                           new Point(element.RenderSize.Width,
                                           element.RenderSize.Height)));
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
    }
}
