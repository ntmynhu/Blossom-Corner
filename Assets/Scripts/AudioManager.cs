using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-------------- Audio Source --------------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;


    [Header("--------------- Audio Clip --------------")]
    public AudioClip background;
    public AudioClip doorBell;
    public AudioClip shovel;
    public AudioClip rustling;
    public AudioClip watering;
    public AudioClip cutting;
    public AudioClip moneyPickup;
    public AudioClip moneyPurchase;

    #region Singleton

    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    private void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySPF(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopSFX()
    {
        SFXSource.Stop();
    }

    public AudioSource GetSPXSource()
    {
        return SFXSource;
    }
}
