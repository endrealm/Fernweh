namespace Core.Utils;

public interface IUpdate<T> where T: IUpdateContext
{
    void Update(float deltaTime, T context);
}