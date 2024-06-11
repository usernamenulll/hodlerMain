using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGuardRot : MonoBehaviour
{
    [Header("Joystick")]
    [SerializeField] private Joystick mainJoystick;
    [SerializeField] private GameObject guard;
    private Rigidbody guardRb;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float horizontalInp;
    // Start is called before the first frame update
    void Start()
    {
        guardRb = guard.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        horizontalInp = mainJoystick.Horizontal;

        if (horizontalInp > -0.1f && horizontalInp < 0.1f)
        {
            horizontalInp = 0;
        }
        else
        {
            horizontalInp = Mathf.RoundToInt(horizontalInp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateGuard();
    }
    private void RotateGuard()
    {
        if (horizontalInp > 0)
        {
            guardRb.AddTorque(Vector3.down * rotationSpeed * horizontalInp, ForceMode.Impulse);
        }
        else
        {
            guardRb.AddTorque(Vector3.up * rotationSpeed * horizontalInp * -1, ForceMode.Impulse);
        }
    }
}
