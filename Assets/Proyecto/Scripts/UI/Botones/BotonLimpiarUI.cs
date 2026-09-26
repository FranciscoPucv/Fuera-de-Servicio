using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotonLimpiarUI : MonoBehaviour
{
    
    public Button boton;
    public LimpiezaManager limpiezaManager;
    public TextMeshProUGUI textoBoton; 

    public Color colorHabilitado = Color.white;
    public Color colorDeshabilitado = new Color(0.5f, 0.5f, 0.5f, 0.5f);

    private void Start()
    {
        if (boton == null)
            boton = GetComponent<Button>();

        boton.onClick.AddListener(OnBotonPresionado);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameData.OnDataChanged += ActualizarEstado;
        }

        if (limpiezaManager != null)
        {
            limpiezaManager.OnEstadoCambio += ActualizarEstado;
        }

        ActualizarEstado();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.GameData != null)
            GameManager.Instance.GameData.OnDataChanged -= ActualizarEstado;

        if (limpiezaManager != null)
            limpiezaManager.OnEstadoCambio -= ActualizarEstado;
    }

    private void OnBotonPresionado()
    {
        if (limpiezaManager != null)
        {
            limpiezaManager.IntentarLimpiar();
        }
    }

    private void ActualizarEstado()
    {
        if (boton == null || limpiezaManager == null) return;

        bool sePuedeLimpiar = limpiezaManager.SePuedeLimpiar();

        boton.interactable = sePuedeLimpiar;

        ColorBlock colores = boton.colors;
        colores.normalColor = sePuedeLimpiar ? colorHabilitado : colorDeshabilitado;
        boton.colors = colores;

        if (textoBoton != null)
        {
            int grafitis = limpiezaManager.ContarGrafitis();
            textoBoton.text = $"Limpiar (${limpiezaManager.costoLimpiar})";
        }
    }

    private void Update()
    {
       
        ActualizarEstado();
    }
}