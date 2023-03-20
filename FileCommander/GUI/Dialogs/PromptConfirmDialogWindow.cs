using Gtk;

namespace FileCommander.GUI.Dialogs;

using static Settings;

public class PromptConfirmDialogWindow : Dialog
{
    private static Gtk.Dialog _dialog;
    private static bool _isConfirmed = false;
    private static string PromptSettingsKey { get; set; }

    public PromptConfirmDialogWindow(string title, string prompt, string promptSettingsKey)
    {
        PromptSettingsKey = promptSettingsKey;

        _dialog = new Gtk.Dialog(title, this, DialogFlags.DestroyWithParent, Stock.Cancel, ButtonsType.Cancel, Stock.Ok,
            ButtonsType.Ok);
        _dialog.Resizable = false;


        var requestLabel = new Label(prompt);
        _dialog.ContentArea.PackStart(requestLabel, true, true, 0);


        KeyPressEvent += (o, args) =>
        {
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    _isConfirmed = true;
                    break;
                case ConsoleKey.Escape:
                    _dialog.Destroy();
                    break;
            }
        };
        
        if (GetConf(promptSettingsKey))
        {
            var promptSettingsButton = new CheckButton("Don't ask again");
            promptSettingsButton.Toggled += OnToggle;
            _dialog.ContentArea.PackStart(promptSettingsButton, true, true, 0);
        }

        _dialog.Response += delegate(object _, ResponseArgs args)
        {
            if ((int) args.ResponseId == 1) //OK
            {
                _isConfirmed = true;
            }
        };


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
            SetConf(PromptSettingsKey, false);
        }
        else
        {
            SetConf(PromptSettingsKey, true);
        }
    }
}