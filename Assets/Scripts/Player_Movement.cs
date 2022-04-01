using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{

  public bool ultraRealistic = false;
  private bool gameOver = false;
  public float mainThrustStrength;
  public float sideThrustStrength;
  public float deathFlipStrength;
  [SerializeField] AudioClip thrustAudio;
  [SerializeField] AudioClip sideAudio;
  [SerializeField] AudioClip bumpAudio;
  [SerializeField] AudioClip explosionAudio;
  [SerializeField] AudioClip successAudio;

  [SerializeField] AudioSource thrustAudioSource;
  [SerializeField] AudioSource sideAudioSource;
  [SerializeField] AudioSource generalAudioSource;

  [SerializeField] ParticleSystem explosionParticles;
  [SerializeField] ParticleSystem successParticles;
  [SerializeField] ParticleSystem thrustParticles;
  [SerializeField] ParticleSystem side1Particles;
  [SerializeField] ParticleSystem side2Particles;

  Rigidbody rb;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  void OnCollisionEnter(Collision other)
  {
    switch (other.gameObject.tag)
    {
      case "Friendly":
        generalAudioSource.PlayOneShot(bumpAudio);
        break;
      case "Finish":
        if (!gameOver)
          ProcessSuccess();
        break;
      default:
        if (gameOver)
        {
          generalAudioSource.PlayOneShot(bumpAudio);
          Invoke("ReloadLevel", 1f);
        }
        else
          ProcessDeath(other);
        break;
    }
  }

  private void ProcessSuccess()
  {
    successParticles.Play();
    generalAudioSource.PlayOneShot(successAudio);
    Invoke("LoadNextLevel", 1f);
    gameOver = true;
    StopAudioAndParticles();
  }

  private void ProcessDeath(Collision other)
  {
    explosionParticles.Play();
    gameOver = true;
    rb.constraints = 0;
    generalAudioSource.PlayOneShot(explosionAudio);
    rb.AddForce(other.relativeVelocity * 20);
    rb.AddRelativeTorque(new Vector3(1, 1, 1) * deathFlipStrength * 1000);
    StopAudioAndParticles();
  }

  void Update()
  {
    if (gameOver == false)
    {
      ProcessRotation();
      ProcessThrust();
    }
  }

  private void StopAudioAndParticles()
  {
    thrustAudioSource.Stop();
    sideAudioSource.Stop();
    thrustParticles.Stop();
    side1Particles.Stop();
    side2Particles.Stop();
  }

  private void ProcessThrust()
  {
    if (Input.GetAxis("Vertical") > 0)
    {
      if (!thrustAudioSource.isPlaying)
      {
        thrustAudioSource.PlayOneShot(thrustAudio);
        thrustParticles.Play();
      }
      rb.AddRelativeForce(Vector3.up * mainThrustStrength * Time.deltaTime);
    }
    else
    {
      thrustAudioSource.Stop();
      thrustParticles.Stop();
    }
  }

  private void ProcessRotation()
  {
    if (Input.GetAxis("Horizontal") != 0)
    {
      if (ultraRealistic)
        rb.AddRelativeTorque(Vector3.back * 5 * sideThrustStrength * Time.deltaTime * Input.GetAxis("Horizontal"));
      else
      {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.back * sideThrustStrength * Time.deltaTime * Input.GetAxis("Horizontal"));
        rb.freezeRotation = false;
      }

      if (!sideAudioSource.isPlaying)
        sideAudioSource.PlayOneShot(sideAudio);

      if (Input.GetAxis("Horizontal") < 0 && !side1Particles.isPlaying)
        side1Particles.Play();
      else if (Input.GetAxis("Horizontal") > 0 && !side2Particles.isPlaying)
        side2Particles.Play();
    }
    else
    {
      sideAudioSource.Stop();
      side1Particles.Stop();
      side2Particles.Stop();
    }
  }

  void ReloadLevel()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }

  void LoadNextLevel()
  {
    SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
  }
}
