using UnityEngine;
using System;

public class AscensorController : MonoBehaviour
{
  
    public Transform puntoInicio; 
    public Transform puntoFin;    

    public Transform puntoInterior;

   
    public float velocidadViaje = 2f;
    public bool enMovimiento = false;  
    
    public bool estaAbajo = true;


 
    public event Action OnLlegoAlInicio;
    public event Action OnLlegoAlFinal;
    public event Action OnSalioDelPunto;

    private Transform targetActual;
    private InputManager inputManager;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();

        if (puntoInicio != null && puntoFin != null)
        {
            transform.position = puntoInicio.position;
            targetActual = puntoFin;
            estaAbajo = true;
        }
        
    }

    private void Update()
    {
        if (!enMovimiento) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetActual.position,
            velocidadViaje * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetActual.position) < 0.01f)
        {
            transform.position = targetActual.position;
            enMovimiento = false;

            if (GameManager.Instance != null)
                GameManager.Instance.GameData.ascensorEnMovimiento = false;

          
            if (targetActual == puntoFin)
            {
                estaAbajo = false; 
                Debug.Log("estaAbajo = false");
                OnLlegoAlFinal?.Invoke();
            }
            else if (targetActual == puntoInicio)
            {
                estaAbajo = true; 
                Debug.Log("estaAbajo = true");
                OnLlegoAlInicio?.Invoke();
            }
        }
    }

    public void IniciarViaje()
    {
        if (enMovimiento) return;
        if (puntoInicio == null || puntoFin == null) return;

        targetActual = estaAbajo ? puntoFin : puntoInicio;

        OnSalioDelPunto?.Invoke();

        enMovimiento = true;
        if (GameManager.Instance != null)
            GameManager.Instance.GameData.ascensorEnMovimiento = true;

        Debug.Log($"Ascensor en marcha hacia {(estaAbajo ? "ARRIBA" : "ABAJO")}");
    }

    public bool EstaEnPuntoInicio() => estaAbajo && !enMovimiento;
    public bool EstaEnPuntoFin() => !estaAbajo && !enMovimiento;

    public Vector3 ObtenerPosicionInterior()
    {
        if (puntoInterior == null) return transform.position;

        Vector3 offset = new Vector3(
            UnityEngine.Random.Range(-0.3f, 0.3f),
            UnityEngine.Random.Range(-0.2f, 0.2f),
            0
        );
        return puntoInterior.position + offset;
    }

    public Transform PuntoInicio => puntoInicio;
    public Transform PuntoFin => puntoFin;
}