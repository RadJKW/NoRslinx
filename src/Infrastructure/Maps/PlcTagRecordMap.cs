using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper.Configuration;
using NoRslinx.Application.PlcTags;

namespace NoRslinx.Infrastructure.Maps;
public partial class PlcTagRecordMap : ClassMap<TagRecord>
{
    public PlcTagRecordMap()
    {
        AutoMap(CultureInfo.InvariantCulture);

        // Map the PlcTag

    }

}
