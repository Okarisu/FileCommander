using FileCommander.GUI.Dialogs;

namespace FileCommander.GUI.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using Gtk;
using static PromptPathInputDialogWindow;
using static App;

public abstract class TargetController
{
    public static (string root, bool cancel) GetTargetPanel(string operation)
    {
        var promptedTargetPanel = new PromptTargetPanelDialogWindow(operation).GetTargetPanel();

        string root;
        if (promptedTargetPanel.targetHere)
        {
            //Cíl je v aktivním panelu
            root = (GetFocusedPanel() == 1 ? LeftRoot : RightRoot).ToString();
        }
        else
        {
            //Cíl je ve vedlejším panelu
            root = (GetFocusedPanel() == 1 ? RightRoot : LeftRoot).ToString();
        }

        var cancel = promptedTargetPanel.cancel;
        PromptTargetPanelDialogWindow.NullPrompt();
        
        return (root, cancel);
    }

    public static (string path, bool cancel, bool addSuffix) GetTargetPath(string dialogTitle, bool promptSuffix)
    {
        (string path, bool cancel, bool addSuffix) path;
        do
        {
            new PromptPathInputDialogWindow(dialogTitle, promptSuffix);
            path = GetPath();
            NullPath();
        } while (path.path == "" && !path.cancel);

        foreach (var c in Path.GetInvalidFileNameChars())
        {
            if (!path.path.Contains(c)) continue;
            new PromptUserDialogWindow("Invalid character in file name.");
            return GetTargetPath(dialogTitle, promptSuffix);
        }

        return (path.path, path.cancel, path.addSuffix);
    }
}