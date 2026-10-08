using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class EventoPuertaAtascada : EventoBase
{
    public string mensajeInicial = "¡La puerta se atascó!";
    public string mensajeInstruccion = "Presiona [C] 10 veces rápido!";
    public Color colorMensaje = Color.yellow;

    [Header("mini juego")]
    public int pressesRequeridos = 10;
    public float tiempoLimite = 3f;

    private int pressesActuales = 0;
    private float tiempoRestante = 0f;
    private bool minijuegoActivo = false;
    private bool yaResuelto = false;

    [Header("---input---")]
    public InputActionAsset inputActions;
    public string actionMapName = "Gameplay";
    public string actionName = "resolver";
    private InputAction inputResolver;

 
    public GameObject animacionPuertaPrefab;
    public Transform puntoAnimacion;
    private GameObject animacionInstanciada;

    
    public int dañoAlContadorPuerta = 10;

    private void Awake()
    {
        if (inputActions != null)
        {
            inputResolver = inputActions.FindActionMap(actionMapName)?.FindAction(actionName);

            if (inputResolver == null)
                Debug.LogError($" No se encontró '{actionName}' en '{actionMapName}'");
        }
        
    }


    private void OnEnable()
    {
        if (inputResolver != null)
        {
            inputResolver.Enable();
            inputResolver.performed += OnResolverPerformed;
        }
    }

    private void OnDisable()
    {
        if (inputResolver != null)
        {
            inputResolver.performed -= OnResolverPerformed;
            inputResolver.Disable();
        }
    }

    protected override void Start()
    {
        base.Start();

        Debug.Log("EventoPuertaAtascada iniciado");
        
        MostrarMensajesIniciales();

      
        if (animacionPuertaPrefab != null)
        {
            Transform padre = puntoAnimacion != null ? puntoAnimacion : null;
            animacionInstanciada = Instantiate(animacionPuertaPrefab, padre);
        }

        IniciarMinijuego();
    }

    private void IniciarMinijuego()
    {
        pressesActuales = 0;
        tiempoRestante = tiempoLimite;
        minijuegoActivo = true;

        ActualizarTextoProgreso();
    }


    protected override void Update()
    {
        if (!minijuegoActivo) return;
        tiempoRestante -= Time.deltaTime;
        ActualizarTextoProgreso();
        if (tiempoRestante <= 0f)
        {
            FallarMinijuego();
        }
    }

    private void OnResolverPerformed(InputAction.CallbackContext context)
    {
        

        if (!minijuegoActivo || yaResuelto) return;

        pressesActuales++;
        
        ActualizarTextoProgreso();

      
        if (pressesActuales >= pressesRequeridos)
        {
            ResolverMinijuego();
        }
    }

    private void ActualizarTextoProgreso()
    {
        if (PanelMensajesMonitor.Instance == null) return;

        string texto = $"{mensajeInstruccion}\n";
        texto += $"Progreso: {pressesActuales}/{pressesRequeridos}\n";
        texto += $"Tiempo: {Mathf.Max(0, tiempoRestante):F1}s";

        PanelMensajesMonitor.Instance.MostrarMensajeInmediato(texto, colorMensaje);
    }

    private void MostrarMensajesIniciales()
    {
        if (PanelMensajesMonitor.Instance != null)
        {
            PanelMensajesMonitor.Instance.MostrarMensajeInmediato(mensajeInicial, colorMensaje);
        }
    }

   
    private void ResolverMinijuego()
    {
        if (yaResuelto) return;
        yaResuelto = true;
        minijuegoActivo = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ModificarPuerta(5f);
        }

        if (PanelMensajesMonitor.Instance != null)
        {
            PanelMensajesMonitor.Instance.MostrarMensaje("¡Puerta desatascada!", Color.green, 2f);
        }

        if (animacionInstanciada != null)
            Destroy(animacionInstanciada);

        Terminar();
    }

    private void FallarMinijuego()
    {
        if (yaResuelto) return;
        yaResuelto = true;
        minijuegoActivo = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ModificarPuerta(-dañoAlContadorPuerta);
        }

        if (PanelMensajesMonitor.Instance != null)
        {
            PanelMensajesMonitor.Instance.MostrarMensaje("¡No la desatascaste a tiempo!", Color.red, 3f);
        }

        if (animacionInstanciada != null)
            Destroy(animacionInstanciada);

        Terminar();
    }
}