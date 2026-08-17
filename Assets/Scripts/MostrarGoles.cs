using UnityEngine;
using TMPro; 

public class MostrarGoles : MonoBehaviour
{
    [Header("Interfaz de Resultados")]
    [SerializeField] private TextMeshProUGUI textoGolesFinales;
    
    [Tooltip("Arrastra aquí tu nuevo texto para los puntos del rival en esta escena")]
    [SerializeField] private TextMeshProUGUI textoRivalFinales; // <-- NUEVO

    void Start()
    {
        // 1. Cargar y mostrar los goles del jugador
        int golesTotales = PlayerPrefs.GetInt("GolesGuardados", 0);
        if (textoGolesFinales != null)
        {
            textoGolesFinales.text = "Goles: " + golesTotales;
        }

        // 2. Cargar y mostrar los puntos del rival (ATAJADAS O FALLAS)
        int puntosRival = PlayerPrefs.GetInt("RivalGuardados", 0); // <-- NUEVO
        if (textoRivalFinales != null)
        {
            textoRivalFinales.text = "Goles:" + puntosRival;
        }

        Debug.Log("Resultados cargados - Jugador: " + golesTotales + " | Rival: " + puntosRival);
    }
}
