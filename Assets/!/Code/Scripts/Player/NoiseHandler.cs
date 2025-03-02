using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NoiseHandler : MonoBehaviour
{
    [SerializeField]
    [Range(1, 10)]
    private int noiseLevel;

    public int NoiseLevel
    {
        get { return noiseLevel; }
        set { noiseLevel = value; }
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
