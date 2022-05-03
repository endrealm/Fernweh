namespace Core.Scripting.Saving;

public interface IDataSaveFunction
{
    object Save(IDataEncoder dataEncoder);
}