using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    private LevelManager levelManager;
    private PlayerController playerController;
    private ZoneManager zoneManager;
    private NavMeshAgent agent;
    private Rigidbody agentRb;
    private Vector3 startPos;
    private GameObject guard;
    private Transform guardTransform;

    [Header("Agent")]
    [SerializeField] private Animator agentAnimator;
    [SerializeField] private Animator attackAnimator;
    [SerializeField] private bool agentActive;
    [SerializeField] private float agentHealth;
    [SerializeField] private int agentExp;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask playerLayer;
    private Vector3 newPos;
    private float x_dis;
    [SerializeField] private float stopDisanim;
    [Header("Wander")]
    [SerializeField] private bool agentStatic;
    [SerializeField] private bool canWander;
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderSpeed;
    [SerializeField] private float minWanderCD;
    [SerializeField] private float maxWanderCD;
    private IEnumerator wanderCoroutine;
    [Header("Chase")]
    [SerializeField] private bool agentGuided;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float maxChaseRadius;
    [SerializeField] private float chaseSpeed;
    [Header("Return")]
    [SerializeField] private float homeDis;
    [SerializeField] private bool isReturning;
    [Header("Attack")]
    [SerializeField] private bool canDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private bool canAttack;
    [SerializeField] private float attackRate;
    [SerializeField] public float agentDamage;
    private IEnumerator attackCoroutine;
    [Header("Take Damage")]
    [SerializeField] private float activasionCD;
    [SerializeField] private float agentTakeDmgRate;
    [SerializeField] private bool canGetDamage;
    [SerializeField] private float takeHitPush;
    [SerializeField] private float canReactDelay;
    [SerializeField] private bool canReact;
    private IEnumerator canReactCoroutine;
    private IEnumerator takeHitCoroutine;
    private IEnumerator activateCoroutine;
    [Header("Die")]
    public bool agentDead;
    [SerializeField] private float agentDieDelay;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [Header("UI")]
    [SerializeField] private Vector3 canvasRot;
    [SerializeField] private GameObject agentCanvas;
    [SerializeField] private Image healthBar;
    private float agentMaxHlth;

    private Vector3 xPos;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        guard = LevelManager.guardStatic;
        levelManager = LevelManager.levelManagerStatic;
        guardTransform = guard.transform;
        agentRb = GetComponent<Rigidbody>();
        zoneManager = transform.GetComponentInParent<ZoneManager>();
        playerController = guard.GetComponent<PlayerController>();
        gameObject.layer = 10;

        startPos = transform.position;
        agentActive = true;
        canWander = true;
        canGetDamage = true;
        canAttack = true;
        isReturning = false;
        agentGuided = false;
        canDamage = false;
        canReact = true;
        agentDead = false;

        wanderCoroutine = null;
        activateCoroutine = null;
        takeHitCoroutine = null;
        attackCoroutine = null;
        canReactCoroutine = null;

        agentMaxHlth = agentHealth;
        zoneManager.AddZoneScore(ZoneManager.ZonerType.Enemy);
    }

    void Update()
    {
        distance = Vector3.Distance(guardTransform.position, transform.position);
        if (agentActive)
        {
            if (!agentGuided)
            {
                if (distance < chaseRadius)
                {
                    if (!agentStatic)
                    {
                        StopWander();
                    }
                    agentGuided = true;
                }
                else
                {
                    if (!agentStatic)
                    {
                        WanderAround();

                        //walk animation controller
                        x_dis = Vector3.Distance(newPos, transform.position);
                        if (x_dis < stopDisanim)
                        {
                            agentAnimator.SetBool("b_walk", false);
                        }
                        else
                        {
                            agentAnimator.SetBool("b_walk", true);
                        }
                    }
                }
            }
            else
            {
                if (distance > maxChaseRadius)
                {
                    StopChase();
                    isReturning = true;
                    SetNavDestination(startPos, wanderSpeed);
                }
                else
                {
                    if (distance < attackRange)
                    {
                        agent.isStopped = true;
                        agentAnimator.SetBool("b_walk", false);
                        AgentAttack();
                        //attack functions   
                    }
                    else
                    {
                        ChasePlayer();
                    }
                }
            }

            if (isReturning)
            {
                homeDis = Vector3.Distance(transform.position, startPos);
                if (distance < chaseRadius)
                {
                    isReturning = false;
                    agentGuided = true;
                }
                if (homeDis < 2)
                {
                    isReturning = false;
                    canWander = true;
                }
            }
        }
    }
    private void LateUpdate()
    {
        agentCanvas.transform.rotation = Quaternion.Euler(canvasRot);
    }
    private Vector3 RandomNavSphere(Vector3 _origin, float _dist)
    {
        Vector3 _ranDirection = Random.insideUnitSphere * _dist;
        _ranDirection += _origin;
        NavMeshHit _navHit;
        NavMesh.SamplePosition(_ranDirection, out _navHit, _dist, NavMesh.AllAreas);
        return _navHit.position;
    }
    private void WanderAround()
    {
        if (canWander)
        {
            agent.speed = wanderSpeed;
            agent.isStopped = false;
            canWander = false;
            float _ranDelay = Random.Range(minWanderCD, maxWanderCD);
            wanderCoroutine = WanderDelayer(_ranDelay);
            StartCoroutine(wanderCoroutine);
        }
    }
    IEnumerator WanderDelayer(float _ranNum)
    {
        NavMeshPath _path = new NavMeshPath();
        newPos = RandomNavSphere(transform.position, wanderRadius);
        /*
        while (!agent.CalculatePath(_newPos, _path))
        {
            _newPos = RandomNavSphere(transform.position, wanderRadius);
            yield return null;
        }
        */

        //if positive infinity is the only problem for calculating new position use this else use calculatepath variant
        while (newPos == Vector3.positiveInfinity)
        {
            newPos = RandomNavSphere(transform.position, wanderRadius);
            yield return null;
        }

        agent.SetDestination(newPos);
        yield return new WaitForSeconds(_ranNum);
        if (agentActive)
        {
            canWander = true;
        }
    }
    private void StopWander()
    {
        if (wanderCoroutine != null)
        {
            agent.isStopped = true;
            agentAnimator.SetBool("b_walk", false);
            canWander = false;
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }
    }
    private void ChasePlayer()
    {
        if (canAttack)
        {
            agent.isStopped = false;
            agent.SetDestination(guardTransform.position);
            //transform.LookAt(new Vector3(guardTransform.position.x, transform.position.y, guardTransform.transform.position.z));
            agentAnimator.SetBool("b_walk", true);
            agent.speed = chaseSpeed;
        }
    }
    private void StopChase()
    {
        agent.isStopped = true;
        //agentAnimator.SetBool("b_walk", false);
        agentGuided = false;
    }
    private void SetNavDestination(Vector3 _destination, float _speed)
    {
        agent.isStopped = false;
        agent.SetDestination(_destination);
        agent.speed = _speed;
        agentAnimator.SetBool("b_walk", true);
    }
    public void AgentGetDamage(float _dmg, Vector3 _fc)
    {
        if (canGetDamage)
        {
            //agentAnimator.ResetTrigger("t_react");
            canGetDamage = false;
            canDamage = false;
            canAttack = false;

            attackAnimator.SetBool("b_aa", false);
            transform.LookAt(new Vector3(guardTransform.position.x, transform.position.y, guardTransform.transform.position.z));
            agentHealth -= _dmg;


            if (agentHealth <= 0)
            {
                agentHealth = 0;
                AgentHealthBarUpdate();
                StopAll(false);
                canGetDamage = false;
                if (takeHitCoroutine != null)
                {
                    StopCoroutine(takeHitCoroutine);
                }
                gameObject.layer = 31;
                agentAnimator.SetTrigger("t_die");
                AgentGetForce(_fc, takeHitPush * 1.5f);
                levelManager.ExpManger(agentExp);
                StartCoroutine(AgentDieCor());
            }
            else
            {
                AgentHealthBarUpdate();

                if (canReact)
                {
                    canReact = false;
                    if (canReactCoroutine == null)
                    {
                        canReactCoroutine = canReactCor();
                        StartCoroutine(canReactCoroutine);
                    }
                }
                takeHitCoroutine = agentTakeDamageCor();
                StartCoroutine(takeHitCoroutine);
                if (activateCoroutine == null)
                {
                    StopAll(true);
                }
                AgentGetForce(_fc, takeHitPush);
                if (!agentActive)
                {

                }
            }
        }
    }
    //function for agent rb damage
    public void AgentGetForce(Vector3 _force, float _push)
    {
        agentRb.velocity = Vector3.zero;
        agentRb.AddForce(_force * agentRb.mass * _push, ForceMode.Impulse);
    }
    IEnumerator canReactCor()
    {
        agentAnimator.SetTrigger("t_react");
        yield return new WaitForSeconds(canReactDelay);
        canReact = true;
        canReactCoroutine = null;
    }
    IEnumerator agentTakeDamageCor()
    {
        yield return new WaitForSeconds(agentTakeDmgRate);
        canGetDamage = true;
    }
    IEnumerator ActivateAgain()
    {
        yield return new WaitForSeconds(activasionCD);
        agentRb.velocity = Vector3.zero;
        agent.enabled = true;
        agentActive = true;
        canAttack = true;

        Collider[] _check = Physics.OverlapSphere(transform.position, chaseRadius, playerLayer);
        if (_check.Length == 0)
        {
            if (!canWander)
            {
                canWander = true;
            }
        }
    }
    private void AgentAttack()
    {
        if (agentActive && canAttack)
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            canAttack = false;
            agent.isStopped = true;
            transform.LookAt(new Vector3(guardTransform.position.x, transform.position.y, guardTransform.transform.position.z));
            agentAnimator.SetTrigger("t_attack");
            attackAnimator.SetBool("b_aa", true);

            attackCoroutine = AgentAttackCor();
            StartCoroutine(attackCoroutine);
        }
    }
    IEnumerator AgentAttackCor()
    {
        yield return new WaitForSeconds(attackRate);
        attackAnimator.SetBool("b_aa", false);
        agentAnimator.ResetTrigger("t_attack");
        canAttack = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (agentActive && canDamage)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                canDamage = false;
                playerController.PlayerDamageController(agentDamage);
            }
        }
    }
    private void agentDamageController(string _can)
    {
        if (agentActive)
        {
            if (_can == "true")
            {
                canDamage = true;
            }
            else
            {
                canDamage = false;
            }
        }
        else
        {
            canDamage = false;
        }
    }

    public void StopAll(bool _isAlive)
    {
        if (activateCoroutine != null)
        {
            StopCoroutine(activateCoroutine);
            activateCoroutine = null;
        }
        if (agentActive)
        {
            agentActive = false;
        }
        if (agent.enabled)
        {
            agent.enabled = false;
            //Debug.Log("agent disable");
        }
        if (_isAlive)
        {
            activateCoroutine = ActivateAgain();
            StartCoroutine(activateCoroutine);
        }
        else
        {
            agentDead = true;
        }
    }
    private void AgentDieEnd()
    {
        if (zoneManager != null)
        {
            zoneManager.ZoneScoreManager(ZoneManager.ZonerType.Enemy);
        }
        gameObject.SetActive(false);
    }

    IEnumerator AgentDieCor()
    {
        yield return new WaitForSeconds(0.5f);
        if (zoneManager != null)
        {
            zoneManager.ZoneScoreManager(ZoneManager.ZonerType.Enemy);
        }
        capsuleCollider.enabled = false;
        yield return new WaitForSeconds(agentDieDelay - 0.5f);
        gameObject.SetActive(false);
    }
    private void AgentHealthBarUpdate()
    {
        healthBar.fillAmount = agentHealth / agentMaxHlth;
    }

}
