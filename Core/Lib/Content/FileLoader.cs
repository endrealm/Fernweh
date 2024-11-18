namespace Core.Content;

public class FileLoader : ILoader<string>
{
    public string Load(string file, IArchiveLoader archiveLoader)
    {
        return archiveLoader.LoadFile(file);
    }
}