namespace ModBuildTool.Lib.Input;

public interface IFileReader
{

    string ReadFile(string path);

    string[] ReadAllFiles(string directory);

}