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
    private AscensorController ascensor;
    private Vector3 posicionObjetivo;


    public void Inicializar(Transform spawn, RolPasajero rolAsignado, AscensorController asc)
    {
        spawnOrigen = spawn;
        rol = rolAsignado;
        ascensor = asc;

        transform.position = spawn.position;
        estado = EstadoPasajero.EsperandoEnAnden;

        Debug.Log($" {gameObject.name} creado en {spawn.name}, rol: {rol}");
    }

    private void Update()
    {
        switch (estado)
        {
            case EstadoPasajero.EsperandoEnAnden: ProcesarEspera(); break;
            case EstadoPasajero.Subiendo: ProcesarSubida(); break;
            case EstadoPasajero.DentroDelAscensor: ProcesarViaje(); break;
            case EstadoPasajero.Bajando: ProcesarBajada(); break;
        }
    }
    public Action<Pasajero> OnPasajeroTerminado;

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
            Debug.Log($"{gameObject.name} completó su viaje");
            estado = EstadoPasajero.Terminado;

            OnPasajeroTerminado?.Invoke(this);

            Destroy(gameObject, 0.3f);
        }
    }

    private void ProcesarEspera()
    {
        
        bool ascensorEnMiLugar = (rol == RolPasajero.SubeEnInicio && ascensor.estaAbajo)
                              || (rol == RolPasajero.SubeEnFinal && !ascensor.estaAbajo);

        if (ascensorEnMiLugar && !ascensor.enMovimiento)
        {
            IniciarSubida();
        }
    }

    private void IniciarSubida()
    {
        estado = EstadoPasajero.Subiendo;
        posicionObjetivo = ascensor.ObtenerPosicionInterior();
        Debug.Log($" {gameObject.name} subiendo al ascensor");
    }

    private void ProcesarSubida()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            posicionObjetivo,
            velocidadMovimiento * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, posicionObjetivo) < 0.1f)
        {
            transform.SetParent(ascensor.transform);
            estado = EstadoPasajero.DentroDelAscensor;

            GameManager.Instance.PasajeroSubio();

            Debug.Log($" {gameObject.name} entró al ascensor");
        }
    }

    private void ProcesarViaje()
    {
        bool ascensorEnMiDestino = (rol == RolPasajero.SubeEnInicio && !ascensor.estaAbajo)
                                || (rol == RolPasajero.SubeEnFinal && ascensor.estaAbajo);

        if (ascensorEnMiDestino && !ascensor.enMovimiento)
        {
            IniciarBajada();
        }
    }

    private void IniciarBajada()
    {
        estado = EstadoPasajero.Bajando;
        transform.SetParent(null);
        posicionObjetivo = transform.position + new Vector3(
             1f,
           0.5f
        );
        Debug.Log($" {gameObject.name} bajando del ascensor");
    }

    

}