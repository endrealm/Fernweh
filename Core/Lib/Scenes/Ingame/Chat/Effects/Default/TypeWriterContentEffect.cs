using System;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class TypeWriterContentEffect: ITextContentEffect
{
    private readonly float _timePerCharacter;
    public Action _onFinish { get; set; }
    private TextComponent _component;
    
    private float timeDone;
    private string originalMessage = "";
    private int index;
    private bool done;

    public TypeWriterContentEffect(
        float timePerCharacter = .01f,
        Action onFinish = null
    )
    {
        _timePerCharacter = timePerCharacter;
        _onFinish = onFinish;
    }
    public void Attach(TextComponent component)
    {
        _component = component;
        originalMessage = component.Message;
        timeDone = 0;
        index = 0;
        done = false;
        component.ChangeMessage("");
    }

    public void Update(float deltaTime, ChatUpdateContext context)
    {
        if(done) return;
        timeDone += deltaTime;

        if (timeDone < _timePerCharacter)
        {
            return;
        }

        if(index >= originalMessage.Length)
        {
            done = true;
            _onFinish?.Invoke();
            return;
        }

        timeDone = 0;
        _component.ChangeMessage(originalMessage.Substring(0, index+1));
        index++;
    }
}