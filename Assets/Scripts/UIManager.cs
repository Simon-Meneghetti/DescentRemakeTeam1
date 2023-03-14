using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject TaserPanel;
    public Image StaminaBar;
    public Image StaminaBar1;
    public Image O2Bar;
    public Image ShieldBar;

    Muzzle m;
    PlayerStats pS;
    void Start()
    {
        m = FindObjectOfType<Muzzle>();
        pS = FindObjectOfType<PlayerStats>();
    }

    
    void Update()
    {
        StaminaBar.fillAmount = m.stamina/m.maxStamina; 
        StaminaBar1.fillAmount = m.stamina/m.maxStamina; 
        O2Bar.fillAmount = pS.oxigen/pS.maxO2; 
        ShieldBar.fillAmount = pS.shield/pS.maxShield; 
    }
}
