using System;
using Core.Scenes.Ingame.Chat;
using Core.Utils;

namespace Core.Scenes.Ingame.Modes;

public interface IChatView: IRenderer<IngameRenderContext>, IUpdate<IngameUpdateContext>, ILoadable
{
    public IChatComponent AddText(string key, Action callback = null, params Replacement[] replacements);
    public IChatComponent AddText(string key, params Replacement[] replacements);
    public IChatComponent AddAction(string key, Action callback, params Replacement[] replacements);
    void ForceLoadNext();
    void Clear();
}