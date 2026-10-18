using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    [Header ("Fuentes de Audio")]
    public AudioSource fuenteMusica;
    public AudioSource fuentesFSX;

    [Header ("Audio Clips")]
    public AudioClip musicaFondo;
    public AudioClip sonidoSalto;
    public AudioClip sonidoGolpe;
    public AudioClip sonidoMoneda;
    public AudioClip sonidoDanio;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(musicaFondo != null && fuenteMusica != null)
        {
            fuenteMusica.clip = musicaFondo;
            fuenteMusica.loop = true;
            fuenteMusica.Play();
        }
    }

    public void ReproducirSfx(AudioClip clip)
    {
        if(clip != null && fuentesFSX != null)
        {
            fuentesFSX.PlayOneShot(clip);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
