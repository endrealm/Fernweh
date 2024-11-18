using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Core.Content;

namespace CrossPlatformDesktop;

public class FileLoader
{
    public IArchiveLoader LoadArchive(string archiveFolder)
    {
        var file = File.OpenRead(archiveFolder);
        return new ArchiveLoader(new ZipArchive(file, ZipArchiveMode.Read), file);
    }

    public IArchiveLoader LoadDirectory(string directory)
    {
        return new DirectoryLoader(directory);
    }
}

public class ArchiveLoader : IArchiveLoader
{
    private readonly ZipArchive _archive;
    private readonly FileStream _stream;

    public ArchiveLoader(ZipArchive archive, FileStream stream)
    {
        _archive = archive;
        _stream = stream;
    }

    public string LoadFile(string file)
    {
        var entry = _archive.GetEntry(file);
        if (entry == null) throw new Exception("File " + file + " is missing");
        using (var sr = new StreamReader(entry.Open()))
        {
            return sr.ReadToEnd();
        }
    }

    public string[] LoadAllFiles(string name)
    {
        var files = new List<string>();

        foreach (var entry in _archive.Entries)
            if (entry.Name.ToLower() == name)
                files.Add(entry.Name);
        Console.WriteLine(files.Count);

        return files.ToArray();
    }

    public Stream LoadFileAsStream(string file)
    {
        var entry = _archive.GetEntry(file);
        if (entry == null) throw new Exception("File " + file + " is missing");
        return entry.Open();
    }

    public void Dispose()
    {
        _archive.Dispose();
        _stream.Dispose();
    }
}

public class DirectoryLoader : IArchiveLoader
{
    private readonly string _dir;

    public DirectoryLoader(string dir)
    {
        _dir = dir;
    }

    public void Dispose()
    {
    }

    public string LoadFile(string file)
    {
        return File.ReadAllText(GetPath(file));
    }

    public string[] LoadAllFiles(string path)
    {
        var dir = new DirectoryInfo(_dir);
        return dir.EnumerateFiles(path, SearchOption.AllDirectories)
            .Select(x => Path.GetRelativePath(_dir, x.FullName))
            .ToArray();
    }

    public Stream LoadFileAsStream(string file)
    {
        return File.OpenRead(GetPath(file));
    }

    private string GetPath(string file)
    {
        return _dir + "/" + file;
    }
}