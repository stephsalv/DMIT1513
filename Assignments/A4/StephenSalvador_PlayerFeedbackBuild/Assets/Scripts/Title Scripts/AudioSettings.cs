using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip background;


    public static AudioSettings instance;
    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this);

        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

}
