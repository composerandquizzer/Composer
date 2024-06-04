using Uno.UI.Runtime.WebAssembly;

[HtmlElement("div")]
public sealed partial class DivControl : FrameworkElement
{
    public DivControl()
    {
        this.SetHtmlContent(@"<div id=""editor"">

        </div>");
        Task.Delay(100).ContinueWith(t => //<- Wait until page is rendered
        {
            // add a simple button
            this.ExecuteJavascript("InitializeQuill()");
        });
    }
}

public sealed partial class MainPage : Page
{

}
