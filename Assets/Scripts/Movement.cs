using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;
    [SerializeField] private float thrustStrength = 1000f;
    [SerializeField] private float rotationStrength = 100f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip mainEngineSFX;
    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem rightThrustParticles;
    [SerializeField] private ParticleSystem leftThrustParticles;


    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    void OnDisable()
    {
        thrust.Disable();
        rotation.Disable();
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngineSFX);
            }

            if (!mainEngineParticles.isPlaying)
            {
                mainEngineParticles.Play();
            }
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput < 0f)
        {
            ApplyRotation(-rotationInput);
            if (!rightThrustParticles.isPlaying)
            {
                leftThrustParticles.Stop();
                rightThrustParticles.Play();
            }
        }
        else if (rotationInput > 0f)
        {
            ApplyRotation(-rotationInput);
            if (!leftThrustParticles.isPlaying)
            {
                rightThrustParticles.Stop();
                leftThrustParticles.Play();
            }
        }else
        {
            rightThrustParticles.Stop();
            leftThrustParticles.Stop();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * rotationStrength * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}
