using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JoyStickGlow : MonoBehaviour
{
    private enum glow
    {
        plus,
        minus,
        neutral
    }
    [Header("Joystick")]
    private Joystick joystick;
    [SerializeField] private Image[] glowImages;
    [SerializeField] private glow hrzn = glow.neutral;
    [SerializeField] private glow vert = glow.neutral;
    private void Start()
    {
        joystick = LevelManager.guardStatic.GetComponent<PlayerController>().joystick;
    }
    private void LateUpdate()
    {
        GlowerVertical();
        GlowerHorizontal();
        GlowActivator(hrzn, vert);
    }
    private void GlowerVertical()
    {
        if (joystick.Vertical < 0.1f && joystick.Vertical > -0.1f)
        {
            vert = glow.neutral;
        }
        else if (joystick.Vertical >= 0.1f)
        {
            vert = glow.plus;
        }
        else
        {
            vert = glow.minus;
        }
    }
    private void GlowerHorizontal()
    {
        if (joystick.Horizontal < 0.1f && joystick.Horizontal > -0.1f)
        {
            hrzn = glow.neutral;
        }
        else if (joystick.Horizontal >= 0.1f)
        {
            hrzn = glow.plus;
        }
        else
        {
            hrzn = glow.minus;
        }
    }
    private void GlowActivator(glow x, glow y)
    {
        switch (x, y)
        {
            case (glow.minus, glow.plus):
                gimager(0, 5);
                break;
            case (glow.minus, glow.minus):
                gimager(3, 5);
                break;
            case (glow.minus, glow.neutral):
                gimager(0, 3);
                break;

            case (glow.plus, glow.plus):
                gimager(1, 5);
                break;
            case (glow.plus, glow.minus):
                gimager(2, 5);
                break;
            case (glow.plus, glow.neutral):
                gimager(1, 2);
                break;

            case (glow.neutral, glow.plus):
                gimager(0, 1);
                break;
            case (glow.neutral, glow.minus):
                gimager(2, 3);
                break;
            default:
                gimager(5, 5);
                break;
        }
    }
    private void gimager(int x, int y)
    {
        for (int i = 0; i < glowImages.Length; i++)
        {
            if (i == x)
            {
                glowImages[i].enabled = true;
            }
            else if (i == y)
            {
                glowImages[i].enabled = true;
            }
            else
            {
                glowImages[i].enabled = false;
            }
        }
    }
}
