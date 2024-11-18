using Aspose.Zip;
using Aspose.Zip.Saving;
using ModBuildTool.Lib.File;
using ModBuildTool.Lib.File.Impl;
using ModBuildTool.Lib.Input;
using ModBuildTool.Lib.Input.Impl;
using ModBuildTool.Lib.Util;

namespace ModBuildTool.Lib;

public class BuildTool
{
    private readonly string _directory;

    private readonly IFileReader _fileReader;

    private readonly List<ContentFile> _files = new();
    private readonly IFileTypeRegistry _typeRegistry;

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
                var file = new ContentFile(filePath.Remove(filePath.LastIndexOf('.')).Remove(0, _directory.Length),
                    instance);
                _files.Add(file);
            }
            catch
            {
                Console.WriteLine("File " + filePath + " has unknown format");
            }
        }
    }


    public void Process()
    {
    }


    public void Export()
    {
        var disposables = new List<IDisposable>();

        using (var zipFile = System.IO.File.Open(_directory + "mod.fwm", FileMode.Create))
        {
            using (var archive = new Archive(new ArchiveEntrySettings()))
            {
                foreach (var contentFile in _files)
                {
                    var source = StreamUtil.FromString(contentFile.GetFileData().GetRaw());
                    disposables.Add(source);

                    var fileEnding = _typeRegistry.GetFileEndingByType(contentFile.GetFileData().GetType());
                    Console.WriteLine(contentFile.GetName() + "." + fileEnding);
                    archive.CreateEntry(contentFile.GetName() + "." + fileEnding,
                        source);
                }

                archive.Save(zipFile);
            }
        }

        foreach (var disposable in disposables) disposable.Dispose();
    }
}