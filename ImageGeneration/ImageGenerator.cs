using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RazorEngine;

namespace ImageGeneration
{
    public class ImageGenerator
    {
        public static void CreateTextImage(string templateFileName, string _imgOutputFileName, dynamic displayText)
        {
            var templatePath = "SampleTemplates/" + templateFileName + ".xaml";
            var templateText = System.IO.File.ReadAllText(templatePath);
            dynamic viewModel = new ExpandoObject();
            viewModel.Text = displayText;
            var inputXaml = Razor.Parse(templateText, viewModel, "Model");
            String _imageOutputPath = System.IO.Directory.CreateDirectory("Output").FullName + "/";
            String _fullFileName = _imageOutputPath + _imgOutputFileName;
            byte[] _pngBytes = new byte[] { };
            Thread _pngCreationThread =
                new Thread((ThreadStart)delegate() { _pngBytes = GenImageFromXaml(inputXaml.ToString(), _fullFileName); });
            _pngCreationThread.IsBackground = true;
            _pngCreationThread.SetApartmentState(ApartmentState.STA);
            _pngCreationThread.Start();
            _pngCreationThread.Join();
        }

        private static byte[] GenImageFromXaml(string xaml, string outputFileName)
        {
            FrameworkElement _element = XamlReader.Parse(xaml) as FrameworkElement;
            var _pngBytes = GetPngImage(_element);

            using (BinaryWriter _binWriter =
            new BinaryWriter(System.IO.File.Open(outputFileName, FileMode.Create)))
            {
                _binWriter.Write(_pngBytes);
            }
            return _pngBytes;
        }

        private static byte[] GetPngImage(FrameworkElement element)
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
    }
}
