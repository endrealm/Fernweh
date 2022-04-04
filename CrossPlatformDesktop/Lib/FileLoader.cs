using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
        this._archive = archive;
        this._stream = stream;
    }

    public string LoadFile(string file)
    {
        var entry = this._archive.GetEntry(file);
        if (entry == null) throw new Exception("File " + file + " is missing");
        using (var sr = new StreamReader(entry.Open()))
        {
            return sr.ReadToEnd();
        }
    }

    public Stream LoadFileAsStream(string file)
    {
        var entry = this._archive.GetEntry(file);
        if (entry == null) throw new Exception("File " + file + " is missing");
        return entry.Open();
    }

    public void Dispose()
    {
        this._archive.Dispose();
        this._stream.Dispose();
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

    public Stream LoadFileAsStream(string file)
    {
        return System.IO.File.OpenRead(GetPath(file));
    }

    private string GetPath(string file)
    {
        return _dir + "/" + file;
    }
}