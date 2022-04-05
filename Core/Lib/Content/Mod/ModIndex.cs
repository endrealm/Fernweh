using System;

namespace Core.Content.Mod;

public class ModIndex
{

    public string Id;
    public string[] Dependencies = Array.Empty<string>();
    public string[] Scripts = Array.Empty<string>();

}