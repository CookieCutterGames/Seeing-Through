using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private BehaviourState currentState = BehaviourState.Stand;

    public BehaviourState CurrentState
    {
        get => currentState;
        set => currentState = value;
    }

    public Transform target;

    public BehaviourStateHandler behaviourStateHandler;

    private void InitializeBehaviourStateHandler()
    {
        behaviourStateHandler.parent = this;
    }

    void Awake()
    {
        InitializeBehaviourStateHandler();
    }

    void Update()
    {
        behaviourStateHandler.Update();
    }

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }
}
