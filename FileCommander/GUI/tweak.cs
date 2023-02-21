/*using System.Runtime.CompilerServices;
using GLib;
using Gtk;
using wn = Gtk.Window.Dialog;

namespace FileCommander.GUI;

public class FileChooser : Window
{
    
    public FileChooser(
        string title,
        Window parent,
        FileChooserAction action,
        params object[] button_data)
        : base(IntPtr.Zero)
    {
        if (this.GetType() != typeof (FileChooserDialog))
        {
            this.CreateNativeObject(new string[0], new Value[0]);
            this.Title = title;
            if (parent != null)
                this.TransientFor = parent;
            this.Action = action;
        }
        else
        {
            IntPtr ptrGstrdup = Marshaller.StringToPtrGStrdup(title);
            this.Raw = FileChooserDialog.gtk_file_chooser_dialog_new(ptrGstrdup, parent == null ? IntPtr.Zero : parent.Handle, (int) action, IntPtr.Zero);
            Marshaller.Free(ptrGstrdup);
        }
        for (int index = 0; index < button_data.Length - 1; index += 2)
            this.AddButton((string) button_data[index], (int) button_data[index + 1]);
    }


    public static void Neco()
    {
Dialog dial = new Dialog("Pls", )
    }
}*/