using System.Collections.Generic;
using Controllers.Character;

namespace Models
{
    public struct EndSessionModel
    {
        public readonly Dictionary<string,(PlayerDataModel, Character)> RedTeam;
        public readonly Dictionary<string,(PlayerDataModel, Character)> BlueTeam;

        public EndSessionModel(Dictionary<string, (PlayerDataModel, Character)> redTeam, Dictionary<string, (PlayerDataModel, Character)> blueTeam)
        {
            RedTeam = redTeam;
            BlueTeam = blueTeam;
        }
    }
}
