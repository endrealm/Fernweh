using System;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class StaticContentEffect: ITextContentEffect
{
    public Action _onFinish { get; set; }

    public StaticContentEffect(Action onFinish = null)
    {
        _onFinish = onFinish;
    }

    public void Attach(TextComponent component)
    {
        _onFinish?.Invoke();
    }

    public void Update(float deltaTime, ChatUpdateContext context)
    {
    }
}