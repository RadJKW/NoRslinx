// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Domain.Entities;

namespace NoRslinx.Infrastructure.Services;

public class RslogixDbImporter //: IRsLogixDbImporter
{
    private readonly Uri _csvFilePath;
    private readonly Uri _jsonFilePath;
    private readonly int _addressColumn;
    private readonly int _symbolColumn;
    private readonly int[] _descriptionColumns;
    private readonly List<PlcTag> _plcTags = new();
    private readonly MicrologixPlc _plc = new();

    /// <summary>
    ///    Initializes a new instance of the <see cref="RslogixDbImporter" /> class.
    /// </summary>
    /// <param name="csvFilePath"></param>
    /// <param name="jsonFilePath"></param>
    /// <param name="addressColumn"></param>
    /// <param name="symbolColumn"></param>
    /// <param name="descriptionColumns"></param>
    /// <param name="plc"></param>
    public RslogixDbImporter(Uri csvFilePath, Uri jsonFilePath, int addressColumn, int symbolColumn, int[] descriptionColumns, MicrologixPlc plc)
    {
        _csvFilePath = csvFilePath;
        _jsonFilePath = jsonFilePath;
        _addressColumn = addressColumn;
        _symbolColumn = symbolColumn;
        _descriptionColumns = descriptionColumns;
        _plc = plc;
    }

    public MicrologixPlc Plc => _plc;
    public List<PlcTag> PlcTags => _plcTags;


    /// <summary>
    /// Converts the CSV file to a JSON file
    /// </summary>
    public void Convert()
    {
        // overwrite the json file data with string.empty
        File.WriteAllText(_jsonFilePath.LocalPath, string.Empty);


        // step 1 - assign the file name to the _plc.Program property
        _plc.Program = _csvFilePath.Segments[^1];
        // step 2 
        // - read the csv file and create a IntPlcTag for each row
        // - if the PlcTag has and empty string in _symbolColumn, do not add it to the list
        using var reader = new StreamReader(_csvFilePath.LocalPath);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line!.Split(',');
            if (values[_symbolColumn] != string.Empty)
            {
                var plcTag = new PlcTag
                {
                    Address = values[_addressColumn],
                    SymbolName = values[_symbolColumn],
                    Description = GetDescription(values),
                    PlcId = _plc.Id

                };

                _plcTags.Add(plcTag);
            }

            // - if the Tag has no symbolName but is associated with the previous tag
            // - add the tag to the list of tags
            else if (values[_addressColumn].Split('/')[0] == _plcTags[^1].Address)
            {
                var plcTag = new PlcTag
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

        // step 4 - erase the json files contents if any
        File.WriteAllText(_jsonFilePath.LocalPath, string.Empty);


        // step 5 - serialize the _plc object to a JSON file
        var json = JsonConvert.SerializeObject(_plc, Formatting.Indented);
        File.WriteAllText(_jsonFilePath.LocalPath, json);
    }

    private string GetDescription(string[] values)
    {
        var description = string.Empty;

        foreach (var column in _descriptionColumns)
        {
            description += values[column];
        }


        return description
                .Replace("\"", string.Empty)
                .Replace("\\", string.Empty)
                .Trim();
    }
}





/* public class RsLogixDbImporter : IRsLogixDbImporter
{
    public Uri CsvFilePath { get; set; } = null!;
    public Uri JsonFilePath { get; set; } = null!;

    private bool _isConverted = false;

    public List<PlcTag> PlcTags { get; } = new();

    public MicrologixPlc Plc
    {
        get
        { // if the plc has tags 

        }
    }



    public void Convert(int addressColumn, int symbolColumn, int[] descriptionColumns)
    {
        // step 1 - assign the name of the CSV file to _plc.Program
        Plc.Program = CsvFilePath.Segments[^1];
        // step 2 
        // - read the csv file and create a IntPlcTag for each row
        // - if the IntPlcTag has and empty string in _symbolColumn, do not add it to the list
        using var reader = new StreamReader(CsvFilePath.LocalPath);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line!.Split(',');
            if (values[symbolColumn] != string.Empty)
            {
                var intPlcTag = new PlcTag
                {
                    Address = values[addressColumn],
                    SymbolName = values[symbolColumn],
                    Description = string.Empty
                };

                // the description columns were split into multiple columns in the CSV file
                // so we need to combine them into a single string

                foreach (var descriptionColumn in descriptionColumns)
                {
                    intPlcTag.Description += values[descriptionColumn];
                }

                PlcTags.Add(intPlcTag);
            }
        }

        // step 3 - assign the list of IntPlcTags to _plc.Tags
        Plc.PlcTags = PlcTags;

        // step 4 - serialize the _plc object to a JSON file
        var json = JsonConvert.SerializeObject(Plc, Formatting.Indented);
        File.WriteAllText(JsonFilePath.LocalPath, json);
        _isConverted = true;
    }

    // after the JSON file is created, allow the user to request the MicrologixPlc
    // object from the importer
}
*/

