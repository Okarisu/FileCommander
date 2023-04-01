using System.IO.Compression;
using FileCommander.GUI.Dialogs;

namespace FileCommander.core;

public class FileHandler
{
    private string _sourcePath { get; set; }
    private string _targetPath { get; set; }
    private bool _isDirectory { get; set; }

    public FileHandler(string sourcePath, string targetPath, bool isDirectory)
    {
        _sourcePath = sourcePath;
        _targetPath = targetPath;
        _isDirectory = isDirectory;
    }
    
    public void Move()
    {
        try
        {
            if (_isDirectory)
            {
                Directory.Move(_sourcePath, _targetPath);
            }
            else
            {
                File.Move(_sourcePath, _targetPath);
            }
        }
        catch (Exception e)
        {
            new PromptUserDialogWindow("Unknown error has occured.");
            //continue
        }
    }

    public void Copy()
    {
        try
        {
            if (_isDirectory)
            {
                RecursiveCopyDirectory(_sourcePath, _targetPath);
            }
            else
            {
                File.Copy(_sourcePath, _targetPath);
            }
        }
        catch (Exception e)
        {
            new PromptUserDialogWindow("Unknown error has occured.");
            //continue
        }
    }
    
    /*
     * MICROSOFT. How to: Copy directories. Microsoft: Microsoft Learn [online]. [cit. 2023-03-11].
     * Dostupné z: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories.
     * Upraveno.
     */

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
        
        foreach (DirectoryInfo subDir in dirs)
        {
            var newDestinationDir = Path.Combine(destinationDirectory, subDir.Name);
            RecursiveCopyDirectory(subDir.FullName, newDestinationDir);
        }
    }
    /* Konec citace */
    
    public void Compress()
    {
        try
        {
            ZipFile.CreateFromDirectory(_sourcePath, _targetPath);
            Directory.Delete(_sourcePath, true);

        }
        catch (Exception e)
        {
            new PromptUserDialogWindow("Unknown error has occured.");
            //continue
        }
    }
    public void Extract()
    {
        try
        {
            ZipFile.ExtractToDirectory(_sourcePath, _targetPath);

        }
        catch (Exception e)
        {
            new PromptUserDialogWindow("Unknown error has occured.");
            //continue
        }
    }

    
}