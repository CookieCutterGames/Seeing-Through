using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject RespawnPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.transform.position = RespawnPoint.transform.position;
        }
    }
}
