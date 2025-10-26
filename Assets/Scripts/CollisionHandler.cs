using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Header("Durations")]
    [SerializeField] private float levelLoadDelay = 2f;
    [Header("Sounds")]
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip crashesSound;
    [Header("Particles")]
    [SerializeField] private ParticleSystem successParticle;
    [SerializeField] private ParticleSystem crasheParticle;

    AudioSource audioSource;
    bool isControllable = true;
    bool isCollidable = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("lye basarsan sonraki seviye geçer c'ye basarsan bütün işlevsel olan herşey önemsi olur");
    }
    void Update()
    {
        RespondToDebugKeys();
    }
    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Debug.Log("l'ye bastın yarram");
            LoadNextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            Debug.Log("c'ye bastın yarram");
            isCollidable = !isCollidable;
        }

    }
    void OnCollisionEnter(Collision other)
    {
        if (!isControllable || !isCollidable) return;
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("arkadaş cvansılı");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                Debug.Log("yaktı");
                break;
            default:
                StartCrashSequence();
                break;
        }

    }

    void StartSuccessSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashesSound);
        crasheParticle.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloaadLevel", 2f);
    }

    void ReloaadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        SceneManager.LoadScene(nextScene);
    }
}