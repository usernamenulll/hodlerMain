using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneControl : MonoBehaviour
{
    public enum RuneType
    {
        red,
        blue,
        green,
        white
    }
    [SerializeField] private RuneType rune;
    [SerializeField] private GameObject runeStone;
    [SerializeField] private Light runeLight;
    [SerializeField] private GameObject[] props;
    [SerializeField] private Color[] colors;
    [SerializeField] private Material[] materials;
    private BoxCollider activationCollider;
    // Start is called before the first frame update
    void Start()
    {
        activationCollider = GetComponent<BoxCollider>();

        activationCollider.enabled = false;

        ActiveteRuneStone2(((int)rune));
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void WakeRune()
    {

    }
    /*
    private void ActivateRuneStone(int x)
    {
        for (int i = 0; i < runeStones.Length; i++)
        {
            if (i == x)
            {
                runeStones[i].SetActive(true);
                runeLight.color = colors[i];
            }
            else
            {
                runeStones[i].SetActive(false);
            }
        }
        runeLight.gameObject.SetActive(true);
    }
    */
    private void ActiveteRuneStone2(int y)
    {
        runeStone.GetComponent<MeshRenderer>().material = materials[y];
        runeLight.color = colors[y];
        foreach (GameObject prp in props)
        {
            prp.GetComponent<MeshRenderer>().material = materials[y];
            prp.SetActive(true);
        }
        runeStone.SetActive(true);
        runeLight.gameObject.SetActive(true);
    }

}
