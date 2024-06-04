using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using Uno.Foundation;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;

namespace Composer;

public sealed partial class MainPage : Page
{
    private async void ShowCustomContentDialog()
    {
        // Create a hyperlink
        var hyperlink = new Hyperlink
        {
            NavigateUri = new Uri("https://github.com/slab/quill")
        };
        hyperlink.Inlines.Add(new Run { Text = "Quilljs" });

        // Create a TextBlock and add the hyperlink
        var textBlockWithLink = new TextBlock();
        textBlockWithLink.Inlines.Add(new Run { Text = "Powered by " });
        textBlockWithLink.Inlines.Add(hyperlink);

        // Button to download the license file
        var downloadButton = new Button
        {
            Content = "Download BSD-3 License",
            Margin = new Thickness(0, 10, 0, 0),
            IsTabStop = false
        };
        downloadButton.Click += DownloadButton_Click;

        // Create the ContentDialog
        var contentDialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Title = "About",
            PrimaryButtonText = "OK",
            IsTabStop = false,
            Content = new StackPanel
            {
                Children =
                    {
                        new TextBlock
                        {
                            Text = "Coded by Ibrahim (tussin12413@gmail.com)\nThis web app is an experimental free to use app and may therefore contain bugs. It is for use as is.\nTo report issues, DM me on twitter.",
                            Margin = new Thickness(0, 0, 0, 10)
                        },
                        textBlockWithLink,
                        downloadButton
                    }
            }
        };

        await contentDialog.ShowAsync();
    }

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        var savePicker = new FileSavePicker
        {
            SuggestedStartLocation = PickerLocationId.Desktop
        };
        savePicker.FileTypeChoices.Add("Text File", new[] { ".txt" });
        savePicker.SuggestedFileName = "license";

        StorageFile file = await savePicker.PickSaveFileAsync();
        if (file != null)
        {
            CachedFileManager.DeferUpdates(file);
            await FileIO.WriteTextAsync(file, GetLicenseText());
            FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
        }
    }

    private string GetLicenseText()
    {
        return @"Copyright (c) 2017-2024, Slab
Copyright (c) 2014, Jason Chen
Copyright (c) 2013, salesforce.com
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

1. Redistributions of source code must retain the above copyright
   notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright
   notice, this list of conditions and the following disclaimer in the
   documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its
   contributors may be used to endorse or promote products derived from
   this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS ""AS
IS"" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.";
    }

    private async void ShowCopyContentDialog()
    {
        // Create the content dialog
        var copyContentDialog = new ContentDialog
        {
            XamlRoot = this.XamlRoot,
            Title = "Confirm Copy",
            Content = "This will copy the contents of the editor as an image in your clipboard. It will replace your clipboard. Do you wish to continue?",
            PrimaryButtonText = "Yes",
            SecondaryButtonText = "No",
            CloseButtonText = "Cancel",
            IsTabStop = false,
        };


        // Show the dialog and get the result
        var result = await copyContentDialog.ShowAsync();

        // Handle the result
        if (result == ContentDialogResult.Primary)
        {
            // Handle Yes button click
            WebAssemblyRuntime.InvokeJS("captureDivToClipboard()");
        }
        else if (result == ContentDialogResult.Secondary)
        {
            // Handle No button click
            // Optionally do nothing or add logic here
        }
    }
}
