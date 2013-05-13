using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            string xamlString = System.IO.File.ReadAllText("SampleTemplates/BlueButton.xaml");
            string outputFilePath = "BlueButton.png";

            dynamic viewModel = new ExpandoObject();
            viewModel.Text = "Add to Cart";
            ImageGenerator.CreateTextImage("BlueButton","BlueButton.png","Add to Cart");
        }
    }
}
