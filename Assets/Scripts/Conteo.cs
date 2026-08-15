using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Conteo : MonoBehaviour
{
    [Header("Marcador UI")]
    [SerializeField] private TextMeshProUGUI textoGoles; 

    private int goles = 0; 

    // Start se ejecuta automáticamente una vez al arrancar el juego
    private void Start()
    {
        // 1. CARGAR: Leemos la memoria. Si no hay datos, empieza en 0.
        goles = PlayerPrefs.GetInt("GolesGuardados", 0);
        
        // Actualizamos el texto en la pantalla para que muestre lo que cargamos
        textoGoles.text = "Goles: " + goles; 
    }

    private void OnTriggerEnter(Collider otro)
    {
        if (otro.gameObject.CompareTag("Pared"))
        {
            goles++; 
            textoGoles.text = "Goles: " + goles; 

            // 2. GUARDAR: Metemos el nuevo número a la memoria
            PlayerPrefs.SetInt("GolesGuardados", goles);
            
            // Forzamos a Unity a guardar el archivo físicamente en este instante
            PlayerPrefs.Save(); 

            Debug.Log("¡GOOOOOL! Total guardado en memoria: " + goles);
        }
    }
}