namespace FileCommander.GUI.Dialogs;

using System;
using System.IO;
using Gtk;
using static Settings;

public class PromptConfirmDialogWindow : Dialog
{
    private static bool _isConfirmed;
    private static string? PromptSettingsKey { get; set; }

    /*
     * Při tvorbě jsem se inspiroval zde:
     * Dialogs in GTK#: Message dialogs. ZetCode [online]. [cit. 2023-04-02].
     * Dostupné z: https://zetcode.com/gui/gtksharp/dialogs/
     * Upraveno.
     */
    public PromptConfirmDialogWindow(string title, string prompt, string? promptSettingsKey)
    {
        PromptSettingsKey = promptSettingsKey;

        var dialog = new Dialog(title, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel, Stock.Ok,
            ButtonsType.Ok);
        dialog.Resizable = false;


        var requestLabel = new Label(prompt);
        dialog.ContentArea.PackStart(requestLabel, true, true, 0);
     
        
        if (GetConf(promptSettingsKey))
        {
            var promptSettingsButton = new CheckButton("Don't ask again");
            promptSettingsButton.Toggled += OnToggle;
            dialog.ContentArea.PackStart(promptSettingsButton, true, true, 0);
        }

        dialog.Response += delegate(object _, ResponseArgs args)
        {
            if ((int) args.ResponseId == 1) //OK
            {
                _isConfirmed = true;
            }
        };


        dialog.ShowAll();
        dialog.Run();
        dialog.Destroy();
    }

    public static bool IsConfirmed()
    {
        var consent = _isConfirmed;
        _isConfirmed = false;
        return consent;
    }

    //Následující funkce byla generována GitHub Copilotem
    private void OnToggle(object sender, EventArgs args)
    {
        var button = (CheckButton) sender;

        if (button.Active)
        {
            SetConf(PromptSettingsKey, false);
        }
        else
        {
            SetConf(PromptSettingsKey, true);
        }
    }
}