using System;
using System.Collections.Generic;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class StaticContentEffect: ITextContentEffect
{
    private readonly float _timePerParagraph;
    public List<Action> OnFinish { get; set; }

    private float _timeDone;
    private bool _done;

    public StaticContentEffect(float timerPerParagraph = 0f, Action onFinish = null)
    {
        _timePerParagraph = timerPerParagraph;
        OnFinish = new List<Action>();
        if(onFinish != null) OnFinish.Add(onFinish);
    }

    public void Attach(TextComponent component)
    {
    }

    public void Update(float deltaTime, ChatUpdateContext context)
    {
        if (_done) return;
        _timeDone += deltaTime;

        if(_timeDone > _timePerParagraph)
        {
            OnFinish?.ForEach(action => action?.Invoke());
            _done = true;
        }
    }
}