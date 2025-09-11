using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem EngineParticle;
    [SerializeField] ParticleSystem LeftEngineParticle;
    [SerializeField] ParticleSystem RightEngineParticle;

    Rigidbody rb;
    AudioSource audioSource;
    AudioSource backgroundAudioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        backgroundAudioSource = GetComponent<AudioSource>();

        backgroundAudioSource.Play();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustSpeed * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine, 0.5f);
        }
        if (!EngineParticle.isPlaying)
        {
            EngineParticle.Play();
        }
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopAudioAndParticles();
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput < 0)
        {
            RightRotate();
        }
        else if (rotationInput > 0)
        {
            LeftRotate();
        }
        else
        {
            StopRotating();
        }
    }

    private void LeftRotate()
    {
        ApplyRotation(-rotationSpeed);
        if (!LeftEngineParticle.isPlaying)
        {
            LeftParticlesController();
        }
    }

    private void RightRotate()
    {
        ApplyRotation(rotationSpeed);
        if (!RightEngineParticle.isPlaying)
        {
            RightParticlesCotroller();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;

        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);

        rb.freezeRotation = false;
    }





    private void StopRotating()
    {
        RightEngineParticle.Stop();
        LeftEngineParticle.Stop();
    }

    private void LeftParticlesController()
    {
        RightEngineParticle.Stop();
        LeftEngineParticle.Play();
    }

    private void RightParticlesCotroller()
    {
        LeftEngineParticle.Stop();
        RightEngineParticle.Play();
    }

    private void StopAudioAndParticles()
    {
        EngineParticle.Stop();
        audioSource.Stop();
    }


}
