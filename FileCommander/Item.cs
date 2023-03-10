namespace FileCommander;

public class Item
{
    public string Path { get; private set; }
    public string Filename { get; private set; }
    public bool IsDirectory { get; private set; }
    
    public Item(string path, string filename, bool isDirectory)
    {
        Path = path;
        Filename = filename;
        IsDirectory = isDirectory;
    }
}