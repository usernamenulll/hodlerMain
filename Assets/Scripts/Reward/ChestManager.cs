using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    private NotificationManager notificationManager;
    private SkillManager skillManager;
    private PlayerController playerController;
    private UIManager uIManager;
    [SerializeField] private GameObject chestCover;
    [SerializeField] private ParticleSystem openParticle;
    [SerializeField] public bool chestCollected;
    [SerializeField] private int minReward;
    [SerializeField] private int maxReward;
    private int reward;
    private BoxCollider activationCollider;
    private GameObject chestCanvas;
    private Animator chestAnim;
    void Start()
    {
        skillManager = LevelManager.guardStatic.GetComponent<SkillManager>();
        notificationManager  =LevelManager.levelManagerStatic.gameObject.GetComponent<NotificationManager>();
        playerController = LevelManager.guardStatic.GetComponent<PlayerController>();
        uIManager = LevelManager.levelManagerStatic.gameObject.GetComponent<UIManager>();
        activationCollider = GetComponent<BoxCollider>();
        chestAnim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            skillManager.targetChest = this;
            skillManager.ChestButtonSetter(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (skillManager.targetChest == this)
            {
                skillManager.ChestButtonSetter(false);
                skillManager.targetChest = null;
            }

            if (chestCollected)
            {
                chestAnim.enabled = false;
                chestCover.transform.localRotation = Quaternion.Euler(new Vector3(-50, 0, 0));
                activationCollider.enabled = false;
            }
        }
    }
    public void ChestOpen()
    {
        chestCollected = true;
        chestAnim.SetBool(chestAnim.GetParameter(0).name, true);
        openParticle.Play();
        CalculateReward();
        // if (transform.parent.CompareTag("ZoneManager"))
        // {
        //     transform.parent.GetComponent<ZoneManager>().ZoneScoreManager(ZoneManager.ZonerType.Other);
        // }
        skillManager.ChestButtonSetter(false);
        skillManager.targetChest = null;
        activationCollider.enabled = false;
    }
    private void ChestDisable()
    {
        chestAnim.enabled = false;
        chestCover.transform.localRotation = Quaternion.Euler(new Vector3(-50, 0, 0));
    }

    private void CalculateReward()
    {
        reward = Random.Range(minReward , maxReward+1);
        string _not = "  " + reward.ToString() + (" Gold earned!");
        notificationManager.NotPlayer(0,_not);
        playerController.levelPlayer.playerCoin += reward;
        uIManager.PlayerGoldControl();
    }
}
