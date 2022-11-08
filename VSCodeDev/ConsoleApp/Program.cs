// See https://aka.ms/new-console-template for more information
using RslogixDbImporter;
using RslogixDbImporter.Models;

var basepath = new Uri("C:/Users/jwest/source/NoRslinx-AbPlc/NoRslinx/VSCodeDev/ConsoleApp/RslogixFiles/");
var csvFilePath = new Uri(basepath, "DEV-PLC2.csv");
var jsonFilePath = new Uri(basepath, "DbExport.json");
var addressColumn = 0;
var symbolColumn = 2;
var descriptionColumns = new int[] { 3, 4, 5, 6, 7 };

MicrologixPlc plc = new()
{
    Name = "RadJKW-MLGX1100",
    IpAddress = "192.168.0.23",
    Description = "Office Dev PLC"
};

var csvToJson = new CsvToJson(csvFilePath, jsonFilePath, addressColumn, symbolColumn, descriptionColumns, plc);
csvToJson.Convert();
