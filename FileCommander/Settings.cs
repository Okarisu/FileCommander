namespace FileCommander;

public class Settings
{
    /*
     * STACK OVERFLOW. How to write data to yaml file. [online]. [cit. 2023-03-19].
     * Dostupné z: https://stackoverflow.com/questions/62371078/how-to-write-data-to-yaml-file
     * Upraveno.
     */

    private static readonly string ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.yaml");

    public static bool GetConf(string key)
    {
        try
        {
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            using var reader = new StreamReader(ConfigFilePath);
            var obj = deserializer.Deserialize<Dictionary<object, object>>(reader);
            var config = (Dictionary<object, object>) obj["settings"];
            reader.Close();

            return (string) config[key] == "true";
        }
        catch (FileLoadException)
        {
            throw new FileLoadException();
        }
        catch (Exception e)
        {
            throw new Exception();
        }
    }

    public static string GetConfStr(string key)
    {
        try
        {
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            using var reader = new StreamReader(ConfigFilePath);
            var obj = deserializer.Deserialize<Dictionary<object, object>>(reader);
            var config = (Dictionary<object, object>) obj["settings"];
            reader.Close();

            return config[key].ToString();
        }
        catch (FileLoadException)
        {
            throw new FileLoadException();
        }
        catch (Exception e)
        {
            throw new Exception();
        }
    }

    public static void SetConf(string key, bool value)
    {
        try
        {
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            using var reader = new StreamReader(ConfigFilePath);
            var obj = deserializer.Deserialize<Dictionary<object, object>>(reader);
            var config = (Dictionary<object, object>) obj["settings"];
            reader.Close();

            using var writer = new StreamWriter(ConfigFilePath);
            var serializer = new YamlDotNet.Serialization.Serializer();
            config[key] = value;

            serializer.Serialize(writer, obj);
        }
        catch (FileLoadException)
        {
            throw new FileLoadException();
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }
}