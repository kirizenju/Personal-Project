using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagDollSpawner : MonoBehaviour
{
    [SerializeField] private Transform ragDollPrefab;
    [SerializeField] private Transform ragDollRootBone;

    private HealthSystem healthSystem;
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform ragDollTranasform =
        Instantiate(ragDollPrefab,transform.position,transform.rotation);
        UnitRagDoll ragDoll=ragDollTranasform.GetComponent<UnitRagDoll>();
        ragDoll.SetUp(ragDollRootBone);
    }
}
