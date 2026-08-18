using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlturaMaxima : MonoBehaviour
{
    [SerializeField] private float alturaMaxima = 3.5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Si el balón supera la altura máxima, lo empujamos hacia abajo
        if (transform.position.y > alturaMaxima)
        {
            // Le aplicamos una fuerza fuerte hacia abajo para obligarlo a bajar
            rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        }
    }
}