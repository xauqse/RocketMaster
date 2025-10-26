using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;

    [Header("Value")]
    [SerializeField] private float thrustStrength = 100f;
    [SerializeField] private float rotateStrength = 100f;

    [Header("Sounds")]
    [SerializeField] private AudioClip mainEngine;

    [Header("Particle")]
    [SerializeField] private ParticleSystem middleParticle;
    [SerializeField] private ParticleSystem leftParticle;
    [SerializeField] private ParticleSystem rightParticle;
    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }
    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotate();
    }
    void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }
    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * Time.fixedDeltaTime * thrustStrength);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!middleParticle.isPlaying)
        {
            middleParticle.Play();
        }
    }
    void StopThrusting()
    {
        audioSource.Pause();
        middleParticle.Stop();
    }
    void ProcessRotate()
    {
        float rotationValue = rotation.ReadValue<float>();
        if (rotationValue < 0)
        {
            RotateRight();
        }
        else if (rotationValue > 0)
        {
            RotateLeft();
        }
        else
        {
            StopRotate();
        }
    }
    void RotateLeft()
    {
        ApplyRotation(-rotateStrength);
        if (!rightParticle.isPlaying)
        {
            leftParticle.Stop();
            rightParticle.Play();
        }
    }
    void RotateRight()
    {
        ApplyRotation(rotateStrength);
        if (!leftParticle.isPlaying)
        {
            rightParticle.Stop();
            leftParticle.Play();
        }
    }
    void StopRotate()
    {
        rightParticle.Stop();
        leftParticle.Stop();
    }
    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * Time.fixedDeltaTime * rotationThisFrame);
        rb.freezeRotation = false;
    }
}
