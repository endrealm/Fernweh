namespace Core.Scenes.Ingame.World
{
    public class MapTileData
    {
        public MapTileData(string _name, string _enterState = null, string _leaveState = null)
        {
            name = _name;
            enterState = (_enterState == null) ? "enter_" + _name : _enterState;
            leaveState = (_leaveState == null) ? "leave_" + _name : _leaveState;
        }

        public string name;
        public string enterState;
        public string leaveState;
    }
}
