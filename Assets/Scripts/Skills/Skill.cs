using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="United Batte Arena/Skills/New Skill")]
[System.Serializable]
public  class Skill : ScriptableObject
{

    public string skillName; // Name of the skill
    public string description; // Skill Description
    public int manaCost; // Mana cost to use the skill
    public float coolDown; // Cooldown to reuse the skill
    public float castDuration; // Time between the use of the skill and the animation finish
    public float skillDuration; // Time the skill remains active. Eg. giving buff or impacting an Area
    public int baseDamage; // Base damage to be applied or healed
    public bool isInstantCast; // Tells if the skill should be instant casted when button is pressed or it's skillshot
    [SerializeField]
    private Sprite icon; // icon to show on UI

    public Sprite GetIcon()
    {
        return icon;
    }

    public virtual void Initialize(GameObject obj) { }
    public virtual void Finish(GameObject obj) { }

    public virtual void TriggerSkill() { }



}
