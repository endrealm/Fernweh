using System.Collections.Generic;

namespace Core.Content;

public interface IContentLoader
{

    IArchiveHandler LoadArchive(string archiveFolder);
    
}


public interface IArchiveHandler
{

    void Dispose();

    string LoadFile(string file);

}