using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Core.Content;

namespace CrossPlatformDesktop;

public class ContentLoader: IContentLoader
{
    
    public IArchiveHandler LoadArchive(string archiveFolder)
    {
        var file = File.OpenRead(archiveFolder);
        return new ArchiveHandler(new ZipArchive(file, ZipArchiveMode.Read), file);
    }
    
}


public class ArchiveHandler : IArchiveHandler
{

    private readonly ZipArchive _archive;
    private readonly FileStream _stream;

    public ArchiveHandler(ZipArchive archive, FileStream stream)
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
    
    public void Dispose()
    {
        this._archive.Dispose();
        this._stream.Dispose();
    }
    
}