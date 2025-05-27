using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightMaskController : AbilityBase
{
    [SerializeField]
    private float baseRadius = 2f;

    [SerializeField]
    private float expandedRadius = 6f;

    [Header("Ring animation properties")]
    /// <summary>
    /// Dislaimer
    /// did you ever play minecraft ? there is some logic from ther, because we cannot extend cooldown time you can manipulate by
    /// thoes 3 parameters below to setup how long each section will take
    ///
    /// Example
    /// if you pick 1,2,1,1 and your cooldown time is 5s
    ///
    /// so your expandDuration will take 1s, hold duration will take 2s, shrinkDuration 1s and idleDuration 1s
    /// </summary>
    [SerializeField]
    private float expandDuration = 2f;

    [SerializeField]
    private float holdDuration = 4f;

    [SerializeField]
    private float shrinkDuration = 2f;

    [SerializeField]
    private float idleDuration = 2f;

    private float currentRadius;
    private Tween tween;

    void Start()
    {
        SetRadius(baseRadius);
    }

    protected override void Execute()
    {
        if (tween.isAlive)
        {
            tween.Stop();
        }

        tween = Tween
            .Custom(
                startValue: currentRadius,
                endValue: expandedRadius,
                duration: (
                    cooldown / (expandDuration + holdDuration + shrinkDuration + idleDuration)
                ) * expandDuration,
                onValueChange: r => SetRadius(r)
            )
            .OnComplete(() =>
            {
                Tween.Delay(
                    cooldown
                        / (expandDuration + holdDuration + shrinkDuration + idleDuration)
                        * holdDuration,
                    () =>
                    {
                        tween = Tween.Custom(
                            startValue: currentRadius,
                            endValue: baseRadius,
                            duration: cooldown
                                / (expandDuration + holdDuration + shrinkDuration + idleDuration)
                                * shrinkDuration,
                            onValueChange: r => SetRadius(r)
                        );
                    }
                );
            });
    }

    private void SetRadius(float r)
    {
        currentRadius = r;
        transform.localScale = new Vector3(r, r, 1f);
    }
}
