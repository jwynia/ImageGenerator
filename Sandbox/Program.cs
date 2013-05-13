using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageGenerator;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new ImageGenerator.ImageGenerator();
            dynamic viewModel = new ExpandoObject();
            string xamlString = "";
            byte[] image = generator.GenerateImage(xamlString, viewModel);
            string outputFilePath = "";
            generator.GenerateImageFile(outputFilePath, xamlString, viewModel);
        }
    }
}
