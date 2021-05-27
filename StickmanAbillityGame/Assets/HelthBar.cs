using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelthBar : MonoBehaviour
{
    public bool everyoneCanSee = false;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject self;
    public PhotonView photonView;
    private void Start()
    {
        if (everyoneCanSee == false && !photonView.isMine)
        {
            self.SetActive(false);
        }
    }
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}