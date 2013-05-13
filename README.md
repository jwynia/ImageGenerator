ImageGenerator
==============

ImageGenerator creates PNG images from XAML templates. Use it to put text onto buttons or any other purpose where you need to generate images based on runtime data.

Usage
==============
After installing the package, you'll need to create XAML templates for the types of images you want to create. A basic button is included as a sample.

<UserControl
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d">
    <Grid x:Name="LayoutRoot"  HorizontalAlignment="Left" VerticalAlignment="Top">
        <Button HorizontalAlignment="Center"
                BorderThickness="0"
                VerticalAlignment="Center"
                FontFamily="Calibri" 
                FontSize="13" 
                FontWeight= "Bold"
            >
            <Button.Background>
                <SolidColorBrush Color="#153E7E" />
            </Button.Background>
            <Border Margin="2"
                    BorderBrush="#ffffff"
                    BorderThickness="1"
                    Padding="5">
                <TextBlock Text="@Model.Text.ToUpper()" Foreground="#ffffff" Padding="40, 0, 40, 0" />
            </Border>
        </Button>
    </Grid>
</UserControl>

