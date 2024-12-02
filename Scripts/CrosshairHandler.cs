using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairHandler : MonoBehaviour
{
    public Sprite[] crosshairs;
    private Image image;
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        SetCrosshair();
    }

    private void SetCrosshair()
    {
        image.sprite = crosshairs[index];
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            index++;
            if (index == crosshairs.Length)
            {
                index = 0;
            }
        }
    }
}
