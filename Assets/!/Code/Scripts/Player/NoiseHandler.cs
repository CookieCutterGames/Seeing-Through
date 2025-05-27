using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NoiseHandler : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float noiseLevel;
    public bool IsMoving;
    public bool IsHiding;

    public float NoiseLevel
    {
        get { return noiseLevel; }
        set { noiseLevel = value; }
    }

    void Start()
    {
        Player.Instance.playerMovement.IsMovingValueChanged += value =>
        {
            IsMoving = value;
        };
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (collision.gameObject.TryGetComponent(out Ground ground))
            {
                NoiseLevel = ground.groundData.NoiseLevel;
            }
        }
    }
}
