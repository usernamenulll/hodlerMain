using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Player player;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager uIManager;
    [Header("Level Player")]
    public float playerMaxHealth;
    public Player levelPlayer;
    public float guardSpeed;
    public float guardDamage;
    [SerializeField] private Animator animator;
    [Header("Move")]
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] public Joystick joystick;
    public bool canMove;
    private bool canRotate = true;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float horizontalInp;
    [SerializeField] private float verticalInp;
    private Vector3 moveVector;
    private Vector3 turnVector;

    [Header("Attack")]
    [SerializeField] public bool canHit;
    [SerializeField] public bool canDamage;
    [SerializeField] private BoxCollider attackCollider;
    [SerializeField] private int attackIndex = 1;
    private float timeHolder = 0;
    [SerializeField] public bool canTakeDamage;

    [Header("Auto Target")]
    [SerializeField] private float push;
    [SerializeField] private bool canPush;
    [SerializeField] private float attackRadius;
    [SerializeField] private Collider[] autoTargetHits;
    [SerializeField] private LayerMask targetLayer;
    private void Awake()
    {
        GetDatas();
    }
    private void Start()
    {
        Time.timeScale = 1f;
        canHit = true;
        canPush = false;
        canTakeDamage = true;
        attackCollider.enabled = false;

        playerMaxHealth = levelPlayer.playerHealth;
        playerRb.isKinematic = false;
    }
    private void Update()
    {
        InputControl();

        //AA
        if (Input.GetKeyDown(KeyCode.Space) && canHit)
        {
            canPush = true;
            playerAA();
        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    public void playerEarnExp(int exp)
    {
        levelPlayer.playerExp += exp;
        if (levelPlayer.playerExp >= levelPlayer.playerLevel * 1000)
        {
            levelPlayer.playerLevel++;
            playerMaxHealth = Mathf.Round(playerMaxHealth * 1.07f);
            levelPlayer.playerSpeed = Mathf.Round((levelPlayer.playerSpeed + 0.1f) * 10) / 10.0f;
            levelPlayer.playerDamage = Mathf.Round(levelPlayer.playerDamage * 1.08f);
            levelPlayer.playerMana = Mathf.Round(levelPlayer.playerMana * 1.05f);
            levelPlayer.playerExp = 0;
            levelPlayer.playerHealth = playerMaxHealth;


            if (guardSpeed < levelPlayer.playerSpeed)
            {
                guardSpeed = levelPlayer.playerSpeed;
            }
            if (guardDamage < levelPlayer.playerDamage)
            {
                guardDamage = levelPlayer.playerDamage;
            }

            uIManager.PlayerHealthControl(levelPlayer.playerHealth / playerMaxHealth);
        }
    }
    public void playerAA()
    {
        if (canHit)
        {
            if (attackIndex == 2 && (timeHolder + 0.7f) < Time.time)
            {
                attackIndex = 1;
            }
            switch (attackIndex)
            {
                case 1:
                    PlayerAttack("b_aa", true, push, attackRadius);
                    timeHolder = Time.time;
                    break;
                case 2:
                    PlayerAttack("b_ab", true, push, attackRadius);
                    break;
                default:
                    PlayerAttack("b_aa", true, push, attackRadius);
                    break;
            }
            attackIndex++;
            if (attackIndex > 2)
            {
                attackIndex = 1;
            }
        }
    }
    public void DamageControl(string x)
    {
        if (x == "start")
        {
            canDamage = true;
            attackCollider.enabled = true;
        }
        else if (x == "stop")
        {
            canDamage = false;
            attackCollider.enabled = false;
        }
        else
        {
            Debug.Log("wrong input mate");
        }
    }
    public void PlayerAttack(string _anim, bool _push, float _force, float _radius)
    {
        if (canHit)
        {
            canHit = false;
            canPush = _push;
            playerRb.velocity = Vector3.zero;
            autoTarget(_radius, _force);
            animator.SetBool("b_run", false);
            animator.SetBool(_anim, true);
            canMove = false;
            canRotate = false;
        }
    }
    public void HitEnder(string _anim)
    {
        canRotate = true;
        canHit = true;
        canDamage = false;
        if (canPush)
        {
            canPush = false;
        }
        if (canHit)
        {
            canMove = true;
            if (moveVector != Vector3.zero)
            {
                animator.SetBool("b_run", true);
            }
        }
        if (_anim != null)
        {
            animator.SetBool(_anim, false);
        }
    }

    private void Move()
    {
        if (canMove)
        {
            //playerRb.velocity = moveVector * levelPlayer.playerSpeed;
            playerRb.velocity = moveVector * guardSpeed;
            if (moveVector != Vector3.zero)
            {
                if (moveVector != Vector3.zero && canRotate)
                {
                    Quaternion xtargetRotation = Quaternion.LookRotation(turnVector.normalized, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, xtargetRotation, Time.deltaTime * rotationSpeed);
                }
            }
            if (playerRb.velocity != Vector3.zero)
            {
                animator.SetBool("b_run", true);
            }
            else
            {
                animator.SetBool("b_run", false);

            }
        }
        else
        {
            if (!canPush)
            {
                playerRb.velocity = Vector3.zero;
            }
        }
    }
    private void InputControl()
    {
        horizontalInp = joystick.Horizontal;
        verticalInp = joystick.Vertical;
        moveVector = new Vector3(horizontalInp, 0, verticalInp).normalized;
        turnVector = new Vector3(horizontalInp, 0, verticalInp).normalized;
        moveVector.y = 0;
        turnVector.y = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (canDamage)
            {
                EnemyManager em = other.gameObject.GetComponent<EnemyManager>();
                if (!em.agentDead)
                {
                    Vector3 _forceVec = (other.transform.position - transform.position);
                    _forceVec = new Vector3(_forceVec.x, 0, _forceVec.z).normalized;
                    //em.AgentGetDamage(levelPlayer.playerDamage, _forceVec);
                    em.AgentGetDamage(guardDamage, _forceVec);
                }
            }
        }
    }

    private void autoTarget(float _rd, float _psh)
    {
        Vector3 _loc = new Vector3(transform.position.x, 0.5f, transform.position.z);
        autoTargetHits = Physics.OverlapSphere(_loc, _rd, targetLayer);
        float _dis = _rd * 2;
        Collider _targetObj = null;
        if (autoTargetHits.Length > 0)
        {

            foreach (Collider _hit in autoTargetHits)
            {
                if (_hit != null && _hit.gameObject.CompareTag("Enemy"))
                {
                    bool _isDead = _hit.gameObject.GetComponent<EnemyManager>().agentDead;
                    if (!_isDead)
                    {
                        float _x = Vector3.Distance(transform.position, _hit.transform.position);
                        if (_x < _dis)
                        {
                            _dis = _x;
                            _targetObj = _hit;
                        }
                    }
                }
            }
            if (_targetObj != null)
            {
                transform.LookAt(new Vector3(_targetObj.transform.position.x, transform.position.y, _targetObj.transform.position.z));
            }
            if (_dis <= _rd)
            {
                if (canPush)
                {
                    playerRb.AddForce(playerRb.transform.forward * _psh * _dis, ForceMode.Force);
                }
            }
        }
        else
        {
            if (canPush)
            {
                playerRb.AddForce(playerRb.transform.forward * _psh * 3, ForceMode.Force);
            }
        }
        autoTargetHits = null;
    }
    public void PlayerDamageController(float _dmg)
    {
        if (canTakeDamage)
        {
            levelPlayer.playerHealth -= _dmg;
            if (levelPlayer.playerHealth <= 0)
            {
                levelPlayer.playerHealth = 0;
                uIManager.PlayerHealthControl(0f);
            }
            else
            {
                uIManager.PlayerHealthControl(levelPlayer.playerHealth / playerMaxHealth);
            }
        }
    }
    public void PlayerStartedAgain(float _sec)
    {
        StartCoroutine(ResumeGameCoroutine(_sec));
    }
    private IEnumerator ResumeGameCoroutine(float _sec)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(_sec);
        canTakeDamage = true;
    }
    public void PlayerHealer(float mlt)
    {
        float val = (playerMaxHealth / 5) * mlt;
        if (levelPlayer.playerHealth < playerMaxHealth)
        {
            levelPlayer.playerHealth += val;
            if (levelPlayer.playerHealth > playerMaxHealth)
            {
                levelPlayer.playerHealth = playerMaxHealth;
            }
        }
        uIManager.PlayerHealthControl(levelPlayer.playerHealth / playerMaxHealth);
    }
    private void GetDatas()
    {
        gameObject.AddComponent<Player>();
        levelPlayer = GetComponent<Player>();
        levelPlayer.LoadPlayer();

        guardSpeed = levelPlayer.playerSpeed;
        guardDamage = levelPlayer.playerDamage;
    }
    public void PlayerVictory()
    {
        StopAllCoroutines();
        playerRb.isKinematic = true;
        canMove = false;
        canRotate = false;
        playerRb.velocity = Vector3.zero;
        animator.SetTrigger("t_victory");

        SaveLevelPlayer();
    }
    private void SaveLevelPlayer()
    {
        //harcanan boostların levelın geçilmediği durumda kaydedilmemesi durumunu önlemek amacıyla level player oluşturuldur burada asıl playerın boost countları alınarak 
        int[] _x = player.boostCounts;
        int[] _y = player.boostLevels;
        player = levelPlayer;
        player.playerHealth = playerMaxHealth;
        player.boostCounts = _x;
        player.boostLevels = _y;
        //Debug.Log("player stage :" + player.playerStage);
        //Debug.Log("build index : " + SceneManager.GetActiveScene().buildIndex);
        if (player.playerStage == SceneManager.GetActiveScene().buildIndex)
        {
            int levelCount = SceneManager.sceneCountInBuildSettings - 1;
            if (levelCount > player.playerStage)
            {
                player.playerStage++;
            }
            // Debug.Log("scene count : " + SceneManager.sceneCountInBuildSettings);
            // Debug.Log("playerstage   :  " + player.playerStage);
        }
        player.SavePlayer();
    }
}