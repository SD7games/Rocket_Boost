using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadSceneDilay = 1.5f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip successSFX;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isControlable = true;
    bool isCollidable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKey();
    }

    private void RespondToDebugKey()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame) // считывание 'С' wasPressedThisFrame один раз. 
        {
            isCollidable = !isCollidable;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isControlable || !isCollidable) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("You are Starting Platform.");
                break;
            case "Finish":
                StartSuccessSequense();
                break;
            case "Trash":
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequense()
    {
        isControlable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX, 0.6f);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadSceneDilay);
        isControlable = true;
    }

    void StartCrashSequence()
    {
        isControlable = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX, 0.1f);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadSceneDilay);
        isControlable = true;
    }

    void ReloadLevel()
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
