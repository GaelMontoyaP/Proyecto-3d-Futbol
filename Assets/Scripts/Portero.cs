using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portero : MonoBehaviour
{
    [Header("Configuración del Portero")]
    [SerializeField] private float velocidadSalto = 12f; 
    [SerializeField] private float probabilidadDeError = 30f; 
    
    [Header("Límites del Escenario")]
    [Tooltip("El límite más bajo al que puede llegar el centro del portero para no enterrarse")]
    [SerializeField] private float alturaMinimaSuelo = 0.5f; // <-- NUEVO

    private Transform balonObjetivo;
    private bool estaAtajando = false;
    private Vector3 errorDeCalculo = Vector3.zero;
    
    private Vector3 posicionInicial;
    private Quaternion rotacionInicial; 

    private Rigidbody rb;

    void Start()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation; 
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (estaAtajando && balonObjetivo != null)
        {
            // Calculamos la altura a la que quiere ir
            float alturaDeseada = balonObjetivo.position.y + errorDeCalculo.y;
            
            // --- LA PROTECCIÓN ESTÁ AQUÍ ---
            // Mathf.Max elige el número más grande. Si la altura deseada es 0.1 pero el límite es 0.5, elegirá 0.5.
            float alturaFinal = Mathf.Max(alturaMinimaSuelo, alturaDeseada);

            Vector3 destino = new Vector3(
                balonObjetivo.position.x + errorDeCalculo.x, 
                alturaFinal, // Usamos la altura protegida
                posicionInicial.z 
            );

            transform.position = Vector3.MoveTowards(
                transform.position, 
                destino, 
                velocidadSalto * Time.deltaTime
            );

            float direccionX = destino.x - posicionInicial.x;
            float inclinacion = 0f;

            if (direccionX > 0.5f) inclinacion = 50f;
            else if (direccionX < -0.5f) inclinacion = -50f;

            Quaternion rotacionAcostado = rotacionInicial * Quaternion.Euler(0, 0, inclinacion);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotacionAcostado, Time.deltaTime * 8f);
        }
    }

    public void IniciarAtajada(Transform elBalon)
    {
        balonObjetivo = elBalon;
        estaAtajando = true;

        if (rb != null)
        {
            rb.isKinematic = true; 
        }

        float suerte = Random.Range(0f, 100f);
        
        if (suerte <= probabilidadDeError)
        {
            errorDeCalculo = new Vector3(Random.Range(-3f, 3f), Random.Range(-2f, 2f), 0f);
        }
        else
        {
            errorDeCalculo = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
        }

        Invoke("TerminarAtajada", 1.2f);
    }

    private void OnCollisionEnter(Collision choque)
    {
        if (estaAtajando)
        {
            TerminarAtajada();
        }
    }

    private void TerminarAtajada()
    {
        estaAtajando = false;
        
        if (rb != null)
        {
            rb.isKinematic = false; 
        }
    }
}