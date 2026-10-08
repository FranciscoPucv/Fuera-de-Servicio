using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class EventoFrenado : MonoBehaviour
{
   
    public float ventanaTiempo = 1.5f;
    public float rangoPerfecto = 0.05f;
    public float rangoBueno = 0.15f;

   
    public float dañoFallo = 10f;
    public float dañoBueno = 3f;

   
    public AscensorManager ascensorManager;
    public InputActionAsset inputActions;
    public string actionMapName = "Gameplay";
    public string actionName = "resolver";

    private InputAction inputFrenar;
    private bool eventoActivo = false;
    private bool yaPresiono = false;
    private float progresoAlPresionar = -1f;

    private void Awake()
    {
        if (inputActions != null)
        {
            inputFrenar = inputActions.FindActionMap(actionMapName)?.FindAction(actionName);
        }
    }

    private void OnEnable()
    {
        if (inputFrenar != null)
        {
            inputFrenar.Enable();
            inputFrenar.performed += OnFrenarPerformed;
        }

        if (ascensorManager != null)
        {
            ascensorManager.OnPuntoDeFrenado += IniciarEvento;
        }
    }

    private void OnDisable()
    {
        if (inputFrenar != null)
        {
            inputFrenar.performed -= OnFrenarPerformed;
            inputFrenar.Disable();
        }

        if (ascensorManager != null)
        {
            ascensorManager.OnPuntoDeFrenado -= IniciarEvento;
        }
    }

    private void IniciarEvento()
    {

        

        if (eventoActivo) return;
        
        eventoActivo = true;
        yaPresiono = false;
        progresoAlPresionar = -1f;

       
        if (PanelAccion.Instance != null)
        {
            PanelAccion.Instance.MostrarAccion("¡FRENA! [ESPACIO]", ventanaTiempo);
        }

        StartCoroutine(EsperarFrenado());
    }

    private void OnFrenarPerformed(InputAction.CallbackContext context)
    {
        if (!eventoActivo || yaPresiono) return;

        yaPresiono = true;
        progresoAlPresionar = ascensorManager.ascensores[0].progreso;

    }

    private IEnumerator EsperarFrenado()
    {
        yield return new WaitForSecondsRealtime(ventanaTiempo);
        EvaluarFrenado();
    }

    private void EvaluarFrenado()
    {
        if (!eventoActivo) return;

        eventoActivo = false;
        
        if (PanelAccion.Instance != null)
            PanelAccion.Instance.OcultarAccion();

        if (!yaPresiono)
        {
            
            AplicarResultado(ResultadoFrenado.Malo, "¡No frenaste a tiempo!");
            return;
        }

        var asc = ascensorManager.ascensores[0];
        float umbral = ascensorManager.umbralFrenado;

        if (!asc.estaAbajo)
            umbral = 1f - ascensorManager.umbralFrenado;

        float diferencia = Mathf.Abs(progresoAlPresionar - umbral);

        if (diferencia <= rangoPerfecto)
        {
            AplicarResultado(ResultadoFrenado.Perfecto, "¡Frenado perfecto!");
        }
        else if (diferencia <= rangoBueno)
        {
            AplicarResultado(ResultadoFrenado.Bueno, "Frenado aceptable");
        }
        else
        {
            AplicarResultado(ResultadoFrenado.Malo, "¡Frenado brusco!");
        }
    }

    private enum ResultadoFrenado { Perfecto, Bueno, Malo }

    private void AplicarResultado(ResultadoFrenado resultado, string mensaje)
    {
        if (PanelAccion.Instance != null)
        {
            Color color = resultado switch
            {
                ResultadoFrenado.Perfecto => Color.green,
                ResultadoFrenado.Bueno => Color.yellow,
                ResultadoFrenado.Malo => Color.red,
                _ => Color.white
            };
            
            if (PanelAccion.Instance != null)
                PanelAccion.Instance.MostrarResultado(mensaje, color, 2f);
        }

        float daño = resultado switch
        {
            ResultadoFrenado.Perfecto => 0f,
            ResultadoFrenado.Bueno => dañoBueno,
            ResultadoFrenado.Malo => dañoFallo,
            _ => 0f
        };

        if (daño > 0f && GameManager.Instance != null)
        {
            GameManager.Instance.ModificarPolea(-daño);
        }

    }
}