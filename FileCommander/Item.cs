namespace FileCommander;

public class Item
{
    public string Path { get; private set; }
    public string? Name { get; private set; }
    public bool IsDirectory { get; private set; }
    
    public Item(string path, string? name, bool isDirectory)
    {
        Path = path;
        Name = name;
        IsDirectory = isDirectory;
    }
}