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
using static System.Net.WebRequestMethods;

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
        }

        private async void Load_File(string filePath)
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(filePath);
                if (file != null)
                {
                    //IRandomAccessStreamWithContentType
                    Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.Read);
                    // Load the file into the Document property of the RichEditBox.
                    editor.Document.LoadFromStream(Microsoft.UI.Text.TextSetOptions.FormatRtf, randAccStream);

                    using (StreamReader sr = new(randAccStream.AsStream()))
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
                            Generate_Link(sr.ReadLine());
                        }
                    }
                }
            }
            catch (Exception)
            {
                ContentDialog errorDialog = new()
                {
                    Title = "File open error",
                    Content = "File no longer exists.",
                    PrimaryButtonText = "Ok"
                };
                errorDialog.XamlRoot = bigPanel.XamlRoot;
                await errorDialog.ShowAsync();
            }

        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".rtf");

            // Get the current window's HWND by passing in the Window object
            IntPtr hwnd = WindowNative.GetWindowHandle(this);
            // Associate the HWND with the file picker
            InitializeWithWindow.Initialize(openPicker, hwnd);

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                string filePath = file.Path;
                Load_File(filePath);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "New Document"
            };
            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

            IntPtr hwnd = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(savePicker, hwnd);

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we
                // finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);

                // Write to file
                Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.ReadWrite);
                editor.Document.SaveToStream(Microsoft.UI.Text.TextGetOptions.FormatRtf, randAccStream);

                using (StreamWriter sw = new(randAccStream.AsStream()))
                {
                    sw.WriteLine();
                    sw.WriteLine(TitleBox.Text);
                    sw.WriteLine(SummaryBox.Text);
                    //Woo. Now we need to get a list of links
                    foreach(StackPanel hypeButt in linkButtonList.Children.Cast<StackPanel>())
                    {
                        int index = Int32.Parse(s: hypeButt.Name[1..]);
                        sw.WriteLine(links[index]);
                    }
                }

                // Let Windows know that we're finished changing the file so the
                // other app can update the remote version of the file.
                Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status != Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Windows.UI.Popups.MessageDialog errorBox = new("File " + file.Name + " couldn't be saved.");
                    await errorBox.ShowAsync();
                }
            }
        }

        private List<string> links = new();

        private List<string> linkSummaries = new();

        private async void Generate_Link(string filePath)
        {
            // Retrieves link metadata
            String titleText = "";
            String summary = "";
            try
            {
                StorageFile file = await StorageFile.GetFileFromPathAsync(filePath);
                if (file != null)
                {
                    Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.Read);
                    using (StreamReader sr = new(randAccStream.AsStream()))
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
                        titleText = sr.ReadLine();
                        summary = sr.ReadLine();
                    }
                }
            }
            catch (Exception)
            {
                titleText = "File not found";
            }
            // Backend indexing of the file path
            links.Add(filePath);
            linkSummaries.Add(summary);
            int index = links.Count - 1;
            // Create the UI elements for the link
            StackPanel leStack = new()
            {
                Orientation = Orientation.Horizontal,
                Name = "s" + index
            };

            Button killButton = new()
            {
                Content = "x",
                Name = "k" + index
            };
            killButton.Click += KillButton_Click;

            HyperlinkButton linkButton = new()
            {
                Content = titleText,
                Name = "l" + index
            };
            linkButton.Click += Link_Click;

            leStack.Children.Add(killButton);
            leStack.Children.Add(linkButton);
            linkButtonList.Children.Add(leStack);
        }

        private async void LinkButton_Click(object sender, RoutedEventArgs e)
        {
            // File picker business
            FileOpenPicker openPicker = new()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".rtf");
            IntPtr hwnd = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(openPicker, hwnd);

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                Generate_Link(file.Path);
            }
        }

        private void Link_Click(object sender, RoutedEventArgs e)
        {
            int index = Int32.Parse(s: (sender as HyperlinkButton).Name[1..]);
            string filePath = links[index];
            Load_File(filePath);
        }

        private void KillButton_Click(object sender, RoutedEventArgs e)
        {
            int index = Int32.Parse(s: (sender as Button).Name[1..]);
            String name = "s" + index;
            object leStack = linkButtonList.FindName(name);
            StackPanel sp = (StackPanel)leStack;
            linkButtonList.Children.Remove(sp);
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
