using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public NoiseHandler noiseHandler;

    void Awake()
    {
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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        noiseHandler.OnTriggerEnter2D(collision);
    }
}
