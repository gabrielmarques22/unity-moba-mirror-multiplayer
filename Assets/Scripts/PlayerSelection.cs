using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MessageBase
{
    public string heroName = "Jonas";

    public void ChangeHeroName(string _heroName)
    {
        this.heroName = _heroName;
    }
}
