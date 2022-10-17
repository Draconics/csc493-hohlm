using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DraconianWorldbuilder
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            string titleText = TitleBox.Text;
            string summaryText = SummaryBox.Text;
        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            var openPicker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".rtf");

            // Get the current window's HWND by passing in the Window object
            var hwnd = WindowNative.GetWindowHandle(this);
            // Associate the HWND with the file picker
            InitializeWithWindow.Initialize(openPicker, hwnd);

            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                try
                {
                    //IRandomAccessStreamWithContentType
                    var randAccStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    // Load the file into the Document property of the RichEditBox.
                    editor.Document.LoadFromStream(Microsoft.UI.Text.TextSetOptions.FormatRtf, randAccStream);

                    using (var sr = new StreamReader(randAccStream.AsStream())) {
                        // find final }
                        int count = 0, stop = 0;
                        while (stop < 1){
                            string junk = sr.ReadLine();
                            for (int i = 0; i < junk.Length; i++)
                            {
                                if (junk[i] == '{')
                                {
                                    count++;
                                }
                                else if (junk[i] == '}')
                                {
                                    count--;
                                }
                            }
                            if (count == 0)
                                stop++;
                        }
                        sr.ReadLine();
                        TitleBox.Text = sr.ReadLine();
                        SummaryBox.Text = sr.ReadLine();
                    }
                }
                catch (Exception)
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = "File open error",
                        Content = "Sorry, I couldn't open the file.",
                        PrimaryButtonText = "Ok"
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "New Document"
            };
            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

            var hwnd = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(savePicker, hwnd);

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we
                // finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                
                // Write to file
                var randAccStream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                editor.Document.SaveToStream(Microsoft.UI.Text.TextGetOptions.FormatRtf, randAccStream);

                using (var sw = new StreamWriter(randAccStream.AsStream()))
                {
                    sw.WriteLine();
                    sw.WriteLine(TitleBox.Text);
                    sw.WriteLine(SummaryBox.Text);
                }

                // Let Windows know that we're finished changing the file so the
                // other app can update the remote version of the file.
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status != Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    var errorBox = new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                    await errorBox.ShowAsync();
                }
            }
        }

        private void LinkButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null && LinkBox.Text != "")
            {
                // hyperlink is the 'button', run is text on the button 
                Hyperlink hyperlink = new Hyperlink();
                Run run = new Run();
                run.Text = selectedText.Text;
                // Set the click event
                hyperlink.Click += (Link_Click);  
                // Formatting
                hyperlink.UnderlineStyle = UnderlineStyle.None;
                hyperlink.FontWeight = Microsoft.UI.Text.FontWeights.SemiBold;

                // Merges run and hyperlink
                hyperlink.Inlines.Add(run);   
                Italic italic = new Italic();
                italic.Inlines.Add(hyperlink);

                // Adds link to textbox
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(italic);
                linkList.Blocks.Add(paragraph);   
            }
        }

        private async void Link_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "C:\\Users\\hohlm\\Documents\\wtf.rtf";
            StorageFile file = await StorageFile.GetFileFromPathAsync(filePath); ;
           
            //File load code. See Open Button for commented code
            if (file != null)
            {
                try
                {
                    var randAccStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    editor.Document.LoadFromStream(Microsoft.UI.Text.TextSetOptions.FormatRtf, randAccStream);

                    using (var sr = new StreamReader(randAccStream.AsStream()))
                    {
                        int count = 0, stop = 0;
                        while (stop < 1)
                        {
                            string junk = sr.ReadLine();
                            for (int i = 0; i < junk.Length; i++)
                            {
                                if (junk[i] == '{')
                                {
                                    count++;
                                }
                                else if (junk[i] == '}')
                                {
                                    count--;
                                }
                            }
                            if (count == 0)
                                stop++;
                        }
                        sr.ReadLine();
                        TitleBox.Text = sr.ReadLine();
                        SummaryBox.Text = sr.ReadLine();
                    }
                }
                catch (Exception)
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = "File open error",
                        Content = "Sorry, I couldn't open the file.",
                        PrimaryButtonText = "Ok"
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Microsoft.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = Microsoft.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Microsoft.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = Microsoft.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Microsoft.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Microsoft.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Microsoft.UI.Text.UnderlineType.Single;
                }
                else
                {
                    charFormatting.Underline = Microsoft.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }
    }
}
