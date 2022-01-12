using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : CONEntity
{
    public Action OnDamaged;
    public Action OnDied;

    [SerializeField]
    private int healthAmountMax = 100;

    private int curHealthMount;

    private void Awake()
    {
        curHealthMount = healthAmountMax;
    }

    public void Damage(int damageAmount)
    {
        curHealthMount -= damageAmount;
        curHealthMount = Mathf.Clamp(curHealthMount, 0, healthAmountMax);

        OnDamaged?.Invoke();

        if (isDead())
        {
            OnDied?.Invoke();
        }
    }

    public bool isDead()
    {
        return curHealthMount == 0;
    }

    public bool IsFullHealth()
    {
        return curHealthMount == healthAmountMax;
    }

    public int GetHealthAmount()
    {
        return curHealthMount;
    }

    public float GetHealthAmoutNomalized()
    {
        return (float)curHealthMount / healthAmountMax;
    }

    public void SetHealthAmountMax(int hpAmountMax, bool updateHpAmout)
    {
        this.healthAmountMax = hpAmountMax;
        if (updateHpAmout)
        {
            curHealthMount = hpAmountMax;
        }
    }
}
