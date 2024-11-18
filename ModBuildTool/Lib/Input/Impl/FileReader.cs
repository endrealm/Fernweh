namespace ModBuildTool.Lib.Input.Impl;

public class FileReader : IFileReader
{
    public string ReadFile(string file)
    {
        return System.IO.File.ReadAllText(file);
    }

    public string[] ReadAllFiles(string directory)
    {
        return Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
    }
}