using System.Reflection.Emit;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using RslogixDbImporter.Models;

namespace RslogixDbImporter;
// TOOO: does not append the plc tags to the json file 
// the json file needs to be overwritten each time

public class CsvToJson
{
    private readonly Uri _csvFilePath;
    private readonly Uri _jsonFilePath;
    private readonly int _addressColumn;
    private readonly int _symbolColumn;
    private readonly int[] _descriptionColumns;

    private readonly List<IntPlcTag> _plcTags = new();

    private readonly MicrologixPlc _plc = new();

    /// <summary>
    /// Converts an RsLogix Database CSV file to a JSON file
    /// </summary>
    /// <param name="csvFilePath"></param>
    /// <param name="jsonFilePath"></param>
    /// <param name="addressColumn"></param>
    /// <param name="symbolColumn"></param>
    /// <param name="descriptionColumns"></param>
    public CsvToJson(Uri csvFilePath, Uri jsonFilePath, int addressColumn, int symbolColumn, int[] descriptionColumns, MicrologixPlc plc)
    {
        _csvFilePath = csvFilePath;
        _jsonFilePath = jsonFilePath;
        _addressColumn = addressColumn;
        _symbolColumn = symbolColumn;
        _descriptionColumns = descriptionColumns;
        _plc = plc;
        // if the json file has data, clear it
        if (File.Exists(_jsonFilePath.LocalPath))
        {
            File.WriteAllText(_jsonFilePath.LocalPath, string.Empty);
        }
    }

    /// <summary>
    /// Converts the CSV file to a JSON file
    /// </summary>
    public void Convert()
    {
        // step 1 - assign the name of the CSV file to _plc.Program
        _plc.Program = _csvFilePath.Segments[^1];
        // step 2 
        // - read the csv file and create a IntPlcTag for each row
        // - if the IntPlcTag has and empty string in _symbolColumn, do not add it to the list
        using var reader = new StreamReader(_csvFilePath.LocalPath);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line!.Split(',');
            if (values[_symbolColumn] != string.Empty)
            {
                var plcTag = new IntPlcTag
                {
                    Address = values[_addressColumn],
                    SymbolName = values[_symbolColumn],
                    Description = GetDescription(values),
                    PlcId = _plc.Id,
                };

                _plcTags.Add(plcTag);
            }
            // sometimes a tag will have two parts to it, like T4:1 and T4:1/DN
            // if the tag does not have a symbol name, but matches the address of the previous tag then we should add the tag
            // example: T4:1 was just added so the next address is T4:1/DN
            //          - split the address on the '/' and get the first part
            //          - if the first part matches the previous tag's address, then add the tag
            else if (values[_addressColumn].Split('/')[0] == _plcTags[^1].Address)
            {
                var plcTag = new IntPlcTag
                {
                    Address = values[_addressColumn],
                    SymbolName = _plcTags[^1].SymbolName,
                    Description = GetDescription(values),
                    PlcId = _plc.Id
                };
                _plcTags.Add(plcTag);
            }
        }

        // step 3 - assign the list of IntPlcTags to _plc.Tags
        _plc.PlcTags = _plcTags;

        // step 4 - serialize the _plc object to a JSON file
        var json = JsonConvert.SerializeObject(_plc, Formatting.Indented);
        File.WriteAllText(_jsonFilePath.LocalPath, json);
    }

    private string GetDescription(string[] values)
    {
        var description = string.Empty;

        foreach (var column in _descriptionColumns)
        {
            description += values[column] + " ";
        }
        return description.Replace("\"", string.Empty)
                           .Replace("\\", string.Empty)
                           .Trim();
    }


}
