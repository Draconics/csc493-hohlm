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

        private async void loadFile(string filePath)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(filePath);
            if (file != null)
            {
                try
                {
                    //IRandomAccessStreamWithContentType
                    var randAccStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    // Load the file into the Document property of the RichEditBox.
                    editor.Document.LoadFromStream(Microsoft.UI.Text.TextSetOptions.FormatRtf, randAccStream);

                    using (var sr = new StreamReader(randAccStream.AsStream()))
                    {
                        // find final }
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

                        linkButtonList.Children.Clear();
                        links.Clear();
                        while (!sr.EndOfStream)
                        {
                            HyperlinkButton linkButton = new HyperlinkButton();
                            linkButton.Content = sr.ReadLine();
                            linkButton.Click += (Link_Click);

                            links.Add(sr.ReadLine());
                            int slot = links.Count - 1;
                            linkButton.Name = "" + slot;

                            linkButtonList.Children.Add(linkButton);
                        }
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
            string filePath = file.Path;
            loadFile(filePath);
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
                    //Woo. Now we need to get a list of children
                    foreach(HyperlinkButton hypeButt in linkButtonList.Children)
                    {
                        sw.WriteLine(hypeButt.Content);
                        int index = Int32.Parse(hypeButt.Name);
                        sw.WriteLine(links[index]);
                    }
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

        private async void LinkButton_Click(object sender, RoutedEventArgs e)
        {
            //Microsoft.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (LinkBox.Text != "")
            {
                var openPicker = new FileOpenPicker
                {
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
                openPicker.FileTypeFilter.Add(".rtf");
                var hwnd = WindowNative.GetWindowHandle(this);
                InitializeWithWindow.Initialize(openPicker, hwnd);

                var file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    HyperlinkButton linkButton = new HyperlinkButton();
                    linkButton.Content = LinkBox.Text;
                    linkButton.Click += (Link_Click);
                    // This returns the filepath the link should path to
                    string filePath = file.Path;

                    links.Add(filePath);
                    int index = links.Count-1;
                    linkButton.Name = "" + index;

                    linkButtonList.Children.Add(linkButton);
                }
            }
        }

        private List<string> links = new List<string>();

        private void Link_Click(object sender, RoutedEventArgs e)
        {
            int index = Int32.Parse((sender as HyperlinkButton).Name);
            string filePath = links[index];
            loadFile(filePath);
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
