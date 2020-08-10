using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hero : CharacterBase
{

    private Animator animator;
    private PlayerMovement pm;
    private AttackComponent ac;

    [SerializeField]
    private Sprite heroFrame;
    private Image XP_UI;
    [Header("HERO PANEL")]
    [SerializeField]
    private TextMeshProUGUI TXT_HeroName;
    [SerializeField]
    private Image heroFramePic;


    private void Start()
    {
        animator = this.GetComponent<Animator>();
        pm = this.GetComponent<PlayerMovement>();
        ac = this.GetComponent<AttackComponent>();
        if (this.GetComponent<NetworkIdentity>().isLocalPlayer) {
            heroFramePic = GameObject.FindGameObjectWithTag("HERO_HUD").GetComponent<HeroHudManager>().heroFrame.GetComponent<Image>();
            TXT_HeroName = GameObject.FindGameObjectWithTag("HERO_HUD").GetComponent<HeroHudManager>().heroName.GetComponent<TextMeshProUGUI>();
            heroFramePic.sprite = heroFrame;
            TXT_HeroName.text = this.name;
        }
        

    }
    public override void Die()
    {
        animator.SetBool("isDead", true);
        pm.enabled = false;
        ac.enabled = false;
        base.Die();

        // Play Hero death Animation
    }
}
