using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cerebro : MonoBehaviour
{
    // Esto hace que el Cerebro sea accesible desde cualquier otro script
    public static Cerebro Instancia;

    [Header("Estadísticas del Partido")]
    public int golesJugador = 0;
    public int golesRival = 0;
    public int tirosJugador = 0;
    public int tirosRival = 0;

    void Awake()
    {
        // ¡EL TRUCO DE LA INMORTALIDAD! 
        // Si no hay un Cerebro, este se convierte en el oficial y no se destruye.
        // Si ya hay uno (porque regresamos al menú), este clon se destruye.
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Esta función la llamaremos desde tus porterías cuando haya gol o fallo
    public void RegistrarTiro(bool fueGol, bool tirasteTu)
    {
        // 1. Sumar el tiro y el gol a quien corresponda
        if (tirasteTu)
        {
            tirosJugador++;
            if (fueGol) golesJugador++;
        }
        else
        {
            tirosRival++;
            if (fueGol) golesRival++;
        }

        // 2. Comprobar las reglas después de cada tiro
        RevisarSiHayGanador();
    }

    private void RevisarSiHayGanador()
    {
        int tirosRestantesJugador = 5 - tirosJugador;
        int tirosRestantesRival = 5 - tirosRival;

        // REGLA A: VICTORIA MATEMÁTICA ANTES DE LOS 5 TIROS (Ej. vas ganando 3-0 y quedan 2 tiros)
        if (tirosJugador <= 5 && tirosRival <= 5)
        {
            if (golesJugador > golesRival + tirosRestantesRival)
            {
                TerminarJuego("¡GANASTE EL PARTIDO!");
                return;
            }
            if (golesRival > golesJugador + tirosRestantesJugador)
            {
                TerminarJuego("¡PERDISTE EL PARTIDO!");
                return;
            }
        }

        // REGLA B: MUERTE SÚBITA (Si pasamos de 5 tiros y ambos han tirado la misma cantidad)
        if (tirosJugador >= 5 && tirosRival == tirosJugador)
        {
            if (golesJugador > golesRival)
            {
                TerminarJuego("¡GANASTE EN MUERTE SÚBITA!");
                return;
            }
            if (golesRival > golesJugador)
            {
                TerminarJuego("¡PERDISTE EN MUERTE SÚBITA!");
                return;
            }
        }

        // REGLA C: EL PARTIDO CONTINÚA
        SiguienteTurno();
    }

    private void SiguienteTurno()
    {
        // Cancelamos cualquier cuenta regresiva anterior por seguridad
        CancelInvoke();

        // Lógica de turnos: Si han tirado las mismas veces, te toca a ti (Escena 1).
        if (tirosJugador == tirosRival)
        {
            Invoke("CargarAtaque", 2f); // El "2f" son los 2 segundos de espera
        }
        else
        {
            Invoke("CargarDefensa", 2f);
        }
    }

    // Funciones que el Invoke mandará llamar después de los 2 segundos
    private void CargarAtaque()
    {
        // Pon el nombre EXACTO de tu Escena 1
        SceneManager.LoadScene("tirar"); 
    }

    private void CargarDefensa()
    {
        // Pon el nombre EXACTO de tu Escena 2
        SceneManager.LoadScene("parar"); 
    }
    private void TerminarJuego(string mensaje)
    {
        Debug.Log("FINAL: " + mensaje);
        // Más adelante, aquí le diremos que cargue la Escena Final (Pantalla de Victoria/Derrota)
    }
}