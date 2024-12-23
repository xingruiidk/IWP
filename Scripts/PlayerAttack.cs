using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;
    private Animator animator;
    public float damage;
    private float baseDmg;
    private int comboInt = 0;
    private bool comboEnd;
    private int maxCombo = 4;
    private float comboTimer = 0f;
    private float comboCooldown = 1f;
    private float resetCooldown = 1f;
    public GameObject[] swordCollider;
    private float colliderTimer = 0.3f;
    private float inCombatTimer = 10f;
    public bool inCombat = false;
    void Start()
    {
        foreach (GameObject collider in swordCollider)
        {
            collider.SetActive(false);
        }
        instance = this;
        comboEnd = true;
        baseDmg = damage;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleCombo();
        CheckComboReset();
        inCombatTimer -= Time.deltaTime;
        if (inCombatTimer > 0)
        {
            inCombat = true;
        }
        else
        {
            inCombat = false;
        }
    }


    void HandleCombo()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            if (comboEnd)
            {
                comboEnd = false;
                comboInt++;
                comboInt = comboInt > maxCombo ? 1 : comboInt;
                if (animator != null)
                {
                    animator.SetInteger("Combo", comboInt);
                    animator.SetTrigger("DoComboAttack");
                }
                Debug.Log($"Combo Attack: {comboInt}");
                comboTimer = Time.time + comboCooldown;
            }
            inCombatTimer = 10;
        }
        if (comboInt == 1)
        {
            damage = baseDmg;
        }
        else if (comboInt == 2)
        {
            damage = baseDmg + 2;
        }
        else if (comboInt == 3)
        {
            damage = baseDmg;
        }
        else if (comboInt == 4)
        {
            damage = baseDmg + 5;
        }
    }
    void CheckComboReset()
    {
        if (comboInt > 0 && Time.time > comboTimer + resetCooldown)
        {
            comboInt = 0;
            Debug.Log("Combo Reset!");
            if (animator != null)
            {
                animator.SetInteger("Combo", comboInt);
            }
        }
    }

    public void EndCombo()
    {
        comboEnd = true;
    }
    public void SetColliderTrueFalse()
    {
        StartCoroutine(ToggleCollider(swordCollider[comboInt - 1], colliderTimer));
    }
    public IEnumerator ToggleCollider(GameObject go, float duration)
    {
        go.SetActive(true);
        yield return new WaitForSeconds(duration);
        go.SetActive(false);
    }
}
