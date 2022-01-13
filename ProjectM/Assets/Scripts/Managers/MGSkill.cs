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

    public void UpdateSkill()
    {
        ManaFill();
    }

    public bool ManaReduce(int value)
    {
        if (mana >= value)
        {
            mana -= value;
            OnManaChanged(mana);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ManaFill()
    {
        manaCooldown += Time.deltaTime;

        if (manaCooldown > manaDelay)
        {
            manaCooldown -= manaDelay;

            if (mana < maxMana)
            {
                mana++;
                OnManaChanged(mana);
            }
        }
    }

    public void Skill1Active()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<HealthSystem>().Damage(50);
        }
    }

    public void Skill2Active()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].transform.position += new Vector3(10, 0, 0);
        }
    }
}
