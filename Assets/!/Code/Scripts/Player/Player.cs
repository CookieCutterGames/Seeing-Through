using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public NoiseHandler noiseHandler;

    public PlayerMovement playerMovement;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (noiseHandler == null)
        {
            if (TryGetComponent(out NoiseHandler noiseHandler))
            {
                this.noiseHandler = noiseHandler;
            }
            else
            {
                this.noiseHandler = gameObject.AddComponent<NoiseHandler>();
            }
        }
        if (playerMovement == null)
        {
            if (TryGetComponent(out PlayerMovement playerMovement))
            {
                this.playerMovement = playerMovement;
            }
            else
            {
                this.playerMovement = gameObject.AddComponent<PlayerMovement>();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        noiseHandler.OnTriggerEnter2D(collision);
    }
}
