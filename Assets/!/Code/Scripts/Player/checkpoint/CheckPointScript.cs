using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    private RespawnScript respawn;
    private BoxCollider2D CheckPointCollider;

    void Start()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnScript>();
        CheckPointCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            respawn.RespawnPoint = this.gameObject;
            CheckPointCollider.enabled = false;
        }
    }
}
