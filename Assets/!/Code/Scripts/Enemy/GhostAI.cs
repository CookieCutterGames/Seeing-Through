using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostAI : MonoBehaviour
{
    enum GhostState
    {
        Patrol,
        Chase,
        Search,
    }

    public float chaseDistance = 5f;
    public float movementSpeed = 2f;
    public float chaseSpeed = 4f;
    public float searchSpeed = 0.5f;
    public float searchDuration = 3f;

    public LayerMask obstacleMask;

    [Header("Decoy Settings")]
    public AudioClip[] decoySounds;
    public float decoySpawnDistance = 2f;
    public float decoyLifetime = 5f;
    public float decoyCooldown = 30f;

    private GhostState currentState = GhostState.Patrol;
    private Transform player;

    private List<Vector3> patrolPoints = new List<Vector3>();
    private int currentPatrolIndex = 0;
    private float searchTimer = 0f;
    private float nextDecoyTime = 0f;

    public float maxChaseTime = 5f;
    private float chaseTimer = 0f;

    public bool freezed = false;
    public bool isDecoy = false;

    private float chaseCooldownTimer = 0f;
    public float chaseCooldownDuration = 3f;
    private bool canChase = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        UserInput.Instance._attack2Action.performed += OnPlayerUsedSpell;
        GeneratePatrolPoints();
        nextDecoyTime = Time.time + decoyCooldown; // pierwsze odliczanie do decoya
    }

    private void OnDestroy()
    {
        UserInput.Instance._attack2Action.performed -= OnPlayerUsedSpell;
    }

    private void OnPlayerUsedSpell(InputAction.CallbackContext ctx)
    {
        currentState = GhostState.Chase;
        chaseTimer = 0f;
    }

    private void Update()
    {
        if (freezed || isDecoy)
            return;

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            Player.Instance.transform.position
        );
        if (Time.time >= nextDecoyTime)
        {
            CreateDecoy();
            nextDecoyTime = Time.time + decoyCooldown;
        }
        if (!canChase)
        {
            chaseCooldownTimer += Time.deltaTime;
            if (chaseCooldownTimer >= chaseCooldownDuration)
            {
                canChase = true;
                chaseCooldownTimer = 0f;
            }
        }

        switch (currentState)
        {
            case GhostState.Patrol:
                PatrolBehavior();
                if (canChase && distanceToPlayer < chaseDistance)
                {
                    currentState = GhostState.Chase;
                    chaseTimer = 0f;
                    canChase = false;
                }
                break;

            case GhostState.Chase:
                ChaseBehavior();
                chaseTimer += Time.deltaTime;
                if (chaseTimer >= maxChaseTime || distanceToPlayer > chaseDistance * 1.5f)
                {
                    currentState = GhostState.Patrol;
                    chaseTimer = 0f;
                    canChase = false;
                }
                break;

            case GhostState.Search:
                SearchBehavior();
                break;
        }
    }

    private void PatrolBehavior()
    {
        if (patrolPoints.Count == 0)
            GeneratePatrolPoints();

        MoveTowards(patrolPoints[currentPatrolIndex], movementSpeed);

        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex]) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
    }

    private void ChaseBehavior()
    {
        MoveTowards(player.position, chaseSpeed);
    }

    private void SearchBehavior()
    {
        // Prosta implementacja poszukiwania: poruszanie się powoli wokół gracza
        searchTimer += Time.deltaTime;
        float radius = 1f;
        float speed = 1f;
        float angle = Time.time * speed;
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
        Vector3 targetPos = player.position + offset;
        MoveTowards(targetPos, searchSpeed);

        if (searchTimer >= searchDuration)
        {
            searchTimer = 0f;
            currentState = GhostState.Patrol;
        }
    }

    private void CreateDecoy()
    {
        GameObject decoy = Instantiate(
            gameObject,
            transform.position
                + (player.position - transform.position).normalized * decoySpawnDistance,
            transform.rotation
        );
        decoy.name = "GhostDecoy";

        // Usuwamy komponent AI z decoya
        Destroy(decoy.GetComponent<GhostAI>());

        // Ustawiamy przezroczystość SpriteRenderer
        SpriteRenderer sr = decoy.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0.5f;
            sr.color = c;
        }

        // Odtwarzamy losowy dźwięk z puli
        if (decoySounds.Length > 0)
        {
            int index = Random.Range(0, decoySounds.Length);
            AudioManager.Instance.PlaySound(decoySounds[index]);
        }

        Destroy(decoy, decoyLifetime);
    }

    private void GeneratePatrolPoints()
    {
        patrolPoints.Clear();
        Vector3 basePos = Player.Instance.transform.position;

        for (int i = 0; i < 3; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 15f;
            Vector3 randomPoint = basePos + new Vector3(randomOffset.x, randomOffset.y, 0);
            patrolPoints.Add(randomPoint);
        }
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        Vector3 newPos = transform.position + direction * speed * Time.deltaTime;

        if (!Physics2D.OverlapCircle(newPos, 0.2f, obstacleMask))
        {
            transform.position = newPos;
        }
    }
}
