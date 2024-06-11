using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeOrb : MonoBehaviour
{
    private bool canDamage = true;
    private float damage = 10f;
    private float orbLife = 5f;
    private IEnumerator orbCor;
    private ParticleSystem particle;
    private void OnEnable()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        canDamage = true;
        particle.Play();
        orbCor = DisableOrb(orbLife);
        StartCoroutine(orbCor);
    }
    public void OrbDamageSetter(float _damage)
    {
        damage = _damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (canDamage && other.gameObject.CompareTag("Player"))
        {
            canDamage = false;
            //Debug.Log("hit player with " + damage);
            PlayerController pc = other.GetComponentInParent<PlayerController>();
            if (pc.canTakeDamage)
            {
                pc.PlayerDamageController(damage);
            }
            particle.Stop();
            if (orbCor != null)
            {
                StopCoroutine(orbCor);
                orbCor = DisableOrb(0.5f);
                StartCoroutine(orbCor); 
            }
        }
        // else if (!other.gameObject.CompareTag("Enemy"))
        // {
        //     gameObject.SetActive(false);
        // }
    }
    private IEnumerator DisableOrb(float x)
    {
        yield return new WaitForSeconds(x);
        particle.Clear();
        gameObject.SetActive(false);
    }
}
