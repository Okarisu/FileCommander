using Gtk;

namespace FileCommander.GUI;

public class IconWidget : Widget
{
    public IconWidget()
    {
        Gdk.Pixbuf icon = new Gdk.Pixbuf("icons/icon_move.png");
        Image image = new Image(icon);
        image.Show();
    }
}
