﻿using System.Collections.Generic;
using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Modes.Battle.Impl;
using NLua;

namespace Core.States;

public class LuaGlobalEventHandler : IGlobalEventHandler
{
    private readonly List<LuaFunction> _battleSpriteLoadListeners = new();
    private readonly List<LuaFunction> _postBattleListeners = new();
    private readonly List<LuaFunction> _postStateRenderEventListeners = new();
    private readonly List<LuaFunction> _prePlayerMoveEventListeners = new();
    private readonly List<LuaFunction> _preStateChangeEventListeners = new();
    private readonly List<LuaFunction> _preStateRenderEventListeners = new();

    public void EmitPreStateChangeEvent()
    {
        _preStateChangeEventListeners.ForEach(fun => fun.Call());
    }

    public void EmitPreStateRenderEvent(StateRenderer renderer, RenderContext renderContext)
    {
        _preStateRenderEventListeners.ForEach(fun => fun.Call(renderer, renderContext));
    }

    public void EmitPostStateRenderEvent(StateRenderer renderer, RenderContext renderContext)
    {
        _postStateRenderEventListeners.ForEach(fun => fun.Call(renderer, renderContext));
    }

    public void EmitLoadBattleSprites(DynamicBattleSpriteManager spriteManager)
    {
        _battleSpriteLoadListeners.ForEach(fun => fun.Call(spriteManager));
    }

    public void EmitPostBattle(bool victory, BattleSnapshot snapshot)
    {
        _postBattleListeners.ForEach(fun => fun.Call(victory, snapshot));
    }

    public void EmitPrePlayerMoveEvent()
    {
        _prePlayerMoveEventListeners.ForEach(fun => fun.Call());
    }

    public LuaGlobalEventHandler AddOnPrePlayerMove(LuaFunction listener)
    {
        _prePlayerMoveEventListeners.Add(listener);
        return this;
    }

    public LuaGlobalEventHandler AddOnPreStateChange(LuaFunction listener)
    {
        _preStateChangeEventListeners.Add(listener);
        return this;
    }

    public LuaGlobalEventHandler AddOnPreStateRender(LuaFunction listener)
    {
        _preStateRenderEventListeners.Add(listener);
        return this;
    }

    public LuaGlobalEventHandler AddOnPostStateRender(LuaFunction listener)
    {
        _postStateRenderEventListeners.Add(listener);
        return this;
    }

    public LuaGlobalEventHandler AddOnBattleSpriteLoad(LuaFunction listener)
    {
        _battleSpriteLoadListeners.Add(listener);
        return this;
    }

    public LuaGlobalEventHandler AddOnPostBattle(LuaFunction listener)
    {
        _postBattleListeners.Add(listener);
        return this;
    }
}