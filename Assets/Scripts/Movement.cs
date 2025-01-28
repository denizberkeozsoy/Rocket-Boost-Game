using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustStrength = 100f;
    [SerializeField] float rotationStrength = 100f;
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainBoosterSFX;
    [SerializeField] ParticleSystem leftBoosterSFX;
    [SerializeField] ParticleSystem rightBoosterSFX;

    Rigidbody rb;

    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {

        thrust.Enable();
        rotation.Enable();
        
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
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

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainBoosterSFX.isPlaying)
        {
            mainBoosterSFX.Play();
        }
    }
    private void StopThrusting()
    {
        audioSource.Stop();
        mainBoosterSFX.Stop();
    }


    private void ProcessRotation(){
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            RotateRight();
        }
        else if (rotationInput > 0)
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }


    private void RotateRight()
    {
        ApplyRotation(rotationStrength);
        if (!leftBoosterSFX.isPlaying)
        {
            leftBoosterSFX.Play();
            rightBoosterSFX.Stop();
        }
    }
    private void RotateLeft()
    {
        ApplyRotation(-rotationStrength);
        if (!rightBoosterSFX.isPlaying)
        {
            rightBoosterSFX.Play();
            leftBoosterSFX.Stop();
        }
    }
    private void StopRotating()
    {
        rightBoosterSFX.Stop();
        leftBoosterSFX.Stop();
    }


    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.left * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}
