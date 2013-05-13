using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var _pngBytes = GenImageFromXaml(xaml);

            using (BinaryWriter _binWriter =
            new BinaryWriter(System.IO.File.Open(outputFileName, FileMode.Create)))
            {
                _binWriter.Write(_pngBytes);
            }
            return _pngBytes;
        }

        private byte[] GenImageFromXaml(string xaml)
        {
            FrameworkElement _element = XamlReader.Parse(xaml) as FrameworkElement;
            var _pngBytes = GetPngImage(_element);
            return _pngBytes;
        }

        private byte[] GetPngImage(FrameworkElement element)
        {
            var _size = new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity);
            element.Measure(_size);
            element.Arrange(new Rect(element.DesiredSize));
            var _renderTarget =
              new RenderTargetBitmap((int)element.RenderSize.Width,
                                     (int)element.RenderSize.Height,
                                     96, 96,
                                     PixelFormats.Pbgra32);
            var _sourceBrush = new VisualBrush(element);
            var _drawingVisual = new DrawingVisual();
            using (DrawingContext _drawingContext = _drawingVisual.RenderOpen())
            {
                _drawingContext.DrawRectangle(
                    _sourceBrush, null, new System.Windows.Rect(
                                           new System.Windows.Point(0, 0),
                                           new System.Windows.Point(element.RenderSize.Width,
                                           element.RenderSize.Height)));
            }
            _renderTarget.Render(_drawingVisual);
            var _pngEncoder = new PngBitmapEncoder();
            _pngEncoder.Frames.Add(BitmapFrame.Create(_renderTarget));
            using (var _outputStream = new MemoryStream())
            {
                _pngEncoder.Save(_outputStream);
                return _outputStream.ToArray();
            }
        }

        public byte[] GenerateImage(string xamlString, object viewModel)
        {
            var parsedXaml = Razor.Parse(xamlString, viewModel,"Model");
            return GenImageFromXaml(parsedXaml);
        }

        public void GenerateImageFile(string outputFilePath, string xamlString, object viewModel)
        {
            var parsedXaml = Razor.Parse(xamlString, viewModel,"Model");
            GenImageFileFromXaml(parsedXaml, outputFilePath);
        }
    }
}
