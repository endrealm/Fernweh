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
    private int _lastHealth = -1;
    private IChatComponent _mana;
    private int _lastMana = -1;
    private int _width;
    private readonly StatBar _healthBar = new(Color.Green, Color.Gray);
    private readonly StatBar _manaBar = new(Color.SteelBlue, Color.Gray);
    public PlayerStatsRow(IBattleParticipant target, IFontManager fontManager, DialogTranslationData translationData)
    {
        _target = target;
        _fontManager = fontManager;
        _translationData = translationData;
        _name = translationData.GetOrKey("battle.statsRow.name").Build(fontManager.GetChatFont(), new Replacement("name", target.DisplayName));
        RebuildHealth();
        RebuildMana();
    }

    private void RebuildHealth()
    {
        if(_target.Health == _lastHealth) return;
        _lastHealth = _target.Health;
        
        var current = _target.Health;
        var max = _target.GetStats().Health;
        
        _health = _translationData.GetOrKey("battle.statsRow.health")
            .Build(_fontManager.GetChatFont(), 
                new Replacement("current", current.ToString()),
                new Replacement("max", max.ToString())
            );
        
        _health.MaxWidth = _width;
        _healthBar.Percentage = current / (float) max;
    }
    private void RebuildMana()
    {
        if(_target.Mana == _lastMana) return;
        _lastMana = _target.Mana;
        
        var current = _target.Mana;
        var max = _target.GetStats().Mana;
        
        _mana = _translationData.GetOrKey("battle.statsRow.mana")
            .Build(_fontManager.GetChatFont(), 
                new Replacement("current", current.ToString()),
                new Replacement("max", max.ToString())
            );
        _mana.MaxWidth = _width;
        _manaBar.Percentage = current / (float) max;
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

        var partialWidth = _width / 3;
        RebuildHealth();
        RebuildMana();
        var start = new Vector2(context.XOffset, context.BaseScreenSize.Y- (context.TotalSlots - context.Slot) * _name.Dimensions.Y - 4);
        
        _name.Render(spriteBatch, new ChatRenderContext(start));
        var healthGridPos = start + new Vector2(partialWidth, 0);
        _health.Render(spriteBatch, new ChatRenderContext(healthGridPos));
        _healthBar.Render(spriteBatch, new BarRenderContext(healthGridPos + new Vector2(0, _health.Dimensions.Y), partialWidth-2));
        var manaGridPos = start + new Vector2(partialWidth, 0) * 2;
        _mana.Render(spriteBatch, new ChatRenderContext(manaGridPos));
        _manaBar.Render(spriteBatch, new BarRenderContext(manaGridPos + new Vector2(0, _mana.Dimensions.Y), partialWidth-2));
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