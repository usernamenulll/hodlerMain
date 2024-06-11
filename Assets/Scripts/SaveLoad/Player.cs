using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    //datas which will be saved/stored
    [Header("Main")]
    public int playerCoin;
    public int playerLevel;
    public int playerExp;
    public List<int> playerSet;
    public bool playerSound;
    [Header("Player Stats")]
    public float playerHealth;
    private float playerHlt;
    public float playerDamage;
    public float playerSpeed;
    public float playerMana;
    [Header("Stage")]
    public int playerStage;
    //public List<bool> stageComp;
    //public List<bool> stageMission;

    [Header("Boost Levels")]
    public int[] boostCounts = new int[3];
    public int[] boostLevels = new int[3];

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        playerCoin = data.coin;
        playerStage = data.stage;
        //stageComp = new List<bool>(data.stageComp);
        //stageMission = new List<bool>(data.stageMission);
        playerLevel = data.level;
        playerExp = data.exp;

        playerSet = new List<int>(data.set);
        playerSound = data.sound;

        playerHealth = data.health;
        playerDamage = data.damage;
        playerSpeed = data.speed;
        playerMana = data.mana;

        boostCounts = data.boostCounts;
        boostLevels = data.boostLevels;
    }
    public void DeletePlayer()
    {
        SaveSystem.DeletePlayer();
    }
}

/*
    public static Player PushPlayerData()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        GameObject push = new GameObject();
        push.name = "level player";
        push.AddComponent<Player>();
        Player pushPlayer = push.GetComponent<Player>();

        pushPlayer.playerCoin = data.coin;
        pushPlayer.playerStage = data.stage;
        pushPlayer.stageComp = new List<bool>(data.stageComp);
        pushPlayer.stageMission = new List<bool>(data.stageMission);
        pushPlayer.playerLevel = data.level;
        pushPlayer.playerExp = data.exp;

        pushPlayer.playerSet = new List<int>(data.set);

        pushPlayer.playerHealth = data.health;
        pushPlayer.playerDamage = data.damage;
        pushPlayer.playerSpeed = data.speed;
        pushPlayer.playerMana = data.mana;

        pushPlayer.boostCounts = data.boostCounts;
        pushPlayer.boostLevels = data.boostLevels;
        //Destroy(push);
        return pushPlayer;
    }

    public static void PushPlayerData(Player pushPlayer)
    {
        PlayerData data = SaveSystem.LoadPlayer();
        pushPlayer.playerCoin = data.coin;
        pushPlayer.playerStage = data.stage;
        pushPlayer.stageComp = new List<bool>(data.stageComp);
        pushPlayer.stageMission = new List<bool>(data.stageMission);
        pushPlayer.playerLevel = data.level;
        pushPlayer.playerExp = data.exp;

        pushPlayer.playerSet = new List<int>(data.set);

        pushPlayer.playerHealth = data.health;
        pushPlayer.playerDamage = data.damage;
        pushPlayer.playerSpeed = data.speed;
        pushPlayer.playerMana = data.mana;

        pushPlayer.boostCounts = data.boostCounts;
        pushPlayer.boostLevels = data.boostLevels;

    }
    */
