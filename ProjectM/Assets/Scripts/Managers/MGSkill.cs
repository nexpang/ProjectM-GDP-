using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MGSkill : MonoBehaviour
{
    public Action<int> OnManaChanged;

    private const int maxMana = 20;
    public int mana = 0;

    private const int manaDelay = 1;
    private float manaCooldown = 0;

    public MGSkill()
    {
        
    }

    public void ManaFill()
    {
        manaCooldown += Time.deltaTime;

        if(manaCooldown > manaDelay)
        {
            manaCooldown -= manaDelay;

            if (mana < maxMana)
            {
                mana++;
                OnManaChanged(mana);
            }
        }
    }
}
