using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using MsBox.Avalonia.Models; // Add this using directive


using Newtonsoft.Json;
using MsBox.Avalonia.Dto;

namespace ASLX2JSON.Views
{
    public class MainWindow : Window
    {
        private TextBox _xmlPathBox = null!;
        private TextBox _jsonPathBox = null!;
        private Button _browseXmlButton = null!;
        private Button _browseJsonButton = null!;
        private Button _convertButton = null!;
        private TextBlock _xmlFileNameTextBlock = null!;
        private TextBlock _jsonFileNameTextBlock = null!;
        private TextBlock _conversionMessageTextBlock = null!;

        public MainWindow()
        {
            InitializeComponent();

            _xmlPathBox = this.FindControl<TextBox>("XmlPathBox");
            _jsonPathBox = this.FindControl<TextBox>("JsonPathBox");
            _browseXmlButton = this.FindControl<Button>("BrowseXmlButton");
            _browseJsonButton = this.FindControl<Button>("BrowseJsonButton");
            _convertButton = this.FindControl<Button>("ConvertButton");
            _xmlFileNameTextBlock = this.FindControl<TextBlock>("XmlFileNameTextBlock");
            _jsonFileNameTextBlock = this.FindControl<TextBlock>("JsonFileNameTextBlock");
            _conversionMessageTextBlock = this.FindControl<TextBlock>("ConversionMessageTextBlock");

            if (_xmlPathBox == null || _xmlFileNameTextBlock == null)
            {
                throw new InvalidOperationException("Controls not found in the XAML layout.");
            }

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
                Title = "Select ASLX File",
                Filters = new List<FileDialogFilter>
                    {
                        new FileDialogFilter { Name = "Quest 5 Files", Extensions = new List<string> { "aslx" } }
                    }
            };

            var result = await dialog.ShowAsync(this);
            if (result != null && result.Length > 0)
            {
                _xmlPathBox.Text = result[0];
                _xmlFileNameTextBlock.Text = result[0]; // Set the file name in the TextBlock
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
                _jsonFileNameTextBlock.Text = result; // Set the file name in the TextBlock
            }
        }

        private async void ConvertXmlToJson()
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

                _conversionMessageTextBlock.Text = "File successfully converted!";
            }
            catch (Exception ex)
            {
                _conversionMessageTextBlock.Text = $"Error: {ex.Message}";
            }
        }
    }
}
