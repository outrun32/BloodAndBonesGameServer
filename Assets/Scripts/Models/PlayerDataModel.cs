namespace Models
{
    public class PlayerDataModel
    {
        public readonly string Username;
        public readonly int ID;
        public int KillCount;
        public int DeathCount;
        public readonly float Score;

        public PlayerDataModel(string username, int id, int killCount, int deathCount, float score)
        {
            Username = username;
            ID = id;
            KillCount = killCount;
            DeathCount = deathCount;
            Score = score;
        }
    }
}
