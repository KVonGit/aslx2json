using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;

namespace ASLX2JSON.Views
{
    public class MainWindow : Window
    {
        private TextBox _xmlPathBox = null!;
        private TextBox _jsonPathBox = null!;
        private Button _browseXmlButton = null!;
        private Button _browseJsonButton = null!;
        private Button _convertButton = null!;

        public MainWindow()
        {
            InitializeComponent();

            _xmlPathBox = this.FindControl<TextBox>("XmlPathBox");
            _jsonPathBox = this.FindControl<TextBox>("JsonPathBox");
            _browseXmlButton = this.FindControl<Button>("BrowseXmlButton");
            _browseJsonButton = this.FindControl<Button>("BrowseJsonButton");
            _convertButton = this.FindControl<Button>("ConvertButton");

            _browseXmlButton.Click += async (_, _) => await SelectXmlFile();
            _browseJsonButton.Click += async (_, _) => await SelectJsonFile();
            _convertButton.Click += (_, _) => ConvertXmlToJson();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task SelectXmlFile()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select ASLX File to Convert",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "Quest 5 Files", Extensions = new List<string> { "aslx" } }
                }
            };

            var result = await dialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                _xmlPathBox.Text = result[0];
            }
        }

        private async Task SelectJsonFile()
        {
            var dialog = new SaveFileDialog
            {
                Title = "Select JSON File",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "JSON Files", Extensions = new List<string> { "json" } }
                }
            };

            var result = await dialog.ShowAsync(this);
            if (result != null)
            {
                _jsonPathBox.Text = result;
            }
        }

        private void ConvertXmlToJson()
        {
            try
            {
                var xmlPath = _xmlPathBox.Text;
                var jsonPath = _jsonPathBox.Text;

                if (string.IsNullOrEmpty(xmlPath) || string.IsNullOrEmpty(jsonPath))
                {
                    throw new InvalidOperationException("Both XML and JSON paths must be specified.");
                }

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);

                var json = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);

                File.WriteAllText(jsonPath, json);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., show a message to the user)
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
