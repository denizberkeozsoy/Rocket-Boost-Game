using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioSource audioSource;    
    [SerializeField] AudioClip ObstacleSoundSFX;    
    [SerializeField] AudioClip FinishSoundSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] GameObject gm;
    bool isControllAble = true;
    bool isCollidable = true;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }    

    private void Update() {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys(){
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            loadNextLevel();
        }
        else if(Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
    }


    private void OnCollisionEnter(Collision other) 
    {

        if(!isControllAble || !isCollidable) { return; }

        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Fuel":
                Debug.Log("Fuel");
                break;
            case "Finish":
                Debug.Log("Finish");
                StartSuccessSequance();
                break; 
            default:   
                StartCrashSequance();
                break;
        }

    }

    void StartSuccessSequance()
    {
        successParticles.Play();
        isControllAble = false;        
        audioSource.Stop();
        audioSource.PlayOneShot(FinishSoundSFX);
        GetComponent<Movement>().enabled = false;        
        Invoke("loadNextLevel", levelLoadDelay);
    }

    void StartCrashSequance()
    {   
        crashParticles.Play();
        isControllAble = false;
        audioSource.Stop();
        audioSource.PlayOneShot(ObstacleSoundSFX);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void ReloadLevel()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);      
        }
        void loadNextLevel()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            int nextScene = currentScene + 1;
            
            if(nextScene == SceneManager.sceneCountInBuildSettings)
            {
                nextScene = 0;
            }
            
            SceneManager.LoadScene(nextScene);      
        }        
}
