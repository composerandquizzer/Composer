using Microsoft.UI;
using Uno.Foundation;

namespace Composer;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        this.Resources["ButtonBackgroundPointerOver"] = Colors.DarkGray;
        this.Resources["ButtonBackgroundPressed"] = Colors.LightBlue;

        var stackpanel = new StackPanel()
        {
            Orientation = Orientation.Vertical,
            Background = new SolidColorBrush(Colors.White)
        };

        var btnCopy = new Button()
        {
            Content = "Copy As Image",
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 80, 10, 0),
            Background = new SolidColorBrush(Colors.Gray),
            IsTabStop = false // Disable tab stop
        };
        var btnDownloadHTML = new Button()
        {
            Content = "Download As HTML",
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 80, 10, 0),
            Background = new SolidColorBrush(Colors.Gray),
            IsTabStop = false // Disable tab stop
        };
        var btnDownloadDOCX = new Button()
        {
            Content = "Download As DOCX",
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 80, 10, 0),
            Background = new SolidColorBrush(Colors.Gray),
            IsTabStop = false
        };
        var btnAbout = new Button()
        {
            Content = "About",
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 80, 10, 0),
            Background = new SolidColorBrush(Colors.Gray),
            IsTabStop = false
        };

        var btnstackpanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        btnstackpanel.Children.Add(btnCopy);
        btnstackpanel.Children.Add(btnDownloadHTML);
        btnstackpanel.Children.Add(btnDownloadDOCX);
        btnstackpanel.Children.Add(btnAbout);


        DivControl divControl = new DivControl()
        {
            Height = 500,
            Width = 700
        };

        btnCopy.Click += (sender, e) =>
        {
            ShowCopyContentDialog();
        };
        btnDownloadHTML.Click += (sender, e) =>
        {
            WebAssemblyRuntime.InvokeJS("saveContentAsHTML()");
        };
        btnDownloadDOCX.Click += (sender, e) =>
        {
            WebAssemblyRuntime.InvokeJS("saveContentAsDOCX()");
        };
        btnAbout.Click += (sender, e) =>
        {
            ShowCustomContentDialog();
        };

        this.Loaded += async (sender, e) =>
        {
            await Task.Delay(100);
            divControl.Height = Window.Current.Bounds.Height - btnCopy.ActualHeight - 90;
        };
        stackpanel.Children.Add(divControl);
        stackpanel.Children.Add(btnstackpanel);
        Content = stackpanel;

    }
}
