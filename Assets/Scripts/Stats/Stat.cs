using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Base Stat Class, responsible for creating multiple stats, such as str, vit, speed, etc. Has Add and Remove modifiers 
// functionality, so Skills (buff/debuff) and also itens can interact with player stats.
// @todo Might be a good idea to move this management to server later.

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int baseValue;

    private List<int> modifiers = new List<int>();

    public int getValue()
    {
        int finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifier(int modifier)
    {
        if (modifier == 0) return;

        modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier == 0) return;

        modifiers.Remove(modifier);
    }
}
