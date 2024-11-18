using Core.Scenes.Ingame.Battle;
using Core.Scenes.Ingame.Chat;
using Core.Scenes.Ingame.Localization;
using Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PipelineExtensionLibrary.Tokenizer.Chat;

namespace Core.Scenes.Ingame.Modes.Battle;

public class PlayerStatsRow : IRenderer<PlayerStatsRowRenderContext>
{
    private readonly IFontManager _fontManager;
    private readonly StatBar _healthBar = new(Color.Green, Color.Gray);
    private readonly ILocalizationManager _localizationManager;
    private readonly StatBar _manaBar = new(Color.SteelBlue, Color.Gray);
    private readonly IChatComponent _name;
    private readonly IBattleParticipant _target;
    private IChatComponent _health;
    private int _lastHealth = -1;
    private int _lastMana = -1;
    private IChatComponent _mana;
    private int _width;

    public PlayerStatsRow(IBattleParticipant target, IFontManager fontManager, ILocalizationManager localizationManager)
    {
        _target = target;
        _fontManager = fontManager;
        _localizationManager = localizationManager;
        _name = _localizationManager.GetData("battle.statsRow.name", new TextReplacement("name", target.DisplayName))
            .Compile().Build(fontManager.GetChatFont());
        RebuildHealth();
        RebuildMana();
    }


    public void Render(SpriteBatch spriteBatch, PlayerStatsRowRenderContext context)
    {
        if (_width != context.Width)
        {
            _width = context.Width;
            var piece = _width / 3;
            _name.MaxWidth = piece;
            _health.MaxWidth = piece;
            _mana.MaxWidth = piece;
        }

        var partialWidth = _width / 3;
        RebuildHealth();
        RebuildMana();
        var start = new Vector2(context.XOffset,
            context.BaseScreenSize.Y - (context.TotalSlots - context.Slot) * _name.Dimensions.Y - 4);

        _name.Render(spriteBatch, new ChatRenderContext(start));
        var healthGridPos = start + new Vector2(partialWidth, 0);
        _health.Render(spriteBatch, new ChatRenderContext(healthGridPos));
        _healthBar.Render(spriteBatch,
            new BarRenderContext(healthGridPos + new Vector2(0, _health.Dimensions.Y), partialWidth - 2));
        var manaGridPos = start + new Vector2(partialWidth, 0) * 2;
        _mana.Render(spriteBatch, new ChatRenderContext(manaGridPos));
        _manaBar.Render(spriteBatch,
            new BarRenderContext(manaGridPos + new Vector2(0, _mana.Dimensions.Y), partialWidth - 2));
    }

    private void RebuildHealth()
    {
        if (_target.Health == _lastHealth) return;
        _lastHealth = _target.Health;

        var current = _target.Health;
        var max = _target.GetStats().Health;

        _health = _localizationManager.GetData(
                "battle.statsRow.health",
                new TextReplacement("current", current.ToString()),
                new TextReplacement("max", max.ToString())
            )
            .Compile()
            .Build(_fontManager.GetChatFont());

        _health.MaxWidth = _width;
        _healthBar.Percentage = current / (float) max;
    }

    private void RebuildMana()
    {
        if (_target.Mana == _lastMana) return;
        _lastMana = _target.Mana;

        var current = _target.Mana;
        var max = _target.GetStats().Mana;

        _mana = _localizationManager.GetData(
                "battle.statsRow.mana",
                new TextReplacement("current", current.ToString()),
                new TextReplacement("max", max.ToString())
            )
            .Compile()
            .Build(_fontManager.GetChatFont());
        _mana.MaxWidth = _width;
        _manaBar.Percentage = current / (float) max;
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