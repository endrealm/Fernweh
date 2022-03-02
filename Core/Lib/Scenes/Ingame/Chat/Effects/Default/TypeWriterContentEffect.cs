using System;

namespace Core.Scenes.Ingame.Chat.Effects.Default;

public class TypeWriterContentEffect: ITextContentEffect
{
    private readonly float _timePerParagraph;
    private readonly float _timePerCharacter;
    public Action _onFinish { get; set; }
    private TextComponent _component;
    
    private float timeDone;
    private string originalMessage = "";
    private int index;
    private bool done;

    public TypeWriterContentEffect(
        float timePerCharacter = .01f,
        float timePerParagraph = 0.3f,
        Action onFinish = null
    )
    {
        _timePerParagraph = timePerParagraph;
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

        if(index < originalMessage.Length && timeDone >= _timePerCharacter) // more characters to print + cleared char timer
        {
            timeDone = 0;
            _component.ChangeMessage(originalMessage.Substring(0, index + 1));
            index++;
        }
        else if (timeDone >= _timePerParagraph) // no more char to print + cleared paragraph timer
        {
            done = true;
            _onFinish?.Invoke();
        }
    }
}