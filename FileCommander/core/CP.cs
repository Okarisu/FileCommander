namespace FileCommander.core;

public delegate void ProgressChangeDelegate(double Percentage);
public delegate void Completedelegate();

class CustomFileCopier
{
    public CustomFileCopier(string Source, string Dest)
    {
        this.SourceFilePath = Source;
        this.DestFilePath = Dest;

        OnProgressChanged += delegate { };
        OnComplete += delegate { };
    }

    public void Copy()
    {
        byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
        bool cancelFlag = false;

        using (FileStream source = new FileStream(SourceFilePath, FileMode.Open, FileAccess.Read))
        {
            long fileLength = source.Length;
            using (FileStream dest = new FileStream(DestFilePath, FileMode.CreateNew, FileAccess.Write))
            {
                long totalBytes = 0;
                int currentBlockSize = 0;

                while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    totalBytes += currentBlockSize;
                    double percentage = (double)totalBytes * 100.0 / fileLength;

                    dest.Write(buffer, 0, currentBlockSize);

                    cancelFlag = false;
                    OnProgressChanged(percentage);

                    if (cancelFlag)
                    {
                        File.Delete(DestFilePath);
                        break;
                    }
                }
            }
        }

        OnComplete();
    }

    public string SourceFilePath { get; set; }
    public string DestFilePath { get; set; }
    public double Progress { get; set; }

    public static event ProgressChangeDelegate OnProgressChanged;
    public event Completedelegate OnComplete;
}
