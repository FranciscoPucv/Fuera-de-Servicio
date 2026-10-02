using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TiendaManager : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string id;
        public string nombre;
        public int precio;
        [TextArea(2, 5)]
        public string descripcion;
        public bool esMejoraUnica; // True para Malla, Freno reforzado, Letrero
    }

    [System.Serializable]
    private class CatalogoWrapper
    {
        public List<ItemData> items;
    }

    [Header("Archivo de Datos")]
    public TextAsset archivoJson;

    [Header("Referencias Visuales")]
    public GameObject cuadroTexto;         
    public TextMeshProUGUI textoDescripcion; 
    public TextMeshProUGUI textoBotonComprar;
    public Button botonComprar;
    public TextMeshProUGUI textoDineroDisponible;

    [Header("Economía (Simulada para pruebas)")]
    public static int dineroJugador = 25000; // Saldo de prueba
    public static HashSet<string> mejorasCompradas = new HashSet<string>();
    public static Dictionary<string, int> consumibles = new Dictionary<string, int>();
    private List<ItemData> listaItems = new List<ItemData>();
    private ItemData itemSeleccionadoActual;
    
    void Awake()
    {
        CargarDatosDesdeJson();
    }

    void Start()
    {
        if (cuadroTexto != null) cuadroTexto.SetActive(false);
        ActualizarTextoDinero();
    }

    private void CargarDatosDesdeJson()
    {
        if (archivoJson != null)
        {
            CatalogoWrapper catalogo = JsonUtility.FromJson<CatalogoWrapper>(archivoJson.text);
            listaItems = catalogo.items;
        }
        else
        {
            Debug.LogError("Falta asignar el archivo JSON en TiendaManager.");
        }
    }

    public void SeleccionarItem(string itemId)
    {
        ItemData item = listaItems.Find(i => i.id.ToLower() == itemId.ToLower());

        if (item != null)
        {
            itemSeleccionadoActual = item;
            if (cuadroTexto != null) cuadroTexto.SetActive(true);

            if (textoDescripcion != null)
            {
                textoDescripcion.text = item.descripcion;
            }

            // Comprobar si ya fue comprado (si es mejora única)
            if (item.esMejoraUnica && mejorasCompradas.Contains(item.id))
            {
                if (textoBotonComprar != null) textoBotonComprar.text = "Ya adquirido";
                if (botonComprar != null) botonComprar.interactable = false;
            }
            else
            {
                if (textoBotonComprar != null) textoBotonComprar.text = $"Comprar (${item.precio})";
                
                // Desactiva el boton si no alcanza la plata
                if (botonComprar != null)
                {
                    botonComprar.interactable = (dineroJugador >= item.precio);
                }
            }
        }
    }

    public void ComprarItemActual()
    {
        if (itemSeleccionadoActual == null) return;

        if (dineroJugador >= itemSeleccionadoActual.precio)
        {
            dineroJugador -= itemSeleccionadoActual.precio;
            ActualizarTextoDinero();

            // Guardar en el inventario correspondiente
            if (itemSeleccionadoActual.esMejoraUnica)
            {
                mejorasCompradas.Add(itemSeleccionadoActual.id);
                if (textoBotonComprar != null) textoBotonComprar.text = "Ya adquirido";
                if (botonComprar != null) botonComprar.interactable = false;
            }
            else
            {
                if (!consumibles.ContainsKey(itemSeleccionadoActual.id))
                    consumibles[itemSeleccionadoActual.id] = 0;

                consumibles[itemSeleccionadoActual.id]++;
                Debug.Log($"Tienes {consumibles[itemSeleccionadoActual.id]} unidades de {itemSeleccionadoActual.nombre}");

                if (botonComprar != null)
                {
                    botonComprar.interactable = (dineroJugador >= itemSeleccionadoActual.precio);
                }
            }

            Debug.Log($"¡Compra exitosa! Dinero restante: ${dineroJugador}");
            EfectosManager.AplicarEfecto(itemSeleccionadoActual.id);
        }
        else
        {
            Debug.LogWarning("Dinero insuficiente.");
        }
    }

    private void ActualizarTextoDinero()
    {
        if (textoDineroDisponible != null)
        {
            textoDineroDisponible.text = $"${dineroJugador}";
        }
    }

    public void SalirDeLaTienda()
    {
        SceneManager.LoadScene("SampleScene");
    }
}