using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Conteo : MonoBehaviour
{
    [Header("Marcador UI")]
    [SerializeField] private TextMeshProUGUI textoGoles; 
    [SerializeField] private TextMeshProUGUI textoRival; 

    private int goles = 0; 
    private int puntosRival = 0; 
    
    private bool yaPuntuo = false; 

    private void Start()
    {
        // Cargar goles
        goles = PlayerPrefs.GetInt("GolesGuardados", 0);
        textoGoles.text = "Goles: " + goles; 
        
        // Cargar puntos del rival
        puntosRival = PlayerPrefs.GetInt("RivalGuardados", 0);
        if (textoRival != null)
        {
            textoRival.text = "Rival: " + puntosRival; 
        }
    }

    private void OnTriggerEnter(Collider otro)
    {
        if (yaPuntuo) return; 

        if (otro.gameObject.CompareTag("Pared"))
        {
            yaPuntuo = true;
            goles++; 
            textoGoles.text = "Goles: " + goles; 
            Cerebro.Instancia.RegistrarTiro(true, true);
            
            PlayerPrefs.SetInt("GolesGuardados", goles);
            PlayerPrefs.Save(); 
            Debug.Log("¡GOOOOOL TUYO!");
        }
        else if (otro.gameObject.CompareTag("Falla"))
        {
            yaPuntuo = true;
            // Quitamos la suma de puntos. Solo registramos que el turno acabó.
            Debug.Log("¡El balón salió de la cancha! No hay puntos para nadie.");
        }
    }

    private void OnCollisionEnter(Collision choque)
    {
        if (yaPuntuo) return; 

        if (choque.gameObject.CompareTag("Portero"))
        {
            yaPuntuo = true;
            // Quitamos la suma de puntos.
            Debug.Log("¡El portero atajó el balón! No hay puntos para nadie.");
        }
    }

    // Dejamos esta función lista para cuando el rival realmente patee y anote
    public void SumarPuntoRival()
    {
        puntosRival++;
        if (textoRival != null) 
        {
            textoRival.text = "Rival: " + puntosRival;
        }
        
        PlayerPrefs.SetInt("RivalGuardados", puntosRival);
        PlayerPrefs.Save();
    }
}