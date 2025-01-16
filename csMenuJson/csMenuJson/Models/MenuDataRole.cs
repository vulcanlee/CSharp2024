namespace csMenuJson.Models;

/// <summary>
/// 表示菜單數據角色的類別。
/// </summary>
public class MenuDataRole
{
    public List<Menu> Menus { get; set; } = new();
    public List<DataRole> DataRoles { get; set; } = new();
}

/// <summary>
/// 表示子菜單的類別。
/// </summary>
public class Menu
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public int Order { get; set; }
    public List<MenuItem> SubMenu { get; set; } = new();
    public MenuItem OnlyMenuItem { get; set; }
}

/// <summary>
/// 表示菜單項目的類別。
/// </summary>
public class MenuItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Icon { get; set; }
    public int Order { get; set; }
    public List<string> Permissions { get; set; } = new();
}

/// <summary>
/// 表示數據角色的類別。
/// </summary>
public class DataRole
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Enable { get; set; }
    public List<string> DataPermissions { get; set; } = new();
}
