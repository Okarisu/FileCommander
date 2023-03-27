namespace FileCommander.core;

public class cpp
{
    public string SourceFilePath { get; set; }
    public string DestFilePath { get; set; }
    public bool isDirectory { get; set; }
    
    public cpp(string Source, string Dest, bool isDirectory)
    {
        this.SourceFilePath = Source;
        this.DestFilePath = Dest;
        this.isDirectory = isDirectory;
    }

    public void Copy()
    {
        if (isDirectory)
        {
            RecursiveCopyDirectory(SourceFilePath, DestFilePath);
        }
        else
        {
            File.Copy(SourceFilePath, DestFilePath);
        }
    }
    
    private static void RecursiveCopyDirectory(string sourceDirectory, string destinationDirectory)
    {
        var dir = new DirectoryInfo(sourceDirectory);

        // Cache directories before start of copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        Directory.CreateDirectory(destinationDirectory);

        foreach (FileInfo file in dir.GetFiles())
        {
            var targetFilePath = Path.Combine(destinationDirectory, file.Name);
            file.CopyTo(targetFilePath);
        }
        
        //FileSystem.COp

        foreach (DirectoryInfo subDir in dirs)
        {
            var newDestinationDir = Path.Combine(destinationDirectory, subDir.Name);
            RecursiveCopyDirectory(subDir.FullName, newDestinationDir);
        }
    }
}