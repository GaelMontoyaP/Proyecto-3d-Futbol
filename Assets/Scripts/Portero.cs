using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portero : MonoBehaviour
{
    [SerializeField] private float velocidadSalto = 8f; 
    [SerializeField] private float margenDeError = 1.0f;
    private Vector3 puntoDestino;
    private bool debeSaltar = false;
    void Update()
    {
   
        if (debeSaltar)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                puntoDestino, 
                velocidadSalto * Time.deltaTime
            );
        }
    }

    public void IntentarAtajar(Vector3 puntoApunto)
    {
        float errorX = Random.Range(-margenDeError, margenDeError);
        float errorY = Random.Range(-margenDeError, margenDeError);
        puntoDestino = new Vector3(puntoApunto.x + errorX, puntoApunto.y + errorY, transform.position.z);   
        debeSaltar = true;
        Debug.Log("Portero intentando atajar. Destino calculado: " + puntoDestino);
    }
}
