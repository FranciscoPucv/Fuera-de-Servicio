using UnityEngine;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    [System.Serializable]
    public class EventoConfig
    {
        public string nombre;
        public GameObject prefab;
        [Range(0f, 1f)] public float peso = 1f;
        public bool requiereAscensorDetenido = true; 
    }

 
    public List<EventoConfig> eventosDisponibles = new List<EventoConfig>();

    public float tiempoMinEntreEventos = 15f;
    public float tiempoMaxEntreEventos = 30f;

    public bool eventosActivos = true;
    public bool eventoEnCurso = false;

    private AscensorController ascensor;

    private float timerSiguienteEvento = 0f;

    private void Start()
    {
        ascensor = FindObjectOfType<AscensorController>();
        ProgramarSiguienteEvento();
    }

    private void Update()
    {
        if (!eventosActivos || eventoEnCurso) return;
        if (GameManager.Instance != null && GameManager.Instance.GameData.eventoOcurriendo) return;
        if (Time.timeScale == 0f) return;

        timerSiguienteEvento -= Time.deltaTime;

        if (timerSiguienteEvento <= 0f)
        {
            LanzarEventoAleatorio();
        }
    }

    private void ProgramarSiguienteEvento()
    {
        timerSiguienteEvento = Random.Range(tiempoMinEntreEventos, tiempoMaxEntreEventos);
        Debug.Log($" Próximo evento en {timerSiguienteEvento:F1} segundos");
    }

    private void LanzarEventoAleatorio()
    {
        if (eventosDisponibles.Count == 0) return;

        List<EventoConfig> eventosValidos = new List<EventoConfig>();
        foreach (var e in eventosDisponibles)
        {
            if (e.requiereAscensorDetenido && ascensor != null && ascensor.enMovimiento)
                continue; // Saltar este evento porque el ascensor está en movimiento

            eventosValidos.Add(e);
        }

        if (eventosValidos.Count == 0)
        {
            Debug.Log("No hay eventos válidos (ascensor en movimiento). Reintentando en 2s...");
            timerSiguienteEvento = 2f;
            return;
        }

        EventoConfig elegido = ElegirEventoPorPeso(eventosValidos);
        if (elegido == null || elegido.prefab == null) return;

        LanzarEvento(elegido);
    }

    private EventoConfig ElegirEventoPorPeso(List<EventoConfig> lista)
    {
        float pesoTotal = 0f;
        foreach (var e in lista) pesoTotal += e.peso;

        float r = Random.Range(0f, pesoTotal);
        float acumulado = 0f;

        foreach (var e in lista)
        {
            acumulado += e.peso;
            if (r <= acumulado) return e;
        }

        return lista[0];
    }

    private void LanzarEvento(EventoConfig config)
    {
        eventoEnCurso = true;

        if (GameManager.Instance != null)
            GameManager.Instance.GameData.eventoOcurriendo = true;

        Debug.Log($"Lanzando evento: {config.nombre}");

        Vector3 posicionSpawn = CalcularPosicionSpawn();
        GameObject instancia = Instantiate(config.prefab, posicionSpawn, Quaternion.identity);

        Maleante maleante = instancia.GetComponent<Maleante>();
        if (maleante != null)
        {
            maleante.OnMaleanteTerminado += TerminarEvento;
        }
    }

    private Vector3 CalcularPosicionSpawn()
    {
        if (ascensor == null) return Vector3.zero;

        float ladoX = Random.value > 0.5f ? -8f : 8f;
        float offsetY = Random.Range(-1f, 1f);

        return new Vector3(ladoX, ascensor.transform.position.y + offsetY, 0);
    }

    private void TerminarEvento(Maleante m)
    {
        if (m != null)
            m.OnMaleanteTerminado -= TerminarEvento;

        eventoEnCurso = false;

        if (GameManager.Instance != null)
            GameManager.Instance.GameData.eventoOcurriendo = false;

        Debug.Log("Evento terminado");
        ProgramarSiguienteEvento();
    }
}