using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public bool hit;
    // Start is called before the first frame update
    void Start()
    {
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BloodSword")
            hit = true;
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BloodSword")
            hit = false;
    }
}
