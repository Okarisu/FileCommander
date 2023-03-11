using System.IO.Compression;

namespace FileCommander.GUI;

using static InputPathDialogWindow;
using System;
using System.IO;
using Gtk;
using static App;

public abstract class FunctionController
{
    #region Navigation

    public static DirectoryInfo OnHomeClicked(object? sender, EventArgs e, ListStore store)
    {
        var root = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal));
        FillStore(store, root);
        return root;
    }

    public static DirectoryInfo OnUpClicked(DirectoryInfo root, ListStore store)
    {
        if (root.Parent == null)
            return root;

        FillStore(store, root.Parent);
        return root.Parent;
    }

    public static void OnRefreshClicked(object sender, EventArgs e)
    {
        Refresh();
    }

    public static void OnBackClicked(object sender, EventArgs e)
    {
        //TODO logging historie cest? - Queue<Path>, to bude blivajz
    }

    public static void OnForwardClicked(object sender, EventArgs e)
    {
        //viz výše
    }

    public static void OnUndoClicked(object sender, EventArgs e)
    {
        //TODO logging provedených akcí - command pattern
        //https://stackoverflow.com/questions/3448943/best-design-pattern-for-undo-feature
    }

    public static void OnRedoClicked(object sender, EventArgs e)
    {
        //viz výše
    }

    #endregion

    private static string GetPath(string dialogTitle)
    {
        var inputPathDialogWindow = new InputPathDialogWindow(dialogTitle);
        var path = InputPathDialogWindow.GetPath();
        NullPath();
        return path;
    }

    public static void OnNewClicked(object sender, EventArgs e)
    {
        var path = GetPath("New folder");

        var root = GetFocusedWindow() == 1 ? LeftRoot : RightRoot;

        var newDirectoryPath = Path.Combine(root.ToString(), path);
        Directory.CreateDirectory(newDirectoryPath);
        Refresh();
    }

    public static void OnCopyClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        //if (items.files == null) return;

        string destinationPath;
        do
        {
            destinationPath = GetPath("Copy to...");
        } while (!Directory.Exists(destinationPath));

        foreach (var item in items.files)
        {
            if (item!.IsDirectory)
            {
                RecursiveCopyDirectory(item.Path, Path.Combine(destinationPath, item.Name), true);
            }
            else
            {
                File.Copy(item.Path, Path.Combine(destinationPath, item.Name));
            }
        }

        Refresh();
    }

    /*
     * MICROSOFT. How to: Copy directories. Microsoft: Microsoft Learn [online]. [cit. 2023-03-11].
     * Dostupné z: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories.
     * Upraveno.
     */

    private static void RecursiveCopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        // Get information about the source directory
        var dir = new DirectoryInfo(sourceDir);

        // Cache directories before we start copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory(destinationDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        // If recursive and copying subdirectories, recursively call this method
        if (recursive)
        {
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                RecursiveCopyDirectory(subDir.FullName, newDestinationDir, true);
            }
        }
    }

    public static void OnMoveClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        //if (items.files == null) return;

        string destinationPath;
        do
        {
            destinationPath = GetPath("Move to...");
        } while (!Directory.Exists(destinationPath));

        foreach (var item in items.files)
        {
            if (item!.IsDirectory)
            {
                Directory.Move(item.Path, Path.Combine(destinationPath, item.Name));
            }
            else
            {
                File.Move(item.Path, Path.Combine(destinationPath, item.Name));
            }
        }

        Refresh();
    }

    public static void OnDeleteClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        //if (items.files == null) return;

        var consent = PromptConfirmDialogWindow.IsConfirmed();
        if (!consent) return;

        foreach (var item in items.files)
        {
            if (item!.IsDirectory)
            {
                Directory.Delete(item.Path, true);
            }
            else
            {
                File.Delete(item.Path);
            }
        }

        Refresh();
    }

    public static void OnRenameClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        //if (items.files == null) return;
        var root = GetFocusedWindow() == 1 ? LeftRoot : RightRoot;
        var newFilename = GetPath("Rename to...");

        var destinationPath = Path.Combine(root.ToString(), newFilename);
        Console.WriteLine(destinationPath);


        foreach (var item in items.files)
        {
            if (item!.IsDirectory)
            {
                Directory.Move(item.Path, Path.Combine(destinationPath, item.Name));
            }
            else
            {
                File.Move(item.Path, Path.Combine(destinationPath, item.Name));
            }
        }

        Refresh();
    }

    public static void OnExtractClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        //if (items.files == null) return;

        string destinationPath = GetPath("Extract to...");
        if (!Directory.Exists(destinationPath))
        {
            Directory.CreateDirectory(destinationPath);
        }

        foreach (var item in items.files)
        {
            if (!item!.IsDirectory && item.Name.EndsWith(".zip"))
            {
                ZipFile.ExtractToDirectory(item.Path, destinationPath);
            }

            Refresh();
        }
    }

    public static void OnCompressClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        //if (items.files == null) return;

        string destinationPath;
        do
        {
            destinationPath = GetPath("Copy to...");
        } while (!Directory.Exists(destinationPath));

        foreach (var item in items.files)
        {
            if (item!.IsDirectory)
            {
                ZipFile.CreateFromDirectory(item.Path, Path.Combine(destinationPath, item.Name + ".zip"));
            }
            else
            {
                //TODO
            }
        }

        Refresh();
    }

    private static void Refresh()
    {
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }
}