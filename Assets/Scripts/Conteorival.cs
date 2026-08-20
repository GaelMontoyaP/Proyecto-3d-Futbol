using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Conteorival : MonoBehaviour
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
        goles = PlayerPrefs.GetInt("RivalGuardados", 0);
        textoGoles.text = "Goles: " + goles; 
        
        // Cargar puntos del rival
        puntosRival = PlayerPrefs.GetInt("GolesGuardados", 0);
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

            PlayerPrefs.SetInt("RivalGuardados", goles);
            PlayerPrefs.Save();
            Debug.Log("¡GOOOOOL DEL RIVALLLL!");

            // ¡AQUÍ ES DONDE DEBE IR EL AVISO DE GOL! (Sí fue gol, No tiraste tú)
            Cerebro.Instancia.RegistrarTiro(true, false);
        }
        else if (otro.gameObject.CompareTag("Falla"))
        {
            yaPuntuo = true;
            Debug.Log("¡El balón salió de la cancha! No hay puntos para nadie.");

            // ¡NUEVO! Hay que avisarle al cerebro que el rival voló el balón (No fue gol, No tiraste tú)
            Cerebro.Instancia.RegistrarTiro(false, false);
        }
        
        // (Borré el bloque "Balon" de hasta abajo porque el aviso ya está donde corresponde)
    }
}