using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapManager : MonoBehaviour
{
    private PlayerController playerController;
    private ZoneManager zoneManager;
    private bool trapActive;
    private Animator trapAnim;
    [SerializeField] private BoxCollider[] damageColliders;
    [SerializeField] private float trapActivasionDelay;
    private float trapDuration;
    [SerializeField] private float trapSpeed;
    [SerializeField] private float trapDamage;
    private bool trapCanDamage;
    private IEnumerator trapCor;


    void Start()
    {
        playerController = LevelManager.guardStatic.GetComponent<PlayerController>();
        zoneManager = transform.GetComponentInParent<ZoneManager>();
        trapAnim = GetComponent<Animator>();

        trapActive = false;
        trapCanDamage = true;
        DamageColliderControl(false);
        trapDuration = 7200f;
        trapAnim.speed = trapSpeed;
        zoneManager.AddZoneScore(ZoneManager.ZonerType.Trap);
    }
    public void TrapActivate()
    {
        trapActive = true;
        trapCor = TrapCorV2();
        StartCoroutine(trapCor);
    }
    private IEnumerator TrapCorV2()
    {
        yield return new WaitForSeconds(trapActivasionDelay);
        trapAnim.SetBool(trapAnim.GetParameter(2).name, true);
        yield return new WaitForSeconds(trapDuration);
        DisableTrap();
    }
    public void DisableTrap()
    {
        if (trapActive)
        {
            trapActive = false;
            trapCanDamage = false;
            trapAnim.SetBool(trapAnim.GetParameter(1).name, true);
            if (!zoneManager.isZonePassed)
            {
                zoneManager.ZoneScoreManager(ZoneManager.ZonerType.Trap);
            }
            trapAnim.SetBool(trapAnim.GetParameter(2).name, false);
            trapAnim.SetBool(trapAnim.GetParameter(1).name, true);
            StopCoroutine(trapCor);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (trapActive && trapCanDamage)
            {
                trapCanDamage = false;

                //player damage
                if (other.GetComponent<BoostManager>().reduceDamage)
                {
                    playerController.PlayerDamageController(Mathf.RoundToInt(trapDamage / 4));
                }
                else
                {
                    playerController.PlayerDamageController(trapDamage);
                }
                DamageColliderControl(false);
            }
        }
    }
    private void DamageColliderControl(bool _value)
    {
        foreach (Collider _item in damageColliders)
        {
            _item.enabled = _value;
        }
    }
    //Animation Use ↓
    private void TrapStartAttack()
    {
        trapCanDamage = true;
        DamageColliderControl(true);
    }
    //Animation Use ↓
    public void DestroyTrap()
    {
        //Destroy(gameObject);
        //Debug.Log("trap disabled " + gameObject.name);
    }
    //Animation Use ↓
    private void ParticleControl(int _value)
    {
        ParticleSystem[] fireParticles = GetComponentsInChildren<ParticleSystem>();
        switch (_value)
        {
            case 1:
                foreach (ParticleSystem particle in fireParticles)
                {
                    particle.Play();
                }
                break;
            case 2:
                foreach (ParticleSystem particle in fireParticles)
                {
                    particle.Stop();
                }
                break;
            default:
                Debug.Log("wrong type/value of value");
                break;
        }
    }
}



// private IEnumerator _TrapCoroutineNOUSE()
// {
//     yield return new WaitForSeconds(trapActivasionDelay);
//     float _trapLenght = trapDuration;
//     while (trapDuration > 0)
//     {
//         TrapStartAttack();
//         trapDuration -= trapAttackRate;
//         yield return new WaitForSeconds(trapAttackRate);
//         //TrapStopAttack();
//     }
//     trapAnim.SetBool(trapAnim.GetParameter(1).name, true);
//     if (!zoneManager.isZonePassed)
//     {
//         zoneManager.ZoneScoreManager(ZoneManager.ZonerType.Trap);
//     }
//     yield return new WaitForSeconds(trapDeactivateDelay);
//     gameObject.SetActive(false);
// }

// private void TrapStopAttack()
// {
//     trapAnim.ResetTrigger(trapAnim.GetParameter(0).name);
//     DamageColliderControl(false);
//     trapCanDamage = true;
// }
