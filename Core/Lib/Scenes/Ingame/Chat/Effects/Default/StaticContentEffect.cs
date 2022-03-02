using System;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class StaticContentEffect: ITextContentEffect
{
    private readonly float _timePerParagraph;
    public Action _onFinish { get; set; }

    private float timeDone;
    private bool done;

    public StaticContentEffect(float timerPerParagraph = 0.3f, Action onFinish = null)
    {
        _timePerParagraph = timerPerParagraph;
        _onFinish = onFinish;
    }

    public void Attach(TextComponent component)
    {
    }

    public void Update(float deltaTime, ChatUpdateContext context)
    {
        if (done) return;
        timeDone += deltaTime;

        if(timeDone > _timePerParagraph)
        {
            _onFinish?.Invoke();
            done = true;
        }
    }
}