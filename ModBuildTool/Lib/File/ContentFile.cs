namespace ModBuildTool.Lib.File;


public class ContentFile
{
    
    private string _name;
    private IFileData _data;

    
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