using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Chat;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PipelineExtensionLibrary;

namespace Core.Scenes.Ingame.Modes.Battle;

public class PlayerStatsRow: IRenderer<PlayerStatsRowRenderContext>
{
    private readonly IBattleParticipant _target;
    private readonly IFontManager _fontManager;
    private readonly DialogTranslationData _translationData;
    private readonly IChatComponent _name;
    private IChatComponent _health;
    private int _lastHealth;
    private IChatComponent _mana;
    private int _lastMana;
    private int _width;

    public PlayerStatsRow(IBattleParticipant target, IFontManager fontManager, DialogTranslationData translationData)
    {
        _target = target;
        _fontManager = fontManager;
        _translationData = translationData;
        _name = translationData.GetOrKey("battle.statsRow.name").Build(fontManager.GetChatFont(), new Replacement("name", target.ParticipantId));
        RebuildHealth();
        RebuildMana();
    }

    private void RebuildHealth()
    {
        if(_target.Health == _lastHealth) return;
        _lastHealth = _target.Health;
        _health = _translationData.GetOrKey("battle.statsRow.health")
            .Build(_fontManager.GetChatFont(), 
                new Replacement("current", _target.Health.ToString()),
                new Replacement("max", _target.GetStats().Health.ToString())
            );
        _health.MaxWidth = _width;

    }
    private void RebuildMana()
    {
        if(_target.Mana == _lastMana) return;
        _lastMana = _target.Mana;
        _mana = _translationData.GetOrKey("battle.statsRow.mana")
            .Build(_fontManager.GetChatFont(), 
                new Replacement("current", _target.Mana.ToString()),
                new Replacement("max", _target.GetStats().Mana.ToString())
            );
        _mana.MaxWidth = _width;
    }


    public void Render(SpriteBatch spriteBatch, PlayerStatsRowRenderContext context)
    {
        if (_width != context.Width)
        {
            _width = context.Width;
            var piece = _width/3;
            _name.MaxWidth = piece;
            _health.MaxWidth = piece;
            _mana.MaxWidth = piece;
        }
        RebuildHealth();
        RebuildMana();
        var start = new Vector2(context.XOffset, context.BaseScreenSize.Y- (context.TotalSlots - context.Slot) * _name.Dimensions.Y);
        
        _name.Render(spriteBatch, new ChatRenderContext(start));
        _health.Render(spriteBatch, new ChatRenderContext(start + new Vector2( _width/3, 0)));
        _mana.Render(spriteBatch, new ChatRenderContext(start + new Vector2( _width/3, 0) * 2));
    }
}

public class PlayerStatsRowRenderContext : IRenderContext
{
    public PlayerStatsRowRenderContext(int slot, int totalSlots, Vector2 baseScreenSize, int xOffset, int width)
    {
        Slot = slot;
        TotalSlots = totalSlots;
        BaseScreenSize = baseScreenSize;
        XOffset = xOffset;
        Width = width;
    }

    public int Slot { get; }
    public int TotalSlots { get; }
    public Vector2 BaseScreenSize { get; }
    public int XOffset { get; }
    public int Width { get; }
}