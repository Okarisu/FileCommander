// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using FileCommander.GUI;
using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;
using static FileCommander.GUI.Controllers.TargetController;

namespace FileCommander.core;

using static App;
using static NavigationController;

public partial class Core
{
    public static void OnRenameClicked(object sender, EventArgs e)
    {
        var items = GetSelectedItems();
        if (items.Length == 0)
        {
            new PromptUserDialogWindow("No files selected.");
            return;
        }

        var root = GetFocusedWindow() == 1 ? App.LeftRoot : RightRoot;
        (string Name, bool Cancel, bool addSuffix) newFilename;
        if (items.Length == 1)
        {
            newFilename = GetTargetPath("Rename to...", false);
        }
        else
        {
            newFilename = GetTargetPath("Rename to...", true);
        }

        if (newFilename.Cancel)
        {
            return;
        }

        var destinationPath = (GetFocusedWindow() == 1 ? App.LeftRoot : RightRoot).ToString();

        var fileSuffixes = new Queue<int>();
        var folderSuffixes = new Queue<int>();
        if (newFilename.addSuffix)
        {
            for (var i = 1; i <= items.Length; i++)
            {
                fileSuffixes.Enqueue(i);
                folderSuffixes.Enqueue(i);
            }
        }

        foreach (var item in items)
        {
            if(item.Path.Contains(Directory.GetCurrentDirectory()))
            {
                new PromptUserDialogWindow("Cannot rename system files.");
                continue;
            }

            if (item!.IsDirectory)
            {
                var childDestinationPath = Path.Combine(destinationPath, newFilename.Name);
                if (newFilename.addSuffix)
                {
                    childDestinationPath += "_" + folderSuffixes.Dequeue();
                }

                try
                {
                    Directory.Move(item.Path, childDestinationPath);
                }
                catch (ArgumentNullException)
                {
                    new PromptUserDialogWindow("Name cannot be null.");
                    return;
                }
                catch (PathTooLongException)
                {
                    new PromptUserDialogWindow("The specified name exceeded the system-defined maximum length.");
                    return;
                }
                catch (ArgumentException)
                {
                    new PromptUserDialogWindow("Malformed name");
                    return;
                }
                catch (IOException)
                {
                    new PromptUserDialogWindow("Parent directory is read-only.");
                    return;
                }
                catch (UnauthorizedAccessException)
                {
                    new PromptUserDialogWindow("Access to the path is denied.");
                    return;
                }
                catch (NotSupportedException)
                {
                    new PromptUserDialogWindow("The specified name is in an invalid format.");
                    return;
                }
                catch (Exception)
                {
                    new PromptUserDialogWindow("Unknown error has occured.");
                    return;
                }
            }
            else
            {
                var cleanFilename = item.Name!.Split('.'); //rozdělení jména souboru a koncovky
                var childDestinationPath = Path.Combine(destinationPath, newFilename.Name + "." +
                                                                         cleanFilename[cleanFilename.Length - 1]);
                if (newFilename.addSuffix)
                {
                    childDestinationPath = Path.Combine(destinationPath,
                        newFilename.Name + "_" + fileSuffixes.Dequeue() + "." +
                        cleanFilename[1]);
                }

                try
                {
                    File.Move(item.Path, childDestinationPath);
                }
                catch (ArgumentNullException)
                {
                    new PromptUserDialogWindow("Name cannot be null.");
                    return;
                }
                catch (PathTooLongException)
                {
                    new PromptUserDialogWindow("The specified name exceeded the system-defined maximum length.");
                    return;
                }
                catch (ArgumentException)
                {
                    new PromptUserDialogWindow("Malformed name");
                    return;
                }
                catch (IOException)
                {
                    new PromptUserDialogWindow("Parent directory is read-only.");
                    return;
                }
                catch (UnauthorizedAccessException)
                {
                    new PromptUserDialogWindow("Access to the path is denied.");
                    return;
                }
                catch (NotSupportedException)
                {
                    new PromptUserDialogWindow("The specified name is in an invalid format.");
                    return;
                }
                catch (Exception)
                {
                    new PromptUserDialogWindow("Unknown error has occured.");
                    return;
                }
            }
        }

        RefreshIconViews();
    }
}