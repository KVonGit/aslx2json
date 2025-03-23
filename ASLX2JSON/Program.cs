using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: xmltojson.exe <input.xml> [-o <output.json>]");
            return;
        }

        string xmlFilePath = args[0];
        string jsonFilePath = "output.json";

        // Check for output file argument
        for (int i = 1; i < args.Length; i++)
        {
            if (args[i] == "-o" && i + 1 < args.Length)
            {
                jsonFilePath = args[i + 1];
                break;
            }
        }

        try
        {
            // Load the XML file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);

            // Convert XML to JSON
            string jsonText = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);

            // Write JSON to file
            File.WriteAllText(jsonFilePath, jsonText);

            Console.WriteLine("Conversion successful! JSON saved as " + jsonFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}
