﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RangedDrone : Enemy
{
    [SerializeField]
    private Weapon weapon;
    [SerializeField]
    private float AttackRange = 8;

    private NavMeshAgent navAgent;
    private LerpRotationToTarget rotator;
    private Transform playerTransform;

    protected override void Awake()
    {
        base.Awake();
        navAgent = GetComponent<NavMeshAgent>();
        rotator = GetComponentInChildren<LerpRotationToTarget>();
        playerTransform = PlayerCharacter.Instance.transform;
    }

    private void Start()
    {
        StartCoroutine(SetMoveTargetPeriodic());
    }

    private void Update()
    {
        rotator.TargetPosition = playerTransform.position;
        float angleBetween = Vector3.Angle(rotator.transform.forward,
            playerTransform.position - transform.position);
        if (angleBetween <= 22.5f && Vector3.Distance(transform.position,
            playerTransform.position) <= AttackRange)
        {
            weapon.AttemptFireWeapon();
        }
    }

    public override void Die()
    {
        GetComponent<SparePartsGenerator>()?.GenerateSpareParts();
        base.Die();
    }

    private IEnumerator SetMoveTargetPeriodic()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position,
                playerTransform.position) > AttackRange)
            {
                navAgent.destination = playerTransform.position;
            }
            yield return new WaitForSeconds(1f);

        }
    }
}