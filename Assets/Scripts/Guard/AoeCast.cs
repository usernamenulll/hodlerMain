using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeCast : MonoBehaviour
{
    private Animator aoeAnim;
    //[SerializeField] private bool canDamage;
    private void OnEnable()
    {
        aoeAnim = GetComponent<Animator>();
    }
    public void StartAoe()
    {
        aoeAnim.SetTrigger("t_aoe");
    }
    private void OnCollisionEnter(Collision other) {    
        Debug.Log("collision");
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("trigger stay");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
    }
}
