namespace ModBuildTool.Lib.File;

public class ContentFile
{
    private readonly IFileData _data;

    private readonly string _name;


    public ContentFile(string name, IFileData data)
    {
        _name = name;
        _data = data;
    }

    public string GetName()
    {
        return _name;
    }

    public IFileData GetFileData()
    {
        return _data;
    }
}