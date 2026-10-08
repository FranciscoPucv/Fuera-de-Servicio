using UnityEngine;
using TMPro;
using System.Collections;

public class PanelAccion : MonoBehaviour
{
    public static PanelAccion Instance { get; private set; }

    [Header("=== UI ===")]
    public GameObject panelContenedor;
    public TextMeshProUGUI textoAccion;
    public TextMeshProUGUI textoTiempo; 
    public UnityEngine.UI.Image barraTiempo; 

    [Header("=== CONFIGURACIÓN ===")]
    public Color colorNormal = Color.white;
    public Color colorUrgente = Color.red;
    public float tiempoUrgente = 0.5f; 

    private Coroutine corrutinaActual;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (panelContenedor != null)
            panelContenedor.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void MostrarAccion(string texto, float duracion)
    {
        if (corrutinaActual != null)
            StopCoroutine(corrutinaActual);

        corrutinaActual = StartCoroutine(MostrarAccionCorrutina(texto, duracion));
    }

    private IEnumerator MostrarAccionCorrutina(string texto, float duracion)
    {
        if (panelContenedor != null)
            panelContenedor.SetActive(true);

        if (textoAccion != null)
        {
            textoAccion.text = texto;
            textoAccion.color = colorNormal;
        }

        float timer = 0f;

        while (timer < duracion)
        {
            timer += Time.unscaledDeltaTime;
            float restante = duracion - timer;
            float porcentaje = restante / duracion;

           
            if (textoTiempo != null)
                textoTiempo.text = restante.ToString("F1") + "s";

            if (barraTiempo != null)
                barraTiempo.fillAmount = porcentaje;

            
            if (textoAccion != null)
            {
                if (restante <= tiempoUrgente)
                {
                    float t = 1f - (restante / tiempoUrgente);
                    textoAccion.color = Color.Lerp(colorNormal, colorUrgente, t);
                }
            }

            yield return null;
        }

  
        OcultarAccion();
    }
    

    public void OcultarAccion()
    {
        if (corrutinaActual != null)
        {
            StopCoroutine(corrutinaActual);
            corrutinaActual = null;
        }

        if (panelContenedor != null)
            panelContenedor.SetActive(false);
    }

    public bool EstaVisible()
    {
        return panelContenedor != null && panelContenedor.activeSelf;
    }
   
    public void MostrarResultado(string texto, Color color, float duracion = 2f)
    {
        if (corrutinaActual != null)
            StopCoroutine(corrutinaActual);

        corrutinaActual = StartCoroutine(MostrarResultadoCorrutina(texto, color, duracion));
    }

    private IEnumerator MostrarResultadoCorrutina(string texto, Color color, float duracion)
    {
        if (panelContenedor != null)
            panelContenedor.SetActive(true);

        if (textoAccion != null)
        {
            textoAccion.text = texto;
            textoAccion.color = color;
        }

        if (textoTiempo != null) textoTiempo.text = "";
        if (barraTiempo != null) barraTiempo.fillAmount = 0f;

        yield return new WaitForSecondsRealtime(duracion);

        OcultarAccion();
    }
}