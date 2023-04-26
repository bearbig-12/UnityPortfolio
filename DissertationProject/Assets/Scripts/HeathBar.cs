using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeathBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] 
    private Image healthBarImage;
    public void UpdateHPBar(float Max_HP, float Current_HP)
    {
        healthBarImage.fillAmount = Current_HP / Max_HP;
    }


}
