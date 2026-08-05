using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patearpenal : MonoBehaviour
{
    [SerializeField] private Rigidbody balonRb; 
    [SerializeField] private GameObject muroPorteria;
    [SerializeField] private Transform jugadorTransform; 
    [SerializeField] private float fuerzaHorizontalDisparo = 20f; 
    [SerializeField] private float elevacionMinima = 2f; 
    [SerializeField] private float elevacionMaxima = 9f; 
    [SerializeField] private float tiempoCargaMax = 2f; 
    [SerializeField] private float velocidadCarrera = 5f; 
    [SerializeField] private float distanciaParaPatear = 0.8f; 
    private bool estaCargandoPotencia = false;
    private float timerCargaActual = 0f;
    private Vector3 direccionBaseTiro = Vector3.zero;
    private bool yaPateo = false;
    private bool estaCorriendo = false;
    private Vector3 fuerzaGuardada = Vector3.zero;

    void Update()
    {
        if (yaPateo) return;

        if (estaCorriendo)
        {
            CorrerHaciaBalon();
            return; 
        }

        if (Input.GetMouseButtonDown(0))
        {
            ComenzarCarga();
        }

        if (estaCargandoPotencia)
        {
            CargarPotencia();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SoltarDisparo();
        }
    }

    void ComenzarCarga()
    {
        Ray rayoMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(rayoMouse, out RaycastHit infoGolpe, 100f)) 
        {
            if (infoGolpe.collider.gameObject == muroPorteria)
            {
                Vector3 puntoClic = infoGolpe.point;
                direccionBaseTiro = (puntoClic - balonRb.transform.position).normalized; 
                estaCargandoPotencia = true;
                timerCargaActual = 0f;
            }
        }
    }

    void CargarPotencia()
    {
        timerCargaActual += Time.deltaTime;
        timerCargaActual = Mathf.Clamp(timerCargaActual, 0f, tiempoCargaMax);
    }

    void SoltarDisparo()
    {
        if (estaCargandoPotencia)
        {
            estaCargandoPotencia = false;
            PrepararTiroYCorrer();
        }
    }

    void PrepararTiroYCorrer()
    {
        float ratioCarga = timerCargaActual / tiempoCargaMax;
        float elevacionFinal = Mathf.Lerp(elevacionMinima, elevacionMaxima, ratioCarga);

        fuerzaGuardada = direccionBaseTiro * fuerzaHorizontalDisparo;
        fuerzaGuardada.y = elevacionFinal; 

        estaCorriendo = true;
    }

    void CorrerHaciaBalon()
    {
        Vector3 destinoPlano = new Vector3(balonRb.transform.position.x, jugadorTransform.position.y, balonRb.transform.position.z);

        jugadorTransform.position = Vector3.MoveTowards(
            jugadorTransform.position, 
            destinoPlano, 
            velocidadCarrera * Time.deltaTime
        );

        float distancia = Vector3.Distance(jugadorTransform.position, destinoPlano);

        if (distancia <= distanciaParaPatear)
        {
            estaCorriendo = false;
            yaPateo = true; 
            
            balonRb.AddForce(fuerzaGuardada, ForceMode.Impulse);
        }
    }
}