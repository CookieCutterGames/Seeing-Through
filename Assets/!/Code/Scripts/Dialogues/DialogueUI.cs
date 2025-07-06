using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;
    public Image imageDisplay;
    public Button continueButton;

    private DialogueData currentDialogue;
    private int currentIndex = 0;

    Action onComplete;
    Coroutine typingCoroutine;
    bool isTyping = false;
    string fullContent = "";

    [Header("Audio")]
    public AudioClip typingSound;
    private AudioSource typingAudioSource;

    void Awake()
    {
        typingAudioSource = gameObject.AddComponent<AudioSource>();
        typingAudioSource.loop = true;
        typingAudioSource.playOnAwake = false;
        typingAudioSource.spatialBlend = 0f;
    }

    public void Show(DialogueData dialogue, Action onComplete = null)
    {
        currentDialogue = dialogue;
        this.onComplete = onComplete;
        currentIndex = 0;
        DisplaySlide();
        transform.GetChild(0).gameObject.SetActive(true);
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(OnContinueClicked);
    }

    public void Hide()
    {
        onComplete?.Invoke();
        onComplete = null;
        transform.GetChild(0).gameObject.SetActive(false);
        continueButton.onClick.RemoveAllListeners();
    }

    void DisplaySlide()
    {
        if (currentDialogue == null || currentIndex >= currentDialogue.slides.Length)
        {
            DialogueSystem.Hide();
            return;
        }

        var slide = currentDialogue.slides[currentIndex];

        titleText.text = slide.title;
        titleText.gameObject.SetActive(!string.IsNullOrEmpty(slide.title));

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(slide.content));

        if (slide.image != null)
        {
            imageDisplay.sprite = slide.image;
            imageDisplay.gameObject.SetActive(true);
        }
        else
        {
            imageDisplay?.gameObject.SetActive(false);
        }
    }

    IEnumerator TypeText(string content)
    {
        isTyping = true;
        fullContent = content;
        contentText.text = "";

        if (typingSound != null && typingAudioSource != null)
        {
            typingAudioSource.clip = typingSound;
            typingAudioSource.volume = AudioManager.Instance.MainVolume;
            typingAudioSource.Play();
        }

        foreach (char c in content)
        {
            contentText.text += c;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.1f));
        }

        typingAudioSource.Stop();
        isTyping = false;
    }

    void OnContinueClicked()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            contentText.text = fullContent;
            typingAudioSource.Stop();
            isTyping = false;
        }
        else
        {
            NextSlide();
        }
    }

    void NextSlide()
    {
        currentIndex++;
        if (currentIndex >= currentDialogue.slides.Length)
        {
            DialogueSystem.Hide();
        }
        else
        {
            DisplaySlide();
        }
    }
}
