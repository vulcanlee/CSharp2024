using csMenuJson.Helpers;
using csMenuJson.Models;
using System.Text.Json;

namespace csMenuJson.Services;

public class MenuDataRoleBuildService
{
    private readonly ILogger<MenuDataRoleBuildService> _logger;
    public MenuDataRoleBuildService(ILogger<MenuDataRoleBuildService> logger)
    {
        _logger = logger;
    }

    public MenuDataRole ReadMenuListFromFile(string filePath)
    {
        try
        {
            var jsonString = File.ReadAllText(MagicHelper.MenuTemplateFilename);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var menuDataRole = JsonSerializer.Deserialize<MenuDataRole>(jsonString, options);
            return menuDataRole;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading or deserializing the file.");
            return null;
        }
    }
}
