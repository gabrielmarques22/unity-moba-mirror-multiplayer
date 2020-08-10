using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterBase))]
public class HealthComponent : NetworkBehaviour
{
    private CharacterBase characterBase;

    private int maxHealth = 100;
    [SyncVar(hook = nameof(OnHealthChange))]
    public int currentHealth = 0;

    [SyncVar]
    private bool isDead = false;

    [SerializeField]
    private Image UI_HP_BAR;
    [SerializeField]
    private Image UI_MAIN_HP_BAR;
    [SerializeField]
    private GameObject UI_CANVAS;
    [SerializeField]
    private float updateBarTime = 0.1f;

    public override void OnStartLocalPlayer()
    {
        UI_MAIN_HP_BAR = GameObject.FindGameObjectWithTag("HERO_HUD").GetComponent<HeroHudManager>().healthBar.GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {

        characterBase = this.GetComponent<CharacterBase>();
        maxHealth = characterBase.stats.maxHealth.getValue();
        currentHealth = maxHealth;
        if(hasAuthority)
            this.UI_MAIN_HP_BAR.fillAmount = 1;
        this.UI_HP_BAR.fillAmount = 1; // Make sure UI starts at 1
        StartCoroutine("UpdateUI");



    }

    // Check if the Entity is dead
    public bool IsDead()
    {
        return this.isDead;
    }


    // Just Apply Damage. Called By the Server. Current value is basically character damage - enemy armor
    public void TakeDamage(int amout)
    {
        amout -= characterBase.stats.armor.getValue();
        currentHealth -= amout;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void Heal(int amout)
    {
        currentHealth += amout;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // Called on the clients whenever the SYNCVAR health is changed by the server
    public void OnHealthChange(int oldValue, int newValue)
    {
        int damage = oldValue - newValue;
       //Debug.Log(netId + " has taken " + damage + " damage");
        StartCoroutine("UpdateUI");

        // update UI
        if (currentHealth <= 0)
        {
            this.isDead = true;
            Debug.Log(netId + " has been killed");
            // notify clients that the character died. Maybe should be trigger as a RPC from the server?
            this.GetComponent<CharacterBase>().Die();

        }
    }

    // Just update the health bar UI with smooth animation
    IEnumerator UpdateUI()
    {
        float cachedValue = this.UI_HP_BAR.fillAmount;
        float newValue = (float)currentHealth / (float)maxHealth;
        float elapsed = 0;

        while (elapsed < updateBarTime)
        {
            elapsed += Time.deltaTime;
            this.UI_HP_BAR.fillAmount = Mathf.Lerp(cachedValue, newValue, elapsed / updateBarTime);
            if(isLocalPlayer)
                this.UI_MAIN_HP_BAR.fillAmount = Mathf.Lerp(cachedValue, newValue, elapsed / updateBarTime);
            yield return null;

        }

        if (newValue <= .5f && newValue > .25f)
        {
            this.UI_HP_BAR.color = Color.yellow;
        } else if (newValue <= .25f)
        {
            this.UI_HP_BAR.color = Color.red;
        } else
        {
            this.UI_HP_BAR.color = Color.green;
        }
        this.UI_HP_BAR.fillAmount = newValue;
        if (isLocalPlayer)
            this.UI_MAIN_HP_BAR.fillAmount = newValue;

    }

    private void LateUpdate()
    {
        if(UI_CANVAS != null)
        {
            UI_CANVAS.transform.LookAt(Camera.main.transform);
            UI_CANVAS.transform.Rotate(0, 180, 0);
        }
    }
}
