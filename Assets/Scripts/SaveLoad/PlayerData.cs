[System.Serializable]
public class PlayerData
{
    //main
    public int coin;
    public int level;
    public int exp;
    public int skill;
    public int[] set;
    public bool sound;
    //player stats
    public float health;
    public float damage;
    public float speed;
    public float mana;

    //stage
    public int stage;
    public bool[] stageComp;
    public bool[] stageMission;

    //boost
    public int[] boostCounts = new int[3];
    public int[] boostLevels = new int[3];

    public PlayerData(Player player){
        coin = player.playerCoin;

        stage = player.playerStage;
        //stageComp = player.stageComp.ToArray();
        //stageMission = player.stageMission.ToArray();

        level = player.playerLevel;
        exp = player.playerExp;

        set = player.playerSet.ToArray();
        sound = player.playerSound;

        health = player.playerHealth;
        damage = player.playerDamage;
        speed = player.playerSpeed;
        mana = player.playerMana;

        boostCounts = player.boostCounts;
        boostLevels = player.boostLevels;
    }
}
