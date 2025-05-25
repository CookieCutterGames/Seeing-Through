using UnityEngine; 
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
public AudioMixer audioMixer;
public void UpdateMusicVolume(float volume)
{
audioMixer. SetFloat("MusicVolume", volume);
}
public void UpdateSoundVolume (float volume)
{
audioMixer.SetFloat("SFXVolume", volume);
}
}