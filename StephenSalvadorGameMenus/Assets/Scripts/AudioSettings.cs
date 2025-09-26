using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    //[SerializeField] AudioSource SFXSource;

    public AudioClip background;
    //public AudioClip gtr;
    //public AudioClip nsx;
    //public AudioClip rx7;
    //public AudioClip wrx;
    //public AudioClip supra;

    public static AudioSettings instance;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    //public void PlaySFX(AudioClip clip)
    //{
    //    SFXSource.PlayOneShot(clip);
    //}

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

        }
        else
        {
            Destroy(gameObject);
        }

    }
}
