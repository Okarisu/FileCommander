// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable ClassNeverInstantiated.Global

using System.Data;

namespace FileCommander.core;

using System;
using System.IO;
using GUI;
using GUI.Controllers;
using GUI.Dialogs;
using static GUI.App;
using static GUI.Controllers.NavigationController;

public partial class Core
{
    public static void OnNewClicked(object sender, EventArgs e)
    {
        var newFolderName = TargetController.GetTargetPath("New folder", false);

        var root = GetFocusedPanel() == 1 ? LeftRoot : RightRoot;

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
        //Následující catch bloky a výjimky byly generovány GitHub Copilotem.
        //Řádky volání konstruktoru okna s chybovou hláškou jsou mým vlastním dílem.
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
        catch (ReadOnlyException)
        {
            new PromptUserDialogWindow("Directory is read-only.");
            return;
        }
        catch (IOException)
        {
            new PromptUserDialogWindow("Input/output error has occurred.");
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