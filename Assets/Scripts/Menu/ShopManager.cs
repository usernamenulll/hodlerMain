using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    //Main
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private int[] boostCounts;
    private int playerLevel;
    private int[] buys = new int[3];

    //Texts
    [SerializeField] private TextMeshProUGUI[] ownTexts;
    [SerializeField] private TextMeshProUGUI[] buyCosts;
    //Buttons
    [SerializeField] private Button[] buyButtons;

    void Start()
    {
        boostCounts = player.boostCounts;
        playerLevel = player.playerLevel;
        ShopKeep();
        ShopButtons();
    }

    private void ShopKeep()
    {
        for (int i = 0; i < boostCounts.Length; i++)
        {
            boostCounts = player.boostCounts;
            //calculator
            buys[i] = (Mathf.RoundToInt(playerLevel * 0.5f * 1000));

            ownTexts[i].text = boostCounts[i].ToString();
            buyCosts[i].text = buys[i].ToString();
        }
    }
    private void ShopButtons()
    {
        for (int i = 0; i < boostCounts.Length; i++)
        {
            int boostIndex = i;
            buyButtons[i].onClick.AddListener(delegate
            { BuyBoost(boostIndex); });
        }
    }
    private void BuyBoost(int index)
    {
        if ((player.playerCoin - buys[index]) >= 0)
        {
            player.playerCoin -= buys[index];
            player.boostCounts[index]++;
            player.SavePlayer();
            ShopKeep();
            gameManager.UIManage();
        }
        else
        {
            Debug.Log("not enought coin you need : " +
            (buys[index] - player.playerCoin) + " more");
        }
    }
}
