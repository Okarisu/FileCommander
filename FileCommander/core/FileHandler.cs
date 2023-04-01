using System;
using System.IO;
using System.IO.Compression;
using FileCommander.GUI.Dialogs;

// ReSharper disable ObjectCreationAsStatement

namespace FileCommander.core;

public class FileHandler
{
    private string SourcePath { get; set; }
    private string TargetPath { get; set; }
    private bool IsDirectory { get; set; }

    public FileHandler(string sourcePath, string targetPath, bool isDirectory)
    {
        SourcePath = sourcePath;
        TargetPath = targetPath;
        IsDirectory = isDirectory;
    }

    public void Move()
    {
        try
        {
            if (IsDirectory)
            {
                Directory.Move(SourcePath, TargetPath);
            }
            else
            {
                File.Move(SourcePath, TargetPath);
            }
        }
        catch (Exception)
        {
            new PromptUserDialogWindow("Unknown error has occured.");
            //continue
        }
    }

    public void Copy()
    {
        try
        {
            if (IsDirectory)
            {
                RecursiveCopyDirectory(SourcePath, TargetPath);
            }
            else
            {
                File.Copy(SourcePath, TargetPath);
            }
        }
        catch (Exception)
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
            try
            {
                var targetFilePath = Path.Combine(destinationDirectory, file.Name);
                file.CopyTo(targetFilePath);
            }
            catch (Exception)
            {
                new PromptUserDialogWindow("Unknown error has occured.");
                //continue
            }
        }

        foreach (DirectoryInfo subDir in dirs)
        {
            try
            {
                var newDestinationDir = Path.Combine(destinationDirectory, subDir.Name);
                RecursiveCopyDirectory(subDir.FullName, newDestinationDir);
            }
            catch (Exception)
            {
                new PromptUserDialogWindow("Unknown error has occured.");
                //continue
            }
        }
    }
    /* Konec citace */

    public void Delete()
    {
        try
        {
            if (IsDirectory)
            {
                Directory.Delete(SourcePath, true);
            }
            else
            {
                File.Delete(SourcePath);
            }
        }
        catch (Exception)
        {
            new PromptUserDialogWindow("Unknown error has occured.");
            //continue
        }
    }

    public void Compress()
    {
        try
        {
            ZipFile.CreateFromDirectory(SourcePath, TargetPath);
            Directory.Delete(SourcePath, true);
        }
        catch (Exception)
        {
            new PromptUserDialogWindow("Unknown error has occured.");
            //continue
        }
    }

    public void Extract()
    {
        try
        {
            ZipFile.ExtractToDirectory(SourcePath, TargetPath);
        }
        catch (Exception)
        {
            new PromptUserDialogWindow("Unknown error has occured.");
            //continue
        }
    }
}