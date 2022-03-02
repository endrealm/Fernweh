using System;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class StaticContentEffect: ITextContentEffect
{
    public Action _onFinish { get; set; }
    private bool _firstFrame = true;

    public StaticContentEffect(Action onFinish = null)
    {
        _onFinish = onFinish;
    }

    public void Attach(TextComponent component)
    {
    }

    public void Update(float deltaTime, ChatUpdateContext context)
    {
        if(_firstFrame)
        {
            _onFinish?.Invoke();
            _firstFrame = false;
        }
    }
}