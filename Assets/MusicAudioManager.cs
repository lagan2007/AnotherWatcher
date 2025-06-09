using UnityEngine;

public class MusicAudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource musicSource;

    public AudioClip musicClip;
    
    
    void Start()
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

}
