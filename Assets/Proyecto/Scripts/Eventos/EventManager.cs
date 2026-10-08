using UnityEngine;

public class EventManager : MonoBehaviour
{
    public EventoMaleanteManager maleanteManager;
    public EventoEstadoManager estadoManager;
    // public EventoClimaManager climaManager; // Futuro

    public float tiempoMinEntreEventos = 15f;
    public float tiempoMaxEntreEventos = 30f;

    public bool eventosActivos = true;
    public bool eventoEnCurso = false;

    private float timerSiguienteEvento = 0f;

    private void Start()
    {
     
        if (maleanteManager != null)
            maleanteManager.OnEventoTerminado += TerminarEvento;

        if (estadoManager != null)
            estadoManager.OnEventoTerminado += TerminarEvento;

        ProgramarSiguienteEvento();
    }

    private void Update()
    {
        if (!eventosActivos || eventoEnCurso)
        {
            Debug.Log($"EventManager bloqueado. eventosActivos={eventosActivos}, eventoEnCurso={eventoEnCurso}");
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.GameData.eventoOcurriendo)
        {
            Debug.Log(" EventManager bloqueado por GameData.eventoOcurriendo");
            return;
        }
        if (!eventosActivos || eventoEnCurso) return;
        if (GameManager.Instance != null && GameManager.Instance.GameData.eventoOcurriendo) return;
        if (Time.timeScale == 0f) return;

        timerSiguienteEvento -= Time.deltaTime;

        if (timerSiguienteEvento <= 0f)
        {
            IntentarLanzarEvento();
        }

    }

    private void ProgramarSiguienteEvento()
    {
        timerSiguienteEvento = Random.Range(tiempoMinEntreEventos, tiempoMaxEntreEventos);
        Debug.Log($" Próximo evento en {timerSiguienteEvento:F1}s");
    }

    private void IntentarLanzarEvento()
    {
       
        bool algunEventoLanzado = false;

        if (!algunEventoLanzado && estadoManager != null)
        {
            algunEventoLanzado = estadoManager.IntentarLanzarEvento();

        }

        if (!algunEventoLanzado && maleanteManager != null)
        {
            algunEventoLanzado = maleanteManager.IntentarLanzarEvento();
        }

       
        if (!algunEventoLanzado)
        {
            ProgramarSiguienteEvento();
        }
        else
        {
            eventoEnCurso = true;
            if (GameManager.Instance != null)
                GameManager.Instance.GameData.eventoOcurriendo = true;
        }
    }

    private void TerminarEvento(GameObject evento)
    {
        eventoEnCurso = false;

        if (GameManager.Instance != null)
            GameManager.Instance.GameData.eventoOcurriendo = false;

        ProgramarSiguienteEvento();
    }
}