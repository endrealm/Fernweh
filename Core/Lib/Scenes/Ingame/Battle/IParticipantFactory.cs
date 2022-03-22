namespace Core.Scenes.Ingame.Battle;

public interface IParticipantFactory
{
    public string Id { get; }
    public ParticipantConfig Produce();
}