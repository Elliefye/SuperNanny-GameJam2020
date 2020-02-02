using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dontDestroy : MonoBehaviour
{
    [SerializeField] AudioClip[] music;
    AudioSource audioSource;
    int currentScene=0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = music[0];
        audioSource.Play();
        DontDestroyOnLoad(this.gameObject);
    }
    void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().buildIndex != currentScene && SceneManager.GetActiveScene().buildIndex != 1)
        {
            if(currentScene == 0)
            {
                currentScene = 2;
                audioSource.clip = music[1];
                audioSource.Play();
            }
            else if(currentScene == 2)
            {
                currentScene = 0;
                audioSource.clip = music[0];
                audioSource.Play();
            }
        }
    }
    // Update is called once per frame
}
