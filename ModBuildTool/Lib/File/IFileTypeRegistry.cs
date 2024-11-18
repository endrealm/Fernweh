namespace ModBuildTool.Lib.File;

public interface IFileTypeRegistry
{
    IFileData CreateInstanceByFileEnding(string fileEnding, string rawData);

    string GetFileEndingByType(Type type);
}