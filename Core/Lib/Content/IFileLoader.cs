using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Content;


public interface IArchiveLoader: IDisposable
{
    string LoadFile(string file);

    Stream LoadFileAsStream(string file);

}