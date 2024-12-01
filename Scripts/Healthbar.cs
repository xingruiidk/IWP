using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public Image healthbar;
    public Image healthbar2;
    public TMP_Text hptxt;
    private float initialScale;
    private float fadeInDuration = 2f;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        initialScale = healthbar.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
    }
    public void UpdateHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        float scaleValue = health / maxHealth;
        healthbar.transform.localScale = new Vector3(scaleValue * initialScale, healthbar.transform.localScale.y, healthbar.transform.localScale.z) ;
        float targetScaleValue = healthbar.transform.localScale.x;
        float lerpedScale = Mathf.Lerp(healthbar2.transform.localScale.x, targetScaleValue, Time.deltaTime * fadeInDuration);
        healthbar2.transform.localScale = new Vector3(lerpedScale, 1, 1);
        hptxt.text = health + "/" + maxHealth;
    }
}
