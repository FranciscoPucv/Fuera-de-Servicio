using UnityEngine;
using System.Collections.Generic;
using System;

public class EventoEstadoManager : MonoBehaviour
{
    [System.Serializable]
    public class EventoEstadoConfig : EventoConfig
    {
       
        public TipoSistema sistema;

        [Range(0f, 100f)]
        public float umbral = 50f;

        [Range(0f, 1f)]
        public float probabilidadBase = 0.25f;

        [Range(0f, 1f)]
        public float probabilidadMaxima = 1f;
    }

    public List<EventoEstadoConfig> eventos = new List<EventoEstadoConfig>();

    public float intervaloChequeo = 3f;
    public bool debugLogs = true;

    public Action<GameObject> OnEventoTerminado;

    private float timer = 0f;
    private bool eventoEnCurso = false;
    private GameObject eventoInstanciado;

    private void Update()
    {
        if (eventoEnCurso) return;

        timer += Time.deltaTime;
        if (timer < intervaloChequeo) return;

        timer = 0f;
        EvaluarEventos();
    }

    private void EvaluarEventos()
    {
        if (GameManager.Instance == null) return;

        List<EventoEstadoConfig> validos = new List<EventoEstadoConfig>();

        foreach (var e in eventos)
        {
            if (PuedeOcurrir(e))
                validos.Add(e);
        }

        if (validos.Count == 0)
        {
            if (debugLogs)
                Debug.Log("No hay eventos de estado válidos en este momento");
            return;
        }

        EventoEstadoConfig elegido = ElegirEvento(validos);
        float probabilidad = CalcularProbabilidad(elegido);

        if (UnityEngine.Random.value > probabilidad)
        {
            if (debugLogs)
                Debug.Log($" {elegido.nombre}: no ocurrió (prob {probabilidad:P0})");
            return;
        }

        LanzarEvento(elegido);
    }

    public bool IntentarLanzarEvento()
    {
        if (eventoEnCurso) return false;
        if (GameManager.Instance == null) return false;

        List<EventoEstadoConfig> validos = new List<EventoEstadoConfig>();
        foreach (var e in eventos)
        {
            if (PuedeOcurrir(e))
                validos.Add(e);
        }

        if (validos.Count == 0) return false;

        EventoEstadoConfig elegido = ElegirEvento(validos);
        float probabilidad = CalcularProbabilidad(elegido);

        if (UnityEngine.Random.value > probabilidad)
        {
            if (debugLogs)
                Debug.Log($" {elegido.nombre}: no ocurrió (prob {probabilidad:P0})");
            return false;
        }

        LanzarEvento(elegido);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.gameData.resumenDia.RegistrarEvento();
            Debug.Log($"Evento registrado. Total del día: {GameManager.Instance.gameData.resumenDia.eventosOcurridos}");
        }
        return true;
    }

    private bool PuedeOcurrir(EventoEstadoConfig e)
    {
        float valor = ObtenerValorContador(e.sistema);
        return valor <= e.umbral;
    }

    private float CalcularProbabilidad(EventoEstadoConfig e)
    {
        float valor = ObtenerValorContador(e.sistema);
        float t = Mathf.InverseLerp(e.umbral, 0f, valor);
        return Mathf.Lerp(e.probabilidadBase, e.probabilidadMaxima, t);
    }

    private float ObtenerValorContador(TipoSistema sistema)
    {
        var data = GameManager.Instance.GameData;

        return sistema switch
        {
            TipoSistema.Limpieza => data.contadorLimpieza,
            TipoSistema.Polea => data.contadorPolea,
            TipoSistema.Puerta => data.contadorPuerta,
            _ => 100f
        };
    }

    private EventoEstadoConfig ElegirEvento(List<EventoEstadoConfig> lista)
    {
        float pesoTotal = 0f;
        foreach (var e in lista) pesoTotal += e.peso;

        float r = UnityEngine.Random.Range(0f, pesoTotal);
        float acumulado = 0f;

        foreach (var e in lista)
        {
            acumulado += e.peso;
            if (r <= acumulado) return e;
        }

        return lista[0];
    }

    
    private void LanzarEvento(EventoEstadoConfig e)
    {
        eventoEnCurso = true;

        if (debugLogs)
            Debug.Log($" Lanzando evento de estado: {e.nombre}");

        if (GameManager.Instance != null)
            GameManager.Instance.GameData.eventoOcurriendo = true;

        if (e.prefab == null)
        {
            Debug.LogError($"El evento '{e.nombre}' no tiene prefab asignado");
            TerminarEvento();
            return;
        }

       
        eventoInstanciado = Instantiate(e.prefab);

        var eventoBase = eventoInstanciado.GetComponent<EventoBase>();
        if (eventoBase != null)
        {
            eventoBase.OnTerminado += TerminarEventoDesdeBase;
        }
        else
        {
           
            Destroy(eventoInstanciado, 5f);
            Invoke(nameof(TerminarEvento), 5f);
        }
    }

  
    private void TerminarEventoDesdeBase()
    {
        if (debugLogs)
            Debug.Log("EventoBase avisó que terminó");

        TerminarEvento();
    }

    private void TerminarEvento()
    {
        eventoEnCurso = false;
        eventoInstanciado = null;

        if (GameManager.Instance != null)
            GameManager.Instance.GameData.eventoOcurriendo = false;

        OnEventoTerminado?.Invoke(eventoInstanciado);

        if (debugLogs)
            Debug.Log(" Evento de estado finalizado. Listo para el siguiente.");
    }
}