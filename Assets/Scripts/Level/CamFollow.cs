using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    private Vector3 offset;
    [SerializeField] private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        offset = transform.position - player.position;
    }
    private void FixedUpdate()
    {
        Vector3 targetPos = player.position + offset;
        //transform.position = targetPos;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}
