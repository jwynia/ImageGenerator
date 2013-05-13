ImageGenerator
==============

ImageGenerator creates PNG images from XAML templates. Use it to put text onto buttons or any other purpose where you need to generate images based on runtime data.

I've used it mostly to generate buttons that more easily match mockups than fiddling with CSS, but theoretically, most XAML should work. Any fonts will need to be installed on the machine where this code runs in order for it to be properly used.

Usage
==============
After installing the package, you'll need to create XAML templates for the types of images you want to create. A basic button is included as a sample.

<pre>
&lt;UserControl xmlns=&quot;http://schemas.microsoft.com/winfx/2006/xaml/presentation&quot; xmlns:x=&quot;http://schemas.microsoft.com/winfx/2006/xaml&quot; xmlns:d=&quot;http://schemas.microsoft.com/expression/blend/2008&quot; xmlns:mc=&quot;http://schemas.openxmlformats.org/markup-compatibility/2006&quot; mc:Ignorable=&quot;d&quot;&gt;
&lt;Grid x:Name=&quot;LayoutRoot&quot; HorizontalAlignment=&quot;Left&quot; VerticalAlignment=&quot;Top&quot;&gt;
    &lt;Button HorizontalAlignment=&quot;Center&quot; BorderThickness=&quot;0&quot; VerticalAlignment=&quot;Center&quot; FontFamily=&quot;Calibri&quot; FontSize=&quot;13&quot; FontWeight=&quot;Bold&quot;&gt;
        &lt;Button.Background&gt;
            &lt;SolidColorBrush Color=&quot;#153E7E&quot;/&gt;
        &lt;/Button.Background&gt;
        &lt;Border Margin=&quot;2&quot; BorderBrush=&quot;#ffffff&quot; BorderThickness=&quot;1&quot; Padding=&quot;5&quot;&gt;
            &lt;TextBlock Text=&quot;@Model.Text.ToUpper()&quot; Foreground=&quot;#ffffff&quot; Padding=&quot;40, 0, 40, 0&quot;/&gt;
        &lt;/Border&gt;
    &lt;/Button&gt;
&lt;/Grid&gt;
&lt;/UserControl&gt;
</pre>

Standard Razor code is used to put dynamic data into the image. The template can reference properties on an object you'll provide when you call the ImageGenerator. That object is available in your template as "Model"
similar to how ASP.NET MVC ViewModels are.

To get a byte array to do with as you please (save it yourself, serve it up directly, cache, etc.): 

<pre>
string xamlString = File.ReadAllText("SampleTemplates/BlueButton.xaml");
string imgOutputFileName = "BlueButton.png";

dynamic viewModel = new ExpandoObject();
viewModel.Text = "Add to Cart";
byte[] image = ImageGenerator.GenerateImage(xamlString, viewModel);
</pre>

To generate a file, add the full path to where you want the PNG saved as a 3rd parameter and call the GenerateImageFile() method instead:

<pre>
string xamlString = File.ReadAllText("SampleTemplates/BlueButton.xaml");
string imgOutputFileName = "BlueButton.png";

dynamic viewModel = new ExpandoObject();
viewModel.Text = "Add to Cart";
                       
String imageOutputPath = Directory.CreateDirectory("Output").FullName;
String fullFileName = Path.Combine(imageOutputPath,imgOutputFileName);

byte[] imageFile = ImageGenerator.GenerateImageFile(xamlString, viewModel, fullFileName);
</pre>

When generating the file, you'll still get back the byte array, but the file will be sitting in the place you specify.
