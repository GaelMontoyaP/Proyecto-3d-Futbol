using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    // Esta función la conectaremos al botón "JUGAR"
    public void EmpezarJuego()
    {
        // Cambia "NombreDeTuEscena1" por el nombre de tu escena donde tiras el primer penal
        SceneManager.LoadScene("tirar"); 
        PlayerPrefs.DeleteAll();
    }

    // Esta función la conectaremos al botón "SALIR"
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego..."); // Esto es para que lo veas en el editor
        Application.Quit(); // Esto cerrará el juego cuando ya esté exportado (build)
    }
}
