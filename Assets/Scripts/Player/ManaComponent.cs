using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class ManaComponent : NetworkBehaviour
{
    private CharacterBase characterBase;

    private int maxMana = 100;
    [SyncVar(hook = nameof(OnManaChange))]
    public int currentMana = 0;

    [SyncVar]
    private bool isDead = false;

    [SerializeField]
    private Image UI_MANA_BAR;
    [SerializeField]
    private Image UI_MAIN_MANA_BAR;
    [SerializeField]
    private GameObject UI_CANVAS;
    [SerializeField]
    private float updateBarTime = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        UI_MAIN_MANA_BAR = GameObject.FindGameObjectWithTag("HERO_HUD").GetComponent<HeroHudManager>().manaBar.GetComponent<Image>();
        this.UI_MAIN_MANA_BAR.fillAmount = 1;        
        characterBase = this.GetComponent<CharacterBase>();
        maxMana = characterBase.stats.maxMana.getValue();
        currentMana = maxMana;
        this.UI_MANA_BAR.fillAmount = 1; // Make sure UI always start at one                    
        StartCoroutine("UpdateUI");
    }

    // Check if the Entity is dead
    public bool IsDead()
    {
        return this.isDead;
    }


    // Just Spent Mana. Called on the server to decrease the SYNC VAR
    public void SpentMana(int amout)
    {
        currentMana -= amout;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }

    // Called on the clients whenever the SYNCVAR health is changed by the server
    public void OnManaChange(int oldValue, int newValue)
    {
        int manaSpent = oldValue - newValue;
        StartCoroutine("UpdateUI");

        // update UI
        if (currentMana <= 0)
        {
            currentMana = 0; // Do not allow less than 0 mana
            Debug.Log(this.gameObject.name + " is out of mana");
        }
    }

    // Just update the health bar UI with smooth animation
    IEnumerator UpdateUI()
    {
        float cachedValue = this.UI_MANA_BAR.fillAmount;
        float newValue = (float)currentMana / (float)maxMana;
        float elapsed = 0;

        while (elapsed < updateBarTime)
        {
            elapsed += Time.deltaTime;
            this.UI_MANA_BAR.fillAmount = Mathf.Lerp(cachedValue, newValue, elapsed / updateBarTime);
            if (isLocalPlayer && this.UI_MAIN_MANA_BAR != null)
            {
                this.UI_MAIN_MANA_BAR.fillAmount = Mathf.Lerp(cachedValue, newValue, elapsed / updateBarTime);
            }
            yield return null;

        }

        this.UI_MANA_BAR.fillAmount = newValue;
        if(isLocalPlayer && this.UI_MAIN_MANA_BAR != null)
            this.UI_MAIN_MANA_BAR.fillAmount = newValue;

    }

    private void LateUpdate()
    {
        if (UI_CANVAS != null)
        {
            UI_CANVAS.transform.LookAt(Camera.main.transform);
            UI_CANVAS.transform.Rotate(0, 180, 0);
        }
    }
}
