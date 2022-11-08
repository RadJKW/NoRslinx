using System.Security.Cryptography.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoRslinx.Application.Common.Interfaces;
using NoRslinx.Domain.Entities;
using NoRslinx.Infrastructure.Services;

namespace NoRslinx.Infrastructure.Persistence;
public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IRsLogixDbImporter _logixImporter;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, IRsLogixDbImporter logixImporter)
    {
        _logger = logger;
        _context = context;
        _logixImporter = logixImporter;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }

        // Seed, if necessary
        // Default Plc for testing

        var defaultPlc = new MicrologixPlc
        {
            Name = "RadJKW-MLGX1100",
            IpAddress = "192.168.0.23",
            Description = "Office Dev PLC"
        };

        if (!_context.MicrologixPlcs.Any())
        {
            _context.MicrologixPlcs.Add(defaultPlc);
            await _context.SaveChangesAsync();


        }

        // Seed always while development
        // Data from RsLogixDb Exported CSV

        if (!_context.PlcTags.Any())
        {
            var basePath = new Uri("C:/Users/jwest/source/NoRslinx-AbPlc/NoRslinx/rslogix/");
            var csvFilePath = new Uri(basePath, "DEV-PLC2.CSV");
            var jsonFilePath = new Uri(basePath, "DEV-PLC2.JSON");
            var addressColumn = 0;
            var symbolColumn = 2;
            var descriptionColumns = new int[] { 3, 4, 5, 6, 7 };

            var plcFromDb = _context.MicrologixPlcs.FirstOrDefault(x => x.Name == defaultPlc.Name);

            _logixImporter.Convert(csvFilePath, jsonFilePath, addressColumn, symbolColumn, descriptionColumns, plcFromDb!);

            var plcTags = _logixImporter.PlcTags;
            _context.PlcTags.AddRange(plcTags);

            await _context.SaveChangesAsync();

        }

    }
}
