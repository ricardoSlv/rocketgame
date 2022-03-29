using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
  public float mainThrustStrength;
  public float sideThrustStrength;
  Rigidbody rb;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  void Update()
  {

    transform.Rotate(Vector3.back * sideThrustStrength * Time.deltaTime * Input.GetAxis("Horizontal"));

    if (Input.GetAxis("Vertical") > 0)
      rb.AddRelativeForce(Vector3.up * mainThrustStrength * Time.deltaTime);
  }
}
