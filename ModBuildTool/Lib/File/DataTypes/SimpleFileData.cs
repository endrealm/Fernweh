namespace ModBuildTool.Lib.File.DataTypes;

public abstract class SimpleFileData : IFileData
{
    protected string RawData;

    protected SimpleFileData(string rawData)
    {
        RawData = rawData;
    }

    public string GetRaw()
    {
        return RawData;
    }
}