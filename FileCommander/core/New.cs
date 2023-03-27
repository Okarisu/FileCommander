// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using System.Net;
using FileCommander.GUI.Controllers;
using FileCommander.GUI.Dialogs;

namespace FileCommander.core;

using GUI;
using static GUI.App;
using static NavigationController;

public partial class Core
{
    public static void OnNewClicked(object sender, EventArgs e)
    {
        var newFolderName = TargetController.GetTargetPath("New folder", false);

        var root = GetFocusedWindow() == 1 ? LeftRoot : RightRoot;

        var newDirectoryPath = Path.Combine(root.ToString(), newFolderName.path);
        if (Directory.Exists(newDirectoryPath))
        {
            new PromptUserDialogWindow("Folder with this name already exists.");
            return;
        }

        try
        {
            Directory.CreateDirectory(newDirectoryPath);
        }
        catch (ArgumentNullException)
        {
            new PromptUserDialogWindow("Folder name cannot be null.");
            return;
        }
        catch (PathTooLongException)
        {
            new PromptUserDialogWindow("The specified folder name exceeded the system-defined maximum length.");
            return;
        }
        catch (ArgumentException)
        {
            new PromptUserDialogWindow("Malformed folder name");
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
            new PromptUserDialogWindow("The specified folder name is in an invalid format.");
            return;
        }
        catch (Exception)
        {
            new PromptUserDialogWindow("Unknown error has occured.");
            return;
        }
        RefreshIconViews();
    }
}