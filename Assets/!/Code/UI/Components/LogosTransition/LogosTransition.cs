using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LogosTransition : MonoBehaviour
{
    public string TargetScene;
    public float timeToChange;
    public Sprite[] splashes;

    private Image splashHolder;
    private Sequence sequence;

    void Start()
    {
        splashHolder = GetComponent<Image>();
        StartCoroutine(SplashChanger());
    }

    IEnumerator SplashChanger()
    {
        foreach (var splash in splashes)
        {
            splashHolder.sprite = splash;
            splashHolder.color = new Color(1, 1, 1, 0); // Reset to transparent

            // Create a new sequence for this splash
            var sequence = Sequence.Create();

            // Fade in
            sequence.Chain(
                Tween.Alpha(
                    splashHolder,
                    startValue: 0,
                    endValue: 1,
                    duration: timeToChange * 0.25f
                )
            );
            // Stay visible
            sequence.Chain(Tween.Delay(timeToChange * 0.5f));
            // Fade out
            sequence.Chain(
                Tween.Alpha(splashHolder, startValue: 1, endValue: 0, duration: timeToChange * 0.3f)
            );

            while (sequence.isAlive)
            {
                yield return null;
            }
        }

        // All splashes done, load next scene
        SceneManager.LoadScene(TargetScene);
    }
}
