using System.Collections.Generic;

namespace Core.Scenes.Ingame.Battle;

public interface IFriendlyParticipantsProvider
{
    List<ParticipantConfig> Load();
}