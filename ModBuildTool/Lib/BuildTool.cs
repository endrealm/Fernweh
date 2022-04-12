using ModBuildTool.Lib.File;
using ModBuildTool.Lib.File.Impl;
using ModBuildTool.Lib.Input;
using ModBuildTool.Lib.Input.Impl;

namespace ModBuildTool.Lib;

public class BuildTool
{

    private readonly string _directory;
    
    private readonly IFileReader _fileReader;
    private readonly IFileTypeRegistry _typeRegistry;

    private readonly List<ContentFile> _files = new List<ContentFile>();

    public BuildTool(string directory)
    {
        _directory = directory;
        _fileReader = new FileReader();
        _typeRegistry = new FileTypeRegistry();
    }


    public void Load()
    {
        foreach (var filePath in _fileReader.ReadAllFiles(_directory))
        {
            var ending = filePath.Split(".").Last();
            try
            {
                var instance = _typeRegistry.CreateInstanceByFileEnding(ending, _fileReader.ReadFile(filePath));
                var file = new ContentFile( filePath.Remove(filePath.LastIndexOf('.')), instance);
                _files.Add(file);
            }
            catch
            {
                Console.WriteLine("File " + filePath + " has unknown format");
                continue;
            }
        }
    }


    public void Process()
    {
        
    }


    public void Export()
    {
        foreach (var contentFile in _files)
        {
            var type = contentFile.GetFileData().GetType();
            Console.WriteLine("Writing: " + contentFile.GetName() +  " type: " + _typeRegistry.GetFileEndingByType(type));
        }
    }

}