using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroRival : MonoBehaviour
{
    [Header("Objetos")]
    [SerializeField] private Rigidbody balonRb; 
    [SerializeField] private Transform centroPorteria; 

    [Header("Configuración del Tiro")]
    [Tooltip("Aumentamos la fuerza para que sea un misil rápido")]
    [SerializeField] private float fuerzaTiro = 45f; 
    [Tooltip("Velocidad a la que el rival corre hacia el balón")]
    [SerializeField] private float velocidadCorrer = 7f;
    [Tooltip("Segundos antes de que empiece a correr")]
    [SerializeField] private float tiempoDeEspera = 1f; 

    private bool corriendoHaciaBalon = false;
    private Vector3 posicionInicialBalon;

    void Start()
    {
        if (balonRb != null)
        {
            // Guardamos dónde está el balón para saber a dónde correr
            posicionInicialBalon = balonRb.transform.position;
        }
        
        // En lugar de patear de inmediato, inicia la carrera después de la espera
        Invoke("EmpezarACorrer", tiempoDeEspera);
    }

    void EmpezarACorrer()
    {
        corriendoHaciaBalon = true;
    }

    void Update()
    {
        // 1. EL JUGADOR CORRE HACIA EL BALÓN
        if (corriendoHaciaBalon && balonRb != null)
        {
            // Creamos un punto de destino usando la X y Z del balón, pero manteniendo la altura Y del jugador
            // para evitar que el jugador empiece a flotar o enterrarse
            Vector3 destinoJugador = new Vector3(posicionInicialBalon.x, transform.position.y, posicionInicialBalon.z);
            
            // Movemos al jugador hacia el balón
            transform.position = Vector3.MoveTowards(transform.position, destinoJugador, velocidadCorrer * Time.deltaTime);

            // 2. ¿YA LLEGÓ AL BALÓN? (Si está a menos de 1 metro, patea)
            if (Vector3.Distance(transform.position, destinoJugador) < 1f)
            {
                corriendoHaciaBalon = false;
                PatearPenal();
            }
        }
    }

    void PatearPenal()
    {
        if (balonRb == null || centroPorteria == null) return;

        // 3. ELEGIR BLANCO Y ELEVAR EL BALÓN
        float desvioX = Random.Range(-7f, 7f); 
        // Subimos el rango Y. Ahora lo mínimo es 1.5 metros de alto para que siempre vaya elevado.
        float alturaY = Random.Range(1.5f, 3.5f);  

        Vector3 destinoDelTiro = new Vector3(
            centroPorteria.position.x + desvioX, 
            alturaY, 
            centroPorteria.position.z
        );

        // Calculamos la dirección del tiro
        // Calculamos la dirección del tiro
        Vector3 direccion = (destinoDelTiro - balonRb.transform.position).normalized;
        
        // FORZAMOS LA ALTURA: Ignoramos el cálculo original en Y y le imponemos un número alto
        direccion.y = Random.Range(0.30f, 0.40f);
        direccion = direccion.normalized;

        // ¡PUM! Disparo potente usando ForceMode.Impulse
        balonRb.AddForce(direccion * fuerzaTiro, ForceMode.Impulse);
        
        Debug.Log("¡El rival corrió y disparó un misil elevado!");
    }
}