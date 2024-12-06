using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Healthbar healthbar;
    private bool dead;
    private Animator animator;
    public GameObject models;
    public int modelint;
    private GameObject currentModel;
    private Canvas hpCanvas;
    private Vector3 hpPos;
    private CapsuleCollider capsuleCollider;
    private List<EnemyHitbox> colliderList;
    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponent<Healthbar>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        hpCanvas = GetComponentInChildren<Canvas>();
        currentModel = Instantiate(models, transform);
        CheckEnemy();
        animator = currentModel.GetComponentInChildren<Animator>();
        hpCanvas.transform.position += hpPos;
        colliderList = new List<EnemyHitbox>();
        colliderList.AddRange(GetComponentsInChildren<EnemyHitbox>());
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDie();
        CheckForCollision();
    }
   
    public void CheckForCollision()
    {
        foreach (EnemyHitbox collider in colliderList) 
        {
            if (collider.hit == true)
            {
                healthbar.health -= PlayerAttack.instance.damage;
                EnemySpawner.instance.ShowDMGText(PlayerAttack.instance.damage, transform);
                collider.hit = false;
            }
        }
    }
    public void EnemyDie()
    {
        if (healthbar.health == 0)
        {
            dead = true;
            capsuleCollider.isTrigger = true;
            animator.SetBool("die", dead);
            transform.GetComponentInChildren<Canvas>().enabled = false;  
        }
    }
    public void CheckEnemy() 
    {
        if (modelint == 0)
        {
            capsuleCollider.height = 7;
            capsuleCollider.center = Vector3.up * 3.5f;
            hpPos = Vector3.up * 8;
        }
        if (modelint == 1)
        {
            capsuleCollider.height = 10;
            capsuleCollider.center = Vector3.up * 5;
            hpPos = Vector3.up * 11;
        }
    }
}
