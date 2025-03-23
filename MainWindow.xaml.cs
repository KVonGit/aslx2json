using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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
}
