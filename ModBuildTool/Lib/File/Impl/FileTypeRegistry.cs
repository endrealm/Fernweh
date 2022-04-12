using ModBuildTool.Lib.File.DataTypes;

namespace ModBuildTool.Lib.File.Impl;

public class FileTypeRegistry: IFileTypeRegistry
{

    private readonly Dictionary<string, Type> _stringToType = new();
    private readonly Dictionary<Type, string> _typeToString = new();


    public FileTypeRegistry()
    {
        Register<Image>("png");
        Register<Script>("lua");
        Register<Json>("json");
    }

    public IFileData CreateInstanceByFileEnding(string fileEnding, string rawData)
    {
        var type = _stringToType[fileEnding];
        if (type == null) throw new Exception("Unknown file ending");

        var obj = (IFileData?) Activator.CreateInstance(type, new object[] { rawData });

        if (obj == null) throw new Exception("Object instance could not be created");
        return obj;
    }

    public string GetFileEndingByType(Type type)
    {
        return _typeToString[type];
    }

    public void Register<TType>(string fileEnding) where TType : IFileData
    {
        // TODO: check if entry is already present
        _stringToType[fileEnding] = typeof(TType);
        _typeToString[typeof(TType)] = fileEnding;
    }
    
}