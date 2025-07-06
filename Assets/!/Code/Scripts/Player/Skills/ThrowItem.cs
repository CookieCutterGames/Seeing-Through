using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowItem : AbilityBase
{
    public GameObject prefab;
    public float projectileSpeed = 2f;

    protected override void Execute()
    {
        if (!Player.Instance.isHoldingMug)
            return;
        if (DialogueSystem.shown)
            return;
        if (GameplayPauseMenuSystem.isActive)
            return;
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldTargetPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, 0f)
        );
        worldTargetPos.z = 0f;

        var gm = Instantiate(prefab, transform.position, Quaternion.identity);

        Vector2 direction = ((Vector2)worldTargetPos - (Vector2)transform.position).normalized;

        MugProjectile proj = gm.AddComponent<MugProjectile>();
        proj.Init(direction, projectileSpeed, worldTargetPos);
        Player.Instance.isHoldingMug = false;
    }
}
