using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    private PlayerController playerCtrl;
    private Player levelPlayer;
    [SerializeField] private GameObject[] swords;

    [Header("Spell")]
    [SerializeField] private float castCd;
    [SerializeField] private GameObject[] castOrbs;
    [SerializeField] private Transform castLoc;
    [SerializeField] private float castRadius;
    [SerializeField] private float castForce;
    [SerializeField] private Button castButton;
    [SerializeField] private Image castCDImage;

    [Header("AOE")]
    [SerializeField] private float aoeCd;
    [SerializeField] private Button aoeButton;
    [SerializeField] private Image aoeCDImage;
    [SerializeField] private GameObject aoeBox;
    [SerializeField] private Transform aoeLoc;
    [SerializeField] private float aoeRadius;
    [SerializeField] public float aoePushBoost;
    [SerializeField] public float aoeDamageBoost;
    //[SerializeField] private GameObject aoeParticle;
    [SerializeField] private ParticleSystem aoeParticleSystem;

    [Header("Chest Skill")]
    [SerializeField] private GameObject openButton;
    [SerializeField] private GameObject attackButton;
    public ChestManager targetChest;
    private void Start()
    {
        playerCtrl = gameObject.GetComponent<PlayerController>();
        levelPlayer = playerCtrl.levelPlayer;

        if (levelPlayer.playerLevel >= levelPlayer.playerSet.Capacity)
        {
            ActiveteSword(2);
        }
        else
        {
            ActiveteSword(levelPlayer.playerSet[levelPlayer.playerLevel] - 1);
        }
    }
    private void Update()
    {
        GuardTest();
    }
    private void GuardTest()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CastSpell();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CastCombo();
        }
    }
    public void CastSpell()
    {
        if (playerCtrl.canHit && castButton.interactable)
        {
            playerCtrl.PlayerAttack("b_cast", false, 0, castRadius);
            StartCoroutine(CastCDCor());
        }
    }
    private void CastOrb()
    {
        GameObject orb = null;
        for (int i = 0; i < castOrbs.Length; i++)
        {
            if (!castOrbs[i].activeSelf)
            {
                orb = castOrbs[i];
                break;
            }
        }
        if (orb == null)
        {
            castOrbs[0].SetActive(false);
            orb = castOrbs[0];
        }
        orb.transform.position = castLoc.position;
        orb.transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        Rigidbody orbRB = orb.GetComponent<Rigidbody>();
        orb.SetActive(true);
        if (orbRB != null)
        {
            orbRB.velocity = Vector3.zero;
            orbRB.AddForce(transform.forward * castForce, ForceMode.Impulse);
        }
        orb.GetComponent<CastSpell>().PlayParticle();
        orb = null;
        orbRB = null;
    }

    private IEnumerator CastCDCor()
    {
        castButton.interactable = false;
        castCDImage.enabled = true;
        castCDImage.fillAmount = 1f;
        float cd = castCd;
        while (0 < cd)
        {
            yield return null;
            cd -= Time.deltaTime;
            castCDImage.fillAmount = cd / castCd;
        }
        castCDImage.enabled = false;
        castButton.interactable = true;
    }
    //castAOE
    public void CastCombo()
    {
        if (playerCtrl.canHit && aoeButton.interactable)
        {
            playerCtrl.PlayerAttack("b_combo", false, 0, aoeRadius);
            StartCoroutine(ComboCD());
        }
    }
    private void AoeStart()
    {
        aoeBox.SetActive(false);
        aoeBox.transform.position = aoeLoc.position;
        aoeBox.transform.rotation = Quaternion.identity;
        aoeBox.SetActive(true);
        aoeBox.GetComponent<AoeCast>().StartAoe();
        StartCoroutine(AoeCoroutine());
        aoeParticleSystem.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        aoeParticleSystem.Play();

    }
    private IEnumerator AoeCoroutine()
    {
        float _stop = 1f;
        while (_stop > 0)
        {
            _stop -= 0.1f;
            aoeParticleSystem.gameObject.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.3f);
        //aoeBox.SetActive(false);
        aoeParticleSystem.Stop();
    }
    private IEnumerator ComboCD()
    {
        aoeButton.interactable = false;
        aoeCDImage.enabled = true;
        aoeCDImage.fillAmount = 1f;
        float cd = aoeCd;
        while (0 < cd)
        {
            yield return null;
            cd -= Time.deltaTime;
            aoeCDImage.fillAmount = cd / aoeCd;
        }
        aoeCDImage.enabled = false;
        aoeButton.interactable = true;
    }
    private void ActiveteSword(int x)
    {
        for (int i = 0; i < swords.Length; i++)
        {
            if (i == x)
            {
                swords[i].gameObject.SetActive(true);
            }
            else
            {
                swords[i].gameObject.SetActive(false);
            }
        }
    }

    public void ChestButtonSetter(bool value)
    {
        attackButton.SetActive(!value);
        openButton.SetActive(value);
    }
    public void OpenChest()
    {
        if (targetChest)
        {
            targetChest.ChestOpen();
        }
    }

}
