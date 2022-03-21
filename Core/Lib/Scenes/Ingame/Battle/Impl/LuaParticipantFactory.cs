using System.Linq;
using NLua;

namespace Core.Scenes.Ingame.Battle.Impl;

public class LuaParticipantFactory: IParticipantFactory
{
    private readonly LuaFunction _producer;

    public LuaParticipantFactory(string id, LuaFunction producer)
    {
        _producer = producer;
        Id = id;
    }

    public string Id { get; }
    public ParticipantConfig Produce()
    {
        return (ParticipantConfig) _producer.Call(new ParticipantConfigBuilder(Id)).First();
    }
}