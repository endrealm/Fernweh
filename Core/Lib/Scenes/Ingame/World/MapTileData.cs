using System;

namespace Core.Scenes.Ingame.World;

[Serializable]
public class MapTileData
{
    public string enterState;
    public string firstEnterState;
    public string lastLeaveState;
    public string leaveState;
    public string name;
}