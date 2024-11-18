using System;
using System.IO;

namespace Core.Content;

public interface IArchiveLoader : IDisposable
{
    string LoadFile(string file);
    string[] LoadAllFiles(string name);

    Stream LoadFileAsStream(string file);
}