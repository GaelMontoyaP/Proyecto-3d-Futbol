using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JugadorPortero : MonoBehaviour
{
    [Header("Configuración del Salto")]
    [SerializeField] private GameObject muroAtajada; 
    [SerializeField] private float velocidadSalto = 14f;
    [SerializeField] private float alturaMinimaSuelo = 0.5f;

    private Vector3 puntoDestino;
    private bool estaSaltando = false;
    
    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private Rigidbody rb;

    void Start()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
        
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // ¡EL CANDADO DE TITANIO!
            // Le decimos a Unity desde el código: "Congela las rotaciones Y TAMBIÉN congela la posición Z"
            // El símbolo "|" sirve para sumar ambas reglas.
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }
    }

    void Update()
    {
        // 1. EL CLIC DEL MOUSE
        if (Input.GetMouseButtonDown(0) && !estaSaltando)
        {
            Ray rayoMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(rayoMouse, out RaycastHit infoGolpe, 100f))
            {
                if (infoGolpe.collider.gameObject == muroAtajada)
                {
                    SaltarHacia(infoGolpe.point);
                }
            }
        }

        // 2. MOVIMIENTO DE SALTO
        if (estaSaltando)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                puntoDestino, 
                velocidadSalto * Time.deltaTime
            );

            float direccionX = puntoDestino.x - posicionInicial.x;
            float inclinacion = 0f;

            // --- SOLUCIÓN ERROR 1: Signos invertidos ---
            // Ahora cuando vas a la derecha (> 0.3) se acuesta a la derecha
            if (direccionX > 0.3f) inclinacion = 50f;
            else if (direccionX < -0.3f) inclinacion = -50f;

            Quaternion rotacionAcostado = rotacionInicial * Quaternion.Euler(0, 0, inclinacion);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotacionAcostado, Time.deltaTime * 8f);

            // --- SOLUCIÓN ERROR 2: Caída instantánea ---
            // Calculamos qué tan lejos está de su destino. Si está a menos de 0.1 metros, ¡ya llegó!
            if (Vector3.Distance(transform.position, puntoDestino) < 0.1f)
            {
                TerminarSalto();
            }
        }
    }

    void SaltarHacia(Vector3 puntoClic)
    {
        float alturaFinal = Mathf.Max(alturaMinimaSuelo, puntoClic.y);
        puntoDestino = new Vector3(puntoClic.x, alturaFinal, posicionInicial.z);
        estaSaltando = true;
        
        if (rb != null) rb.isKinematic = true; 

        // Mantenemos esto como un "seguro de vida" por si se atora con algo (vuela máximo 1.5s)
        Invoke("TerminarSalto", 1.5f);
    }

   private void OnCollisionEnter(Collision choque)
    {
        if (estaSaltando && choque.gameObject.CompareTag("Balon"))
        {
            // 1. Matamos la velocidad del balón en seco ANTES de soltar al portero
            Rigidbody balonRb = choque.gameObject.GetComponent<Rigidbody>();
            if (balonRb != null)
            {
                balonRb.velocity = Vector3.zero; 
                balonRb.angularVelocity = Vector3.zero; 
            }

            // 2. Nos aseguramos de que el portero tampoco tenga fuerzas residuales
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
            }

            // 3. Ahora sí, que caiga al piso
            TerminarSalto();
            Cerebro.Instancia.RegistrarTiro(false, false);
        }
    }
     void RegresarAEscena1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    private void TerminarSalto()
    {
        estaSaltando = false;
        if (rb != null) rb.isKinematic = false; 
        
        // Cancelamos el "seguro de vida" para que no vuelva a intentar caer si ya se cayó
        CancelInvoke("TerminarSalto");
    }
    
}