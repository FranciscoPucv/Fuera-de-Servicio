using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager Instance { get; private set; }


    public TextAsset archivoJson;

    public GameObject panelDialogo;
    public TextMeshProUGUI textoHablante;
    public TextMeshProUGUI textoContenido;
    public GameObject indicadorContinuar; 

   
    public float velocidadTipeo = 0.03f; 
    public InputActionAsset inputActions;
    public string actionMapName = "Gameplay";
    public string actionName = "resolver";

    private InputAction inputContinuar;

    
    private List<Dialogo> dialogosCargados = new List<Dialogo>();
    private Dialogo dialogoActual;
    private int indiceLineaActual = 0;
    private bool escribiendo = false;
    private bool esperandoInput = false;
    private Coroutine corrutinaTipeo;


    public GameData gameDataFallback;

    private void Awake()
    {
        

        Instance = this; 

        CargarDialogos();

        if (inputActions != null)
        {
            inputContinuar = inputActions.FindActionMap(actionMapName)?.FindAction(actionName);
        }
    }
    private void Start()
    {
        if (panelDialogo != null) panelDialogo.SetActive(false);
        if (indicadorContinuar != null) indicadorContinuar.SetActive(false);

        ValidarPlaceholders();
    }

    private void ValidarPlaceholders()
    {
        string[] validos = {
        "dineroGanado", "pasajerosTransportados", "eventos", "grafitis", "basura",
        "dineroTotal", "pasajerosActuales", "limpieza", "polea", "puerta", "hora"
    };

        foreach (var dialogo in dialogosCargados)
        {
            foreach (var linea in dialogo.lineas)
            {
                var matches = System.Text.RegularExpressions.Regex.Matches(
                    linea.texto, @"\{([^}]+)\}");

                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    string clave = match.Groups[1].Value.Trim();

                    if (System.Array.IndexOf(validos, clave) < 0)
                    {
                        Debug.LogWarning($"Placeholder desconocido '{{{clave}}}' en diálogo '{dialogo.id}'");
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        
        if (Instance == this)
            Instance = null;
    }

    private void OnEnable()
    {
        if (inputContinuar != null)
        {
            inputContinuar.Enable();
            inputContinuar.performed += OnContinuarPerformed;
        }
    }

    private void OnDisable()
    {
        if (inputContinuar != null)
        {
            inputContinuar.performed -= OnContinuarPerformed;
            inputContinuar.Disable();
        }
    }

    

    private void CargarDialogos()
    {
        if (archivoJson == null)
        {
            Debug.LogError("DialogoManager: archivoJson no asignado");
            return;
        }

        try
        {
            DialogosWrapper wrapper = JsonUtility.FromJson<DialogosWrapper>(archivoJson.text);
            dialogosCargados = wrapper.dialogos;
            Debug.Log($"{dialogosCargados.Count} diálogos cargados");
        }
        catch (System.Exception e)
        {
            Debug.LogError($" Error al parsear diálogos: {e.Message}");
        }
    }

    public void MostrarDialogo(string id)
    {
        Dialogo dialogo = dialogosCargados.Find(d => d.id == id);

        if (dialogo == null)
        {
            Debug.LogWarning($" Diálogo '{id}' no encontrado");
            return;
        }

        dialogoActual = dialogo;
        indiceLineaActual = 0;

        if (EstadoJuegoManager.Instance != null)
            EstadoJuegoManager.Instance.CambiarEstado(EstadoJuego.EnDialogo);

        if (panelDialogo != null) panelDialogo.SetActive(true);
        MostrarLineaActual();
    }

    private void MostrarLineaActual()
    {
        if (dialogoActual == null) return;
        if (indiceLineaActual >= dialogoActual.lineas.Count)
        {
            TerminarDialogo();
            return;
        }

        LineaDialogo linea = dialogoActual.lineas[indiceLineaActual];

        if (textoHablante != null)
            textoHablante.text = linea.hablante;

        if (textoContenido != null)
        {
            textoContenido.text = "";
            if (corrutinaTipeo != null) StopCoroutine(corrutinaTipeo);
            corrutinaTipeo = StartCoroutine(EscribirTexto(linea.texto));
        }

        if (indicadorContinuar != null)
            indicadorContinuar.SetActive(false);
    }
    private IEnumerator EscribirTexto(string textoCompleto)
    {
        float timeout = 3f;
        float timer = 0f;

        while ((GameManager.Instance == null || GameManager.Instance.gameData == null) && timer < timeout)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        if (GameManager.Instance == null || GameManager.Instance.gameData == null)
        {
            Debug.LogError("GameManager nunca estuvo listo. Mostrando texto sin reemplazar.");
        }
        else
        {
            Debug.Log($"GameManager listo después de {timer:F2}s");
        }

        textoCompleto = ReemplazarPlaceholders(textoCompleto);

        escribiendo = true;
        esperandoInput = false;

        for (int i = 0; i < textoCompleto.Length; i++)
        {
            textoContenido.text = textoCompleto.Substring(0, i + 1);
            yield return new WaitForSecondsRealtime(velocidadTipeo);
        }

        textoContenido.text = textoCompleto;
        escribiendo = false;
        esperandoInput = true;

        if (indicadorContinuar != null)
            indicadorContinuar.SetActive(true);

        corrutinaTipeo = null;
    }

 
    private void OnContinuarPerformed(InputAction.CallbackContext context)
    {
       
        if (dialogoActual == null) return;

    
        if (escribiendo)
        {
            if (corrutinaTipeo != null) StopCoroutine(corrutinaTipeo);
            corrutinaTipeo = null;

            LineaDialogo linea = dialogoActual.lineas[indiceLineaActual];
            textoContenido.text = linea.texto;

            escribiendo = false;
            esperandoInput = true;

            if (indicadorContinuar != null)
                indicadorContinuar.SetActive(true);
            return;
        }

        
        if (esperandoInput)
        {
            SiguienteLinea();
        }
    }

    private void SiguienteLinea()
    {
        indiceLineaActual++;

        if (indiceLineaActual >= dialogoActual.lineas.Count)
        {
            TerminarDialogo();
        }
        else
        {
            MostrarLineaActual();
        }
    }

    private void TerminarDialogo()
    {
   

        dialogoActual = null;
        indiceLineaActual = 0;
        escribiendo = false;
        esperandoInput = false;

        if (panelDialogo != null) panelDialogo.SetActive(false);
        if (indicadorContinuar != null) indicadorContinuar.SetActive(false);

        
        if (EstadoJuegoManager.Instance != null)
            EstadoJuegoManager.Instance.CambiarEstado(EstadoJuego.Jugando);
    }

    private string ReemplazarPlaceholders(string texto)
    {
        GameData data = gameDataFallback;

        if (data == null && GameManager.Instance != null)
            data = GameManager.Instance.gameData;

        if (data == null)
        {
            Debug.LogWarning("No hay GameData disponible. Devolviendo texto sin reemplazar.");
            return texto;
        }

        var resumen = data.resumenDia;

        texto = texto.Replace("{dineroGanado}", resumen.DineroGanado.ToString());

        if (GameManager.Instance == null) return texto;

        //estos son los reemplazos
        texto = texto.Replace("{dineroGanado}", resumen.DineroGanado.ToString());
        texto = texto.Replace("{pasajerosTransportados}", resumen.PasajerosTransportados.ToString());
        texto = texto.Replace("{eventos}", resumen.eventosOcurridos.ToString());
        texto = texto.Replace("{grafitis}", resumen.grafitisHechos.ToString());
        texto = texto.Replace("{basura}", resumen.basuraTirada.ToString());

  
        texto = texto.Replace("{dineroTotal}", data.dineroTotal.ToString());
        texto = texto.Replace("{pasajerosActuales}", data.pasajerosTotales.ToString());
        texto = texto.Replace("{limpieza}", data.contadorLimpieza.ToString("F0"));
        texto = texto.Replace("{polea}", data.contadorPolea.ToString("F0"));
        texto = texto.Replace("{puerta}", data.contadorPuerta.ToString("F0"));
        texto = texto.Replace("{hora}", data.horaActual.ToString());

        return texto;
    }

 
    public bool HayDialogoActivo() => dialogoActual != null;
}