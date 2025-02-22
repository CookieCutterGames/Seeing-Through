using System;
using UnityEngine;

[Serializable]
public class BehaviourStateHandler
{
    #region Patroling
    public Vector3[] patrolPoints;
    public int currentPatrolPointIndex = 0;
    #endregion

    public float listeningTime = 3f;
    public float elapsedListeningTime = 0f;

    public float movementSpeed = 0.5f;
    public bool executingBehaviour = false;
    public Enemy parent;

    public void Update()
    {
        HandleBehaviour();
    }

    public void HandleBehaviour()
    {
        if (executingBehaviour)
            return;
        switch (parent.CurrentState)
        {
            case BehaviourState.Patrol:

                if (patrolPoints.Length == 0)
                    break;

                if (
                    Vector2.Distance(
                        parent.gameObject.transform.position,
                        patrolPoints[currentPatrolPointIndex]
                    ) < 0.1f
                )
                    currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;

                parent.gameObject.transform.position = Vector2.MoveTowards(
                    parent.gameObject.transform.position,
                    patrolPoints[currentPatrolPointIndex],
                    movementSpeed * Time.deltaTime
                );
                break;
            case BehaviourState.Listening:
                if (elapsedListeningTime >= listeningTime)
                {
                    elapsedListeningTime = 0f;

                    parent.CurrentState = BehaviourState.Chase;
                }
                else
                {
                    elapsedListeningTime += Time.deltaTime;
                }
                break;
            case BehaviourState.Chase:
                //tutaj warto potem rozkminić czy nie lepiej robić jakis safeDistance na colliderze i bazować na tym niż na jakiejś statycznej wartości
                if (
                    Vector2.Distance(parent.gameObject.transform.position, parent.target.position)
                    < 1f
                )
                    parent.CurrentState = BehaviourState.Stand;

                parent.gameObject.transform.position = Vector2.MoveTowards(
                    parent.gameObject.transform.position,
                    parent.target.position,
                    movementSpeed * Time.deltaTime
                );
                break;
            case BehaviourState.Lost:
                //dokończyć mechanike w przypadku "braku słyszenia" u przeciwnika naszej bohaterki
                parent.CurrentState = BehaviourState.Patrol;
                break;
            case BehaviourState.Stand:
                break;
        }
    }
}
