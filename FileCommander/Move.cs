namespace FileCommander;

public class Move
{
    public static void MoveObject(string sourcePath, string targetPath)
    {
        //if (Path.HasExtension(sourcePath))
        if (File.Exists(sourcePath))
        {
            File.Move(sourcePath, targetPath);
        } else if (Directory.Exists(sourcePath))
        {
            Directory.Move(sourcePath, targetPath);
        }
    }

    public static void Rename(string sourcePath, string targetPath)
    {
        if (File.Exists(sourcePath))
        {
            File.Move(sourcePath, targetPath);
        } else if (Directory.Exists(sourcePath))
        {
            Directory.Move(sourcePath, targetPath);
        }

    }
    public static void Copy(string sourcePath, string targetPath)
    {
        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, targetPath);
        } else if (Directory.Exists(sourcePath))
        {
            
        }

    }
}