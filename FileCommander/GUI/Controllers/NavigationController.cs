// ReSharper disable ObjectCreationAsStatement

namespace FileCommander.GUI.Controllers;

using System;
using System.Collections.Generic;
using System.IO;
using Gtk;
using static App;

public abstract class NavigationController
{
    public static DirectoryInfo OnHomeClicked(DirectoryInfo root, Stack<DirectoryInfo> history, ListStore store)
    {
        history.Push(root);

        root = new DirectoryInfo(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal));
        FillStore(store, root);
        return root;
    }

    public static DirectoryInfo OnUpClicked(DirectoryInfo root, Stack<DirectoryInfo> history, ListStore store)
    {
        history.Push(root);

        if (root.Parent == null)
            return root;

        FillStore(store, root.Parent);
        return root.Parent;
    }

    public static DirectoryInfo OnBackClicked(DirectoryInfo root, Stack<DirectoryInfo> history,
        Stack<DirectoryInfo> historyForward, ListStore store)
    {
        if (history.Count == 0)
            return root;

        historyForward.Push(root);
        FillStore(store, history.Peek());
        return history.Pop();
    }

    public static DirectoryInfo OnForwardClicked(DirectoryInfo root, Stack<DirectoryInfo> history,
        Stack<DirectoryInfo> historyForward, ListStore store)
    {
        if (historyForward.Count == 0)
            return root;

        history.Push(root);
        FillStore(store, historyForward.Peek());
        return historyForward.Pop();
    }

    public static void OnRefreshClicked(object sender, EventArgs e)
    {
        RefreshIconViews();
    }

    //Obnovení zobrazení položek v obou panelech
    public static void RefreshIconViews()
    {
        FillStore(LeftStore, LeftRoot);
        FillStore(RightStore, RightRoot);
    }
}