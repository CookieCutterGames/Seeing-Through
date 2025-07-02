using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIShockWave : MonoBehaviour
{
    public Material shockWaveMaterial;
    public float waveSpeed = 2f;
    public float waveWidth = 0.05f;

    [SerializeField]
    Image img;

    void Start()
    {
        img = GetComponent<Image>();
        img.color = new Color(255, 255, 255, 0f);
    }

    public void Triggered()
    {
        TriggerWave(new Vector2(.5f, .5f));
    }

    IEnumerator WaveEffect()
    {
        float waveTime = 0f;
        yield return new WaitForSeconds(0.35f);

        while (waveTime <= 2f)
        {
            waveTime += Time.deltaTime;
            shockWaveMaterial.SetFloat("_WaveTime", waveTime);
            img.color = Color.white;
            yield return null;
        }

        img.color = new Color(255, 255, 255, 0f);
        shockWaveMaterial.SetFloat("_WaveTime", 0f);
    }

    public void TriggerWave(Vector2 originUV)
    {
        GetComponent<Image>().color = new Color(255, 255, 255, 255);
        shockWaveMaterial.SetFloat("_WaveTime", 0f);
        shockWaveMaterial.SetFloat("_WaveSpeed", waveSpeed);
        shockWaveMaterial.SetFloat("_WaveWidth", waveWidth);
        shockWaveMaterial.SetVector("_WaveOrigin", new Vector4(originUV.x, originUV.y, 0, 0));
        StartCoroutine(WaveEffect());
    }
}
