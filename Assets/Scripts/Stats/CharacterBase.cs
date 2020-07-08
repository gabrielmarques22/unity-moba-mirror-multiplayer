using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public new string name;
    [SerializeField]
    bool isRanged = false;

    public CharacterStatSO stats;
 
    public enum GROUP {
        GREEN,
        PURPLE
    };
    GROUP group = GROUP.GREEN;

    public bool IsRanged()
    {
        return this.isRanged;
    }

    // This is called by the Health Component only when life reaches 0
    public virtual void Die()
    {
        // Override this method for each character
        Debug.Log("Character " + name + " Died. ");
    }
}
