using UnityEngine;
using TMPro;
using System.Collections;

public class PanelMensajesMonitor : MonoBehaviour
{
   
    public static PanelMensajesMonitor Instance { get; private set; }

    public TextMeshProUGUI textoMensaje;
    public CanvasGroup canvasGroup;

    public float duracionMensaje = 3f;
    public Color colorPorDefecto = Color.white;

    private Coroutine corrutinaActual;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null) canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
       
        if (Instance == this)
            Instance = null;
    }

    public void MostrarMensaje(string mensaje, Color color, float duracion = -1f)
    {
        if (textoMensaje == null) return;

        if (corrutinaActual != null)
            StopCoroutine(corrutinaActual);

        textoMensaje.text = mensaje;
        textoMensaje.color = color;

        float duracionReal = duracion > 0f ? duracion : duracionMensaje;
        corrutinaActual = StartCoroutine(MostrarYOcultar(duracionReal));
    }
    public void MostrarMensajeInmediato(string mensaje, Color color)
    {
        if (textoMensaje == null) return;

        if (corrutinaActual != null)
        {
            StopCoroutine(corrutinaActual);
            corrutinaActual = null;
        }

        textoMensaje.text = mensaje;
        textoMensaje.color = color;

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
    }

    private IEnumerator MostrarYOcultar(float duracion)
    {
        float t = 0f;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / 0.2f);
            yield return null;
        }
        if (canvasGroup != null) canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(Mathf.Max(0.1f, duracion - 0.4f));

        t = 0f;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / 0.2f);
            yield return null;
        }
        if (canvasGroup != null) canvasGroup.alpha = 0f;

        corrutinaActual = null;
    }
}