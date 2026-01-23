using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip background;
    //public AudioClip gtr;
    //public AudioClip nsx;
    //public AudioClip rx7;
    //public AudioClip wrx;
    //public AudioClip supra;

    public static AudioSettings instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // keep this object alive
        }
        else
        {
            Destroy(gameObject); // destroy duplicate instances
        }
    }

    private void Start()
    {
        if (!musicSource.isPlaying) // optional safety check
        {
            musicSource.clip = background;
            musicSource.Play();
        }
    }
}
