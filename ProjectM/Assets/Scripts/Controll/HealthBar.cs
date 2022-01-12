using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private HealthSystem healthSystem;

    private Transform barTrm;

    private void Awake()
    {
        barTrm = transform.Find("Bar");
    }

    void Start()
    {
        healthSystem.OnDamaged += CallHealthSystemOnDamaged;

        CallHealthSystemOnDamaged();
    }

    void CallHealthSystemOnDamaged()
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar()
    {
        barTrm.localScale = new Vector3(healthSystem.GetHealthAmoutNomalized(), 1, 1);
    }

    private void UpdateHealthBarVisible()
    {
        gameObject.SetActive(!healthSystem.IsFullHealth());
    }
}
