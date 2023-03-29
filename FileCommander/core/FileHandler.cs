namespace FileCommander.core;

public class FileHandler
{
    private string Source { get; set; }
    private string Destination { get; set; }
    public FileHandler(string source, string destination)
    {
        this.Source = source;
        this.Destination = destination;
    }
    
    public void Copy()
    {
        try
        {
            File.Copy(Source, Destination);
        }
        catch (Exception e)
        {
            throw new Exception();
        }
    }
}