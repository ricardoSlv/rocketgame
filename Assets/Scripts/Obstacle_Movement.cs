using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Movement : MonoBehaviour
{
  private Vector3 startingPos;
  public float period;
  public Vector3 movementVector;

  void Start()
  {
    startingPos = transform.position;
  }
  void Update()
  {
    if (period <= Mathf.Epsilon) { return; }

    float cycle = Time.time / period;
    float sinValue = Mathf.Sin(cycle * 2f * Mathf.PI);
    float normalizedSinValue = (sinValue + 1f) / 2f;

    transform.position = startingPos + movementVector * normalizedSinValue;
  }
}
