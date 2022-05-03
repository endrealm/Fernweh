namespace Core.Scripting.Saving;

public interface IDataEncoder
{
    public object DecodeData(object rawData);
    public object EncodeData(object rawData);
}