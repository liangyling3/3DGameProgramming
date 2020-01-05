using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class Example : MonoBehaviour
{
    public Slider mainSlider;
    public float HP;

    private void Start()
    {
        //mainSlider.value = mainSlider.maxValue;
        mainSlider = GetComponent<Slider>();
        HP = mainSlider.maxValue;
    }

    void OnGUI()
    {
        if( GUI.Button(new Rect(300,200,80, 40), "红方加血")) {
            HP += 10;
        }
        if (GUI.Button(new Rect(300, 300, 80, 40), "红方减血")) {
            HP -= 10;
        }

        if (HP > 100) HP = 100;
        else if (HP < 0) HP = 0;

        mainSlider.value = HP;
    }
}