using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NumberUpdate : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public Slider slider;
    
    public void OnSliderValueChanged()
    {
        numberText.text = slider.value + "";
    }
}
