using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Conteo : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textoGoles; 

    // El contador numérico secreto
    private int goles = 0; 

    // Esta función se activa SOLA cuando el balón atraviesa un "Trigger"
    private void OnTriggerEnter(Collider otro)
    {
        // Preguntamos si lo que tocamos tiene el Tag "Gol"
        if (otro.gameObject.CompareTag("Pared"))
        {
            // Sumamos 1 a nuestro contador
            goles++; 

            // Actualizamos el texto en la pantalla
            textoGoles.text = "Goles: " + goles; 

            Debug.Log("¡GOOOOOL!");
        }
    }
}