using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource slash1;
    [SerializeField] private AudioSource slash2;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void playSwordSlash(int x)
    {
        switch (x)
        {
            case 1:
                slash1.Stop();
                slash1.Play();
                break;
            case 2:
                slash2.Stop();
                slash2.Play();
                break;
            default:
                break;
        }
    }
}
