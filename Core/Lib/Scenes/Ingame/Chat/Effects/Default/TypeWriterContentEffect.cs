using System;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class TypeWriterContentEffect: ITextContentEffect
{
    private readonly float _timePerParagraph;
    private readonly float _timePerCharacter;
    public Action OnFinish { get; set; }
    private TextComponent _component;
    
    private float _timeDone;
    private string _originalMessage = "";
    private int _index;
    private bool _done;

    public TypeWriterContentEffect(
        float timePerCharacter = .01f,
        float timePerParagraph = 0.3f,
        Action onFinish = null
    )
    {
        _timePerParagraph = timePerParagraph;
        _timePerCharacter = timePerCharacter;
        OnFinish = onFinish;
    }
    public void Attach(TextComponent component)
    {
        _component = component;
        _originalMessage = component.Message;
        _timeDone = 0;
        _index = 0;
        _done = false;
        component.ChangeMessage("");
    }

    public void Update(float deltaTime, ChatUpdateContext context)
    {
        if(_done) return;
        _timeDone += deltaTime;

        if(_index < _originalMessage.Length && _timeDone >= _timePerCharacter) // more characters to print + cleared char timer
        {
            _timeDone = 0;
            _component.ChangeMessage(_originalMessage.Substring(0, _index + 1));
            _index++;
        }
        else if (_timeDone >= _timePerParagraph) // no more char to print + cleared paragraph timer
        {
            _done = true;
            OnFinish?.Invoke();
        }
    }
}