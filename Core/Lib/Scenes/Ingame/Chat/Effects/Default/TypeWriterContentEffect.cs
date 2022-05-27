using Core.Utils;
using System;
using System.Collections.Generic;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class TypeWriterContentEffect: ITextContentEffect
{
    private readonly float _timePerParagraph;
    private readonly float _timePerCharacter;
    public List<Action> OnFinish { get; set; } = new List<Action>();
    private TextComponent _component;
    
    private float _timeDone;
    private string _originalMessage = "";
    private int _index;
    private bool _done;

    public TypeWriterContentEffect(
        float timePerParagraph = 0f,
        Action onFinish = null
    )
    {
        _timePerParagraph = timePerParagraph;
        _timePerCharacter = GameSettings.Instance.TypingSpeed;
        if(onFinish != null)
        {
            OnFinish.Add(onFinish);
        }
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
            int charCount = _timePerCharacter == 0 ? _originalMessage.Length - _index : (int)Math.Round(deltaTime / _timePerCharacter);
            _component.ChangeMessage(_originalMessage.Substring(0, _index + Math.Min(charCount, _originalMessage.Length - _index)));
            _index++;
        }
        else if (_index >= _originalMessage.Length && _timeDone >= _timePerParagraph) // no more char to print + cleared paragraph timer
        {
            _done = true;
            OnFinish.ForEach(action => action.Invoke());
        }
    }
}