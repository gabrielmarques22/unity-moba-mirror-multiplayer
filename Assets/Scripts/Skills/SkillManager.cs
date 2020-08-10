using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : NetworkBehaviour
{
    [SerializeField]
    private Skill skill_Q;
    [SerializeField]
    public  GameObject skill_Q_GO;


    private bool canCast_Q = true, inUse_Q = false;
    private float cooldown_Q = 0;

    private ManaComponent manaComponent;
    
    [SerializeField] LayerMask groundLayer;
    private GameObject skillIndicator;
    private bool aimingSkill = false;
    

    // UI
    [Header("HERO PANEL")]
    [SerializeField]
    private Image UI_SKILL_Q, UI_SKILL_Q_USED;

    public override void OnStartLocalPlayer()
    {
        if (hasAuthority)
        {
            skillIndicator = GameObject.Find("SkillIndicatorPivot");
            skillIndicator.SetActive(false);
        }
        UI_SKILL_Q = GameObject.FindGameObjectWithTag("HERO_HUD").GetComponent<HeroHudManager>().skillQ.GetComponent<Image>();
        UI_SKILL_Q_USED = GameObject.FindGameObjectWithTag("HERO_HUD").GetComponent<HeroHudManager>().skillQ_Used.GetComponent<Image>();

        UI_SKILL_Q.sprite = skill_Q.GetIcon();
        UI_SKILL_Q_USED.sprite = skill_Q.GetIcon();
        manaComponent = this.GetComponent<ManaComponent>();
    }
    private void Update()
    {
        if (isLocalPlayer)
        {
            Q_Input();
        }     
        


    }

    public void Q_Input()
    {
        if (inUse_Q)
        {
            cooldown_Q += Time.deltaTime;
            UI_SKILL_Q.fillAmount = cooldown_Q / skill_Q.coolDown;

            if (cooldown_Q >= skill_Q.coolDown)
            {
                cooldown_Q = 0;
                canCast_Q = true;
                inUse_Q = false;
            }
        }

        if (aimingSkill)
        {
            if(skillIndicator != null && !skillIndicator.activeSelf) skillIndicator.SetActive(true);
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, groundLayer))
            {
                Debug.Log(hit.collider.gameObject.layer);
                if (hit.collider.gameObject.layer != 9)
                    return;
                float y = hit.point.y + 0.3f;
                y = Mathf.Clamp(y, 0, 1);
                skillIndicator.transform.position = new Vector3(hit.point.x, y, hit.point.z);
            }
            if (Input.GetMouseButtonDown(0))
            {
                this.CastSkillQ(); // @todo change to be dynamic based on the skill button pressed (Q, W, E, R)
                transform.LookAt(hit.transform);

                aimingSkill = false;
                skillIndicator.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (skill_Q.isInstantCast)
            {
                this.CastSkillQ();


            } else
            {
                aimingSkill = true;
                skillIndicator.SetActive(true);
            }
            

        } 
    }

    public void CastSkillQ()
    {
        if (manaComponent.currentMana >= skill_Q.manaCost)
        {
            if (canCast_Q && !inUse_Q)
            {
                inUse_Q = true;
                canCast_Q = false;
                UI_SKILL_Q.fillAmount = 0;
                // Cast Skill
                CmdCastSkill("Q", this.netIdentity);
            }
            else
            {
                Debug.Log("Not enough Mana");
            }
        }
    }

    [Command]
    public void CmdCastSkill(string inputSkill, NetworkIdentity player)
    {
        SkillGameObject skillGO;
        if (inputSkill == "Q")
        {
            GameObject instantiate = Instantiate(player.GetComponent<SkillManager>().skill_Q_GO, player.transform.position, player.transform.rotation);
            NetworkServer.Spawn(instantiate);

            skillGO = instantiate.GetComponent<SkillGameObject>();
            skillGO.cachedPlayer = player.gameObject;
            skillGO.TriggerSkill();

            player.gameObject.GetComponent<ManaComponent>().SpentMana(skillGO.skill.manaCost);
            
        }

    }


}
