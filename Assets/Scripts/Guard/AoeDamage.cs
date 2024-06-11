using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeDamage : MonoBehaviour
{
    private GameObject guard;
    private PlayerController playerController;
    private SkillManager skillManager;
    private float pushBoost;
    private float damageBoost;
    private void OnEnable()
    {
        guard = LevelManager.guardStatic;
        playerController = guard.GetComponent<PlayerController>();
        skillManager = guard.GetComponent<SkillManager>();
        
        pushBoost = skillManager.aoePushBoost;
        damageBoost = skillManager.aoeDamageBoost;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("aoe hit enemy " + other.name);
            EnemyManager em = other.gameObject.GetComponent<EnemyManager>();
            if (!em.agentDead)
            {
                //Vector3 _forceVec = (other.transform.position - transform.position);
                Vector3 _forceVec = (other.transform.position - guard.transform.position);
                _forceVec = new Vector3(_forceVec.x, 0, _forceVec.z).normalized;
                //em.AgentGetExplosive(100f , transform.position , playerController.playerDamage);
                //em.AgentGetDamage(playerController.levelPlayer.playerDamage * damageBoost, _forceVec * pushBoost);
                em.AgentGetDamage(playerController.guardDamage * damageBoost, _forceVec * pushBoost);
            }
        }
    }
}
