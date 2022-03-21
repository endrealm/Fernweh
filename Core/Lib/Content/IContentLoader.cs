using System;
using System.Collections.Generic;

namespace Core.Content;

public interface IContentLoader
{

    IArchiveHandler LoadArchive(string archiveFolder);
    
}


public interface IArchiveHandler: IDisposable
{
    string LoadFile(string file);

}