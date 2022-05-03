namespace Core.Scripting.Saving;

public interface IDataLoadFunction
{
    void Load(IDataEncoder dataEncoder, object data);
}