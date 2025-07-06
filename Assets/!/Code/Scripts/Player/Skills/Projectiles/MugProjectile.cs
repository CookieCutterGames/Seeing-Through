using UnityEngine;

public class MugProjectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private Vector2 targetPos;

    public void Init(Vector2 dir, float spd, Vector2 tp)
    {
        direction = dir;
        speed = spd;
        targetPos = tp;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        Debug.Log((new Vector2(transform.position.x, transform.position.y) - targetPos).magnitude);
        if (
            (new Vector2(transform.position.x, transform.position.y) - targetPos).magnitude <= 0.01f
        )
        {
            Destroy(gameObject);
        }
    }
}
