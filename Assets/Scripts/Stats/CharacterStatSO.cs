using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Characters ScriptableObject that holds Stats and loads to Character Base. Separate class so we can balance the game remotely later.
[CreateAssetMenu(fileName = "New Character Stat", menuName = "UG - New Character Stats", order = 1)]
[System.Serializable]
public class CharacterStatSO : ScriptableObject
{
    [SerializeField]
    public Stat maxHealth, damage, armor, movSpeed;
}
