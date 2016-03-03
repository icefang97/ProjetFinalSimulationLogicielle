using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{

    public enum State { Idle, Chasing, Attacking };
    public GameObject team;

    GameObject player;
    State currentState;

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;

    Color originalColour;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    float damage = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;
    float refreshRate;



    bool hasTarget;
    public bool hasEnded;

    protected override void Start()
    {
        player = GameObject.Find("player");
        hasEnded = false;
        refreshRate = Random.Range(50, 100);
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColour = skinMaterial.color;

        if (team.tag == "Red")
        {
            if (GameObject.FindGameObjectWithTag("Red") != null)
            {
                currentState = State.Chasing;
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Blue") != null)
            {
                currentState = State.Chasing;
            }

        }
    }

    void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAttackTime && target != null)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());

                }

            }
            StartCoroutine(UpdatePath());
        }
        StartCoroutine(Targeting());
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    //chercher la cible la plus proche
    GameObject GetNearestEnnemie()
    {
        GameObject nearest = null;
        if (team.tag == "Red")
        {
            foreach (GameObject ennemy in GameObject.FindGameObjectsWithTag("Blue"))
            {
                if (nearest == null || Vector3.Distance(transform.position, ennemy.transform.position) < Vector3.Distance(transform.position, nearest.transform.position))
                {
                    nearest = ennemy;
                }
            }
        }
        else
        {
            foreach (GameObject ennemy in GameObject.FindGameObjectsWithTag("Red"))
            {
                if (nearest == null || Vector3.Distance(transform.position, ennemy.transform.position) < Vector3.Distance(transform.position, nearest.transform.position))
                {
                    nearest = ennemy;
                }
            }
        }
        return nearest;
    }

    //la cible du soldat
    IEnumerator Targeting()
    {
        refreshRate = refreshRate / 100;
        hasTarget = true;
        target = GetNearestEnnemie().transform;
        targetEntity = target.GetComponent<LivingEntity>();
        targetEntity.OnDeath += OnTargetDeath;

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        currentState = State.Chasing;
        yield return new WaitForSeconds(0.5f);
    }
    //La routine d'attaque d'un soldat
    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        skinMaterial.color = originalColour;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }
    //Quand l'enemy update son pathfinding
    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
