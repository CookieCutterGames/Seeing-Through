using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
   
   private RespawnScript respawn;
    private BoxCollider2D CheckPointCollider;
    void Awake()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnScript>();
        CheckPointCollider = GetComponent<BoxCollider2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other )
    {
        if(other.gameObject.CompareTag("Player")){
            respawn.RespawnPoint = this.gameObject;
            CheckPointCollider.enabled = false;
        }
    }
}
