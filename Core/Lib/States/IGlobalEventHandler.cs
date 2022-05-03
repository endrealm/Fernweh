using Core.Scenes.Ingame.Modes.Battle.Impl;

namespace Core.States;

public interface IGlobalEventHandler
{
    void EmitPreStateChangeEvent();
    void EmitPrePlayerMoveEvent();
    void EmitPreStateRenderEvent(StateRenderer renderer, RenderContext renderContext);
    void EmitPostStateRenderEvent(StateRenderer renderer, RenderContext renderContext);
    void EmitLoadBattleSprites(DynamicBattleSpriteManager dynamicBattleSpriteManager);
}