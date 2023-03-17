namespace FileCommander.GUI;

using Gtk;
using System.Configuration;
using System.Collections.Specialized;
using static Settings;

public class PromptConfirmDialogWindow : Dialog
{
    private static Dialog _dialog;
    private static bool _isConfirmed = false;
    private static string PromptSettingsKey { get; set; }

    public PromptConfirmDialogWindow(string title, string prompt, string promptSettingsKey)
    {
        PromptSettingsKey = promptSettingsKey;

        _dialog = new Dialog(title, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel, Stock.Ok,
            ButtonsType.Ok);
        _dialog.Resizable = false;
        _dialog.DefaultSize = new Gdk.Size(150, 100);

        var requestLabel = new Label(prompt);
        _dialog.ContentArea.PackStart(requestLabel, true, true, 0);


        _dialog.Response += delegate(object _, ResponseArgs args)
        {
            if ((int) args.ResponseId == 1) //OK
            {
                _isConfirmed = true;
            }
        };
        /*
        if (GetBoolValueSetting(promptSettingsKey))
        {
            var promptSettingsButton = new CheckButton("Don't ask again");
            promptSettingsButton.Toggled += OnToggle;
        }*/

        _dialog.ShowAll();
        _dialog.Run();
        _dialog.Destroy();
    }

    public static bool IsConfirmed()
    {
        var consent = _isConfirmed;
        _isConfirmed = false;
        return consent;
    }

    private void OnToggle(object sender, EventArgs args)
    {
        var button = (CheckButton) sender;

        if (button.Active)
        {
            SetBoolValueSetting(PromptSettingsKey, false);
        }
    }
}