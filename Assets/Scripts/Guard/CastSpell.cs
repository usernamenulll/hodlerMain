using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpell : MonoBehaviour
{

    private PlayerController playerController;
    private bool castCanHit;
    IEnumerator castCor;
    private ParticleSystem[] particles;
    private void OnEnable()
    {
        playerController = LevelManager.guardStatic.GetComponent<PlayerController>();
        particles = GetComponentsInChildren<ParticleSystem>();
        StopAllCoroutines();
        castCanHit = true;
        castCor = DisableOrb(5f);
        StartCoroutine(castCor);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && castCanHit)
        {
            castCanHit = false;
            EnemyManager em = other.gameObject.GetComponent<EnemyManager>();
            if (!em.agentDead)
            {
                Vector3 _forceVec = (other.transform.position - transform.position);
                _forceVec = new Vector3(_forceVec.x, 0, _forceVec.z).normalized;
                //em.AgentGetDamage(playerController.levelPlayer.playerDamage * 2f, _forceVec);
                em.AgentGetDamage(playerController.guardDamage * 2f, _forceVec);
            }
            if (castCor != null)
            {
                StopCoroutine(castCor);
                castCor = DisableOrb(0.5f);
                StartCoroutine(castCor);
            }
            StopParticle();
        }
    }
    private IEnumerator DisableOrb(float x)
    {
        yield return new WaitForSeconds(x);
        gameObject.SetActive(false);
    }
    private void StopParticle()
    {
        foreach (ParticleSystem ps in particles)
        {
            ps.Stop();
        }
    }
    public void PlayParticle()
    {
        foreach (ParticleSystem ps in particles)
        {
            ps.Clear();
            ps.Play();
        }
    }
}
