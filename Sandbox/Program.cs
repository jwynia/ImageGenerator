using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageGeneration;
using RazorEngine;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            string xamlString = System.IO.File.ReadAllText("SampleTemplates/BlueButton.xaml");
            string imgOutputFileName = "BlueButton.png";

            dynamic viewModel = new ExpandoObject();
            viewModel.Text = "Add to Cart";
                       
            String imageOutputPath = System.IO.Directory.CreateDirectory("Output").FullName;
            String fullFileName = Path.Combine(imageOutputPath,imgOutputFileName);
            byte[] image = ImageGenerator.GenerateImage(xamlString, viewModel);
            byte[] imageFile = ImageGenerator.GenerateImageFile(xamlString, viewModel, fullFileName);
        }
    }
}
