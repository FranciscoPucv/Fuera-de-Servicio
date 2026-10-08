using System;
using UnityEngine;

public enum EstadoPasajero
{
    EsperandoEnAnden,
    Subiendo,
    DentroDelAscensor,
    Bajando,
    Terminado
}

public enum RolPasajero
{
    SubeEnInicio,
    SubeEnFinal
}

public class Pasajero : MonoBehaviour
{
    public float velocidadMovimiento = 3f;
    public int puntosQueOtorga = 10;

    public EstadoPasajero estado = EstadoPasajero.EsperandoEnAnden;
    public RolPasajero rol = RolPasajero.SubeEnInicio;

    private Transform spawnOrigen;
    private AscensorManager ascensorManager;
    private Vector3 posicionObjetivo;
    private Vector3 offsetInterior; 
    private bool inicializado = false;
    private int ascensorIndex = 0;
    private bool yaRegistrado = false;

    public Action<Pasajero> OnPasajeroTerminado;

   
    public void Inicializar(Transform spawn, RolPasajero rolAsignado, int index, AscensorManager manager)
    {
        spawnOrigen = spawn;
        rol = rolAsignado;
        ascensorIndex = index;
        ascensorManager = manager;

        transform.position = spawn.position;
        estado = EstadoPasajero.EsperandoEnAnden;
        inicializado = true;
    }

    private void Update()
    {
        if (!inicializado) return;
        if (ascensorManager == null) return;

        switch (estado)
        {
            case EstadoPasajero.EsperandoEnAnden: ProcesarEspera(); break;
            case EstadoPasajero.Subiendo: ProcesarSubida(); break;
            case EstadoPasajero.DentroDelAscensor: ProcesarViaje(); break;
            case EstadoPasajero.Bajando: ProcesarBajada(); break;
        }
    }

    
    private void ProcesarEspera()
    {
        bool ascensorEstaAbajo = ascensorManager.EstaAbajo(ascensorIndex);
        bool ascensorEnMovimiento = ascensorManager.ascensores[ascensorIndex].enMovimiento;

        bool ascensorEnMiLugar = (rol == RolPasajero.SubeEnInicio && ascensorEstaAbajo)
                              || (rol == RolPasajero.SubeEnFinal && !ascensorEstaAbajo);

        if (ascensorEnMiLugar && !ascensorEnMovimiento)
        {
            IniciarSubida();
        }
    }

   
    private void IniciarSubida()
    {
        estado = EstadoPasajero.Subiendo;

      
        offsetInterior = new Vector3(
            UnityEngine.Random.Range(-0.15f, 0.15f),
            UnityEngine.Random.Range(-0.1f, 0.1f),
            0
        );
    }

    private void ProcesarSubida()
    {
      
        Transform puntoInterior = ascensorManager.ascensores[ascensorIndex].puntoInterior;
        if (puntoInterior != null)
        {
            posicionObjetivo = puntoInterior.TransformPoint(offsetInterior);
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            posicionObjetivo,
            velocidadMovimiento * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, posicionObjetivo) < 0.1f)
        {
            transform.SetParent(ascensorManager.ascensores[ascensorIndex].ascensorTransform);

            transform.localPosition = new Vector3(offsetInterior.x, offsetInterior.y, -0.1f);

            estado = EstadoPasajero.DentroDelAscensor;
            GameManager.Instance.PasajeroSubio();

            if (!yaRegistrado)
            {
                yaRegistrado = true;
                GameManager.Instance.gameData.resumenDia.RegistrarPasajeroTransportado();
            }
        }
    }

    private void ProcesarViaje()
    {
        bool ascensorEstaAbajo = ascensorManager.EstaAbajo(ascensorIndex);
        bool ascensorEnMovimiento = ascensorManager.ascensores[ascensorIndex].enMovimiento;

        bool ascensorEnMiDestino = (rol == RolPasajero.SubeEnInicio && !ascensorEstaAbajo)
                                || (rol == RolPasajero.SubeEnFinal && ascensorEstaAbajo);

        if (ascensorEnMiDestino && !ascensorEnMovimiento)
        {
            IniciarBajada();
        }
    }

    private void IniciarBajada()
    {
        estado = EstadoPasajero.Bajando;
        transform.SetParent(null);

        Transform puntoSalida = ascensorManager.ObtenerPuntoSalida(ascensorIndex);

        if (puntoSalida != null)
        {
           
            float offsetY = UnityEngine.Random.Range(-0.3f, 0.3f);
            posicionObjetivo = puntoSalida.position + new Vector3(0, offsetY, 0);
        }
        else
        {
            
            Vector2 direccionAleatoria = UnityEngine.Random.insideUnitCircle.normalized;
            posicionObjetivo = transform.position + (Vector3)(direccionAleatoria * 1.5f);
           
        }

    }

    private void ProcesarBajada()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            posicionObjetivo,
            velocidadMovimiento * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, posicionObjetivo) < 0.1f)
        {
            GameManager.Instance.PasajeroCompleto();          
            estado = EstadoPasajero.Terminado;

            OnPasajeroTerminado?.Invoke(this);
            Destroy(gameObject, 0.3f);
        }
    }
}