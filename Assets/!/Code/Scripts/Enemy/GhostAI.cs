using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CircleCollider2D))]
public class GhostAI : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Search,
    }

    public enum GhostSoundType
    {
        Teleport,
        FakeTeleport,
        Laugh,
        Freeze,
    }

    [Header("Base Settings")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float searchSpeed = 0.5f;
    public float detectionRadius = 5f;
    public float maxDetectionRadius = 8f;
    public float searchDuration = 10f;
    public List<Transform> patrolWaypoints;

    [Header("Deception Settings")]
    public float minDeceptionDistance = 3f;
    public float maxDeceptionDistance = 8f;

    [Header("Ghost Mechanics")]
    public float teleportCooldown = 15f;
    public float deceptionChance = 0.3f;
    public float hideDetectionChance = 0.2f;
    public float hideSpotSlowdown = 0.5f;
    public float minTeleportDistance = 3f;
    public float maxTeleportDistance = 8f;

    [Header("Psychological Effects")]
    public float whisperIntensity = 0.5f;
    public AudioClip[] whisperClips;
    public float effectUpdateRate = 0.2f;
    public LayerMask lineOfSightLayers;

    [Header("Dynamic Waypoints")]
    public float waypointUpdateInterval = 5f;
    public int maxWaypoints = 8;
    public float minWaypointDistance = 3f;
    public float waypointGenerationRadius = 10f;

    // Private variables

    [SerializeField]
    private List<Vector2> dynamicWaypoints = new List<Vector2>();

    [SerializeField]
    private List<Vector2> teleportAnchors = new List<Vector2>();
    private float lastWaypointUpdateTime;

    [SerializeField]
    private State currentState;
    private Transform player;
    private Vector2 lastKnownPosition;
    private int currentWaypointIndex;
    private bool isSearching;
    private float lastTeleportTime;
    private Vector3 lastPlayerHidePosition;
    private bool isDeceiving;
    private AudioSource audioSource;
    private Rigidbody2D rb;
    private NoiseHandler playerNoiseHandler;
    private float nextEffectTime;

    public static GhostAI Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerNoiseHandler = player.GetComponentInChildren<NoiseHandler>();
        audioSource = GetComponent<AudioSource>();
        currentState = State.Patrol;
        isDeceiving = false;
        StartCoroutine(PsychologicalEffectsRoutine());
        StartCoroutine(UpdateWaypointsRoutine());
    }

    void Update()
    {
        // HandleGhostParanormalAbilities();

        switch (currentState)
        {
            case State.Patrol:
                PatrolBehavior();
                CheckForPlayer();
                break;

            case State.Chase:
                ChaseBehavior();
                break;

            case State.Search:
                SearchBehavior();
                break;
        }
    }

    #region Core AI Functions
    void PatrolBehavior()
    {
        if (dynamicWaypoints.Count == 0)
            return;
        Vector2 targetPos = dynamicWaypoints[0];
        float minDist = float.MaxValue;

        foreach (Vector2 waypoint in dynamicWaypoints)
        {
            float distToPlayer = Vector2.Distance(waypoint, player.position);
            float distToGhost = Vector2.Distance(waypoint, rb.position);

            if (distToPlayer < minWaypointDistance)
                continue;

            if (distToGhost < minDist)
            {
                minDist = distToGhost;
                targetPos = waypoint;
            }
        }

        Vector2 direction = (targetPos - rb.position).normalized;
        rb.linearVelocity = direction * patrolSpeed;

        if (Vector2.Distance(rb.position, targetPos) < 0.5f)
        {
            dynamicWaypoints.Remove(targetPos);
        }
    }

    void ChaseBehavior()
    {
        Vector2 direction = (lastKnownPosition - rb.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;

        if (Vector2.Distance(rb.position, lastKnownPosition) < 0.5f)
        {
            StartCoroutine(SearchRoutine());
        }
    }

    void SearchBehavior()
    {
        if (!isSearching)
            StartCoroutine(SearchRoutine());
    }

    IEnumerator SearchRoutine()
    {
        isSearching = true;
        currentState = State.Search;
        float timer = 0f;

        Vector2 searchCenter = lastKnownPosition;
        Vector2 currentSearchTarget = GetSearchPoint(searchCenter);

        while (timer < searchDuration)
        {
            Vector2 direction = (currentSearchTarget - rb.position).normalized;
            rb.linearVelocity = direction * searchSpeed;

            if (
                Vector2.Distance(rb.position, currentSearchTarget) < 0.5f
                || timer % 2f < Time.deltaTime
            )
            {
                currentSearchTarget = GetSearchPoint(searchCenter);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        currentState = State.Patrol;
        isSearching = false;
    }

    Vector2 GetSearchPoint(Vector2 center)
    {
        float searchRadius = Mathf.Min(searchDuration * 0.5f, 2f);
        float angle = Random.Range(0, 2 * Mathf.PI);

        float distance =
            Random.value > 0.3f
                ? Random.Range(searchRadius * 0.5f, searchRadius)
                : Random.Range(0, searchRadius * 0.3f);

        Vector2 point =
            center + new Vector2(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);

        RaycastHit2D hit = Physics2D.Raycast(center, (point - center).normalized, distance);
        if (hit.collider != null && !hit.collider.CompareTag("Player"))
        {
            point = hit.point - (hit.point - center).normalized * 0.5f;
        }

        return point;
    }

    IEnumerator UpdateWaypointsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waypointUpdateInterval);
            GenerateDynamicWaypoints();
            GenerateTeleportAnchors();
        }
    }

    void GenerateDynamicWaypoints()
    {
        dynamicWaypoints.Clear();

        // Główne waypointy wokół gracza
        for (int i = 0; i < maxWaypoints; i++)
        {
            Vector2 randomPos =
                (Vector2)player.position
                + Random.insideUnitCircle.normalized
                    * Random.Range(minWaypointDistance, waypointGenerationRadius);

            if (IsValidPosition(randomPos))
            {
                dynamicWaypoints.Add(randomPos);
            }
        }

        AddStrategicPositions();
    }

    void GenerateTeleportAnchors()
    {
        teleportAnchors.Clear();

        // Punkty teleportacyjne w miejscach, gdzie gracz często bywa
        List<Vector2> playerHistory = Player.Instance.playerMovement.GetRecentPositions(5);
        foreach (Vector2 pos in playerHistory)
        {
            // Dodaj punkty w losowej odległości od historycznych pozycji
            Vector2 anchorPos = pos + Random.insideUnitCircle.normalized * Random.Range(2f, 5f);
            if (IsValidPosition(anchorPos))
            {
                teleportAnchors.Add(anchorPos);
            }
        }

        // Dodaj punkty w przeciwnych kierunkach od gracza
        Vector2 oppositeDir = -((Vector2)player.position - rb.position).normalized;
        teleportAnchors.Add((Vector2)player.position + oppositeDir * 4f);
    }
    #endregion

    #region Paranormal Abilities
    void HandleGhostParanormalAbilities()
    {
        if (
            Time.time > lastTeleportTime + teleportCooldown
            && !PlayerInLineOfSight()
            && currentState != State.Chase
        )
        {
            // TryTeleport();
        }

        if (Random.value < deceptionChance * Time.deltaTime && !isDeceiving)
        {
            StartCoroutine(DeceptionRoutine());
        }
    }

    bool PlayerInLineOfSight()
    {
        if (player == null)
            return false;

        Vector2 directionToPlayer = player.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToPlayer,
            directionToPlayer.magnitude,
            lineOfSightLayers
        );

        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    void a()
    {
        if (teleportAnchors.Count == 0)
            return;

        // Wybierz punkt za graczem lub w strategicznym miejscu
        Vector2 targetPos = teleportAnchors[Random.Range(0, teleportAnchors.Count)];

        // 70% szans na teleport za graczem
        if (Random.value > 0.3f)
        {
            Vector2 playerForward = Player.Instance.playerMovement.GetFacingDirection();
            targetPos = (Vector2)player.position - playerForward * 3f;
        }

        if (IsValidPosition(targetPos))
        {
            lastTeleportTime = Time.time;
        }
    }

    void CreateDecoy(Vector3 position)
    {
        GameObject decoy = Instantiate(gameObject, position, Quaternion.identity);
        Destroy(decoy.GetComponent<GhostAI>());
        Destroy(decoy.GetComponent<Collider2D>());

        // Ustawienie przezroczystości sobowtóra
        if (decoy.TryGetComponent<SpriteRenderer>(out var decoyRenderer))
        {
            Color c = decoyRenderer.color;
            decoyRenderer.color = new Color(c.r, c.g, c.b, 0.6f);
        }

        Destroy(decoy, 3f);
    }

    #endregion

    #region Psychological Effects
    IEnumerator PsychologicalEffectsRoutine()
    {
        while (true)
        {
            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                float intensity = 1 - Mathf.Clamp01(distanceToPlayer / 10f);

                if (Random.value < intensity * whisperIntensity)
                {
                    PlayRandomWhisper();
                }

                if (distanceToPlayer < 4f)
                {
                    // CameraShaker.Instance.Shake(0.1f * intensity, 0.3f);
                }
            }

            yield return new WaitForSeconds(effectUpdateRate);
        }
    }

    void PlayRandomWhisper()
    {
        if (whisperClips.Length > 0 && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(whisperClips[Random.Range(0, whisperClips.Length)]);
        }
    }

    public void CreateTemporaryApparition(Vector3 position, float duration)
    {
        StartCoroutine(ApparitionRoutine(position, duration));
    }

    IEnumerator ApparitionRoutine(Vector3 position, float duration)
    {
        GameObject apparition = Instantiate(gameObject, position, Quaternion.identity);
        Destroy(apparition.GetComponent<GhostAI>());
        Destroy(apparition.GetComponent<Collider2D>());

        SpriteRenderer sr = apparition.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.7f);
        }

        yield return new WaitForSeconds(duration);

        if (apparition != null)
        {
            Destroy(apparition);
        }
    }
    #endregion

    #region Detection System
    void CheckForPlayer()
    {
        if (player == null || playerNoiseHandler == null)
            return;
        float currentDetectionRadius = detectionRadius + (playerNoiseHandler.NoiseLevel * 2);
        currentDetectionRadius = Mathf.Min(currentDetectionRadius, maxDetectionRadius);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, currentDetectionRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                if (playerNoiseHandler.IsMoving || playerNoiseHandler.IsHiding)
                {
                    lastKnownPosition = hit.transform.position;
                    currentState = State.Chase;
                }
            }
        }
    }

    IEnumerator DeceptionRoutine()
    {
        isDeceiving = true;

        // Losowy efekt dezorientujący
        int deceptionType = Random.Range(0, 4); // Zwiększyłem do 4 typów
        Vector3 deceptionPosition = GetDeceptionPosition(deceptionType);

        switch (deceptionType)
        {
            case 0: // Fałszywy dźwięk teleportu
                Debug.Log("fake teleport sound");
                // AudioManager.PlayGhostSound("FakeTeleport");
                break;

            case 1: // Tymczasowy sobowtór
                CreateDecoy(deceptionPosition);
                break;

            case 2: // Losowy śmiech
                Debug.Log("random laugh");
                // AudioManager.PlayGhostSound("Laugh");
                break;

            case 3: // Efekt wizualny (migające światło)
                StartCoroutine(FlickerLightsNearPlayer());
                break;
        }

        float deceptionDuration = Random.Range(2f, 5f);
        yield return new WaitForSeconds(deceptionDuration);

        isDeceiving = false;
    }

    IEnumerator FlickerLightsNearPlayer()
    {
        if (player == null)
            yield break;

        Light2D[] allLights = FindObjectsByType<Light2D>(FindObjectsSortMode.None);
        List<Light2D> nearbyLights = new();

        // Znajdź światła w pobliżu gracza
        foreach (Light2D light in allLights)
        {
            if (Vector2.Distance(light.transform.position, player.position) < 7f)
            {
                nearbyLights.Add(light);
            }
        }

        // Efekt migania
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            foreach (Light2D light in nearbyLights)
            {
                float originalIntensity = light.intensity;
                light.intensity = originalIntensity * 0.2f;
                yield return new WaitForSeconds(0.1f);
                light.intensity = originalIntensity;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public void InvestigateSound(Vector2 soundPosition)
    {
        lastKnownPosition = soundPosition;
        currentState = State.Chase;
    }
    #endregion

    #region Hide Spot Interaction
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("HideSpot") && playerNoiseHandler != null)
        {
            if (playerNoiseHandler.IsHiding)
            {
                lastPlayerHidePosition = other.transform.position;
                rb.linearVelocity *= 0.7f;

                // Check for discovery
                if (Random.value < hideDetectionChance * Time.deltaTime)
                {
                    lastKnownPosition = player.position;
                    currentState = State.Chase;
                }
            }
        }
    }
    #endregion

    #region Utility
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("touched player");
            // PlayerRespawn.RespawnPlayer();
            transform.position = patrolWaypoints[0].position;
            currentState = State.Patrol;
        }
    }

    Vector3 GetDeceptionPosition(int deceptionType)
    {
        if (player == null)
            return transform.position;

        if (deceptionType == 0 || deceptionType == 2)
        {
            return player.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
        }
        else
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            return player.position + (Vector3)randomDirection * Random.Range(3f, 6f);
        }
    }

    bool IsValidPosition(Vector2 position)
    {
        Collider2D hit = Physics2D.OverlapCircle(position, 0.5f);
        return hit == null || hit.CompareTag("Floor");
    }

    void AddStrategicPositions()
    {
        // Znajdź miejsca za przeszkodami
        Vector2[] checkDirections = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

        foreach (Vector2 dir in checkDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(player.position, dir, waypointGenerationRadius);
            if (hit.collider != null && !hit.collider.CompareTag("Player"))
            {
                Vector2 strategicPos = hit.point + dir * 0.5f;
                if (IsValidPosition(strategicPos))
                {
                    dynamicWaypoints.Add(strategicPos);
                    teleportAnchors.Add(strategicPos);
                }
            }
        }
    }
    #endregion
}
