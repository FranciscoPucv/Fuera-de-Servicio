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
        public bool esMejoraUnica;
    }

    [System.Serializable]
    private class CatalogoWrapper
    {
        public List<ItemData> items;
    }

    public TextAsset archivoJson;

   
    public GameObject cuadroTexto;
    public TextMeshProUGUI textoDescripcion;
    public TextMeshProUGUI textoBotonComprar;
    public Button botonComprar;
    public TextMeshProUGUI textoDineroDisponible;

   
    [Tooltip("Escena a la que vuelve al salir de la tienda")]
    public string nombreEscenaNoche = "EscenaNoche";


    private List<ItemData> listaItems = new List<ItemData>();
    private ItemData itemSeleccionadoActual;

  
    private static HashSet<string> mejorasCompradas = new HashSet<string>();
    private static Dictionary<string, int> consumibles = new Dictionary<string, int>();

    private void Awake()
    {
        CargarDatosDesdeJson();
    }

    private void Start()
    {
        if (cuadroTexto != null) cuadroTexto.SetActive(false);
        ActualizarTextoDinero();

        
        if (GameManager.Instance != null && GameManager.Instance.gameData != null)
        {
            GameManager.Instance.gameData.OnDataChanged += ActualizarTextoDinero;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameData != null)
        {
            GameManager.Instance.gameData.OnDataChanged -= ActualizarTextoDinero;
        }
    }

    private void CargarDatosDesdeJson()
    {
        if (archivoJson != null)
        {
            CatalogoWrapper catalogo = JsonUtility.FromJson<CatalogoWrapper>(archivoJson.text);
            listaItems = catalogo.items;
           
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
                textoDescripcion.text = item.descripcion;

            if (item.esMejoraUnica && mejorasCompradas.Contains(item.id))
            {
                if (textoBotonComprar != null) textoBotonComprar.text = "Ya adquirido";
                if (botonComprar != null) botonComprar.interactable = false;
            }
            else
            {
                if (textoBotonComprar != null)
                    textoBotonComprar.text = $"Comprar (${item.precio})";

                
                if (botonComprar != null)
                    botonComprar.interactable = (ObtenerDinero() >= item.precio);
            }
        }
        
    }

    public void ComprarItemActual()
    {
        if (itemSeleccionadoActual == null) return;
        if (GameManager.Instance == null) return;

        int dineroActual = ObtenerDinero();

        if (dineroActual >= itemSeleccionadoActual.precio)
        {
            GameManager.Instance.gameData.dineroTotal -= itemSeleccionadoActual.precio;
            GameManager.Instance.gameData.OnDataChanged?.Invoke();
            ActualizarTextoDinero();

          
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
                    botonComprar.interactable = (ObtenerDinero() >= itemSeleccionadoActual.precio);
            }

          
            EfectosManager.AplicarEfecto(itemSeleccionadoActual.id);
        }
        
    }

    private int ObtenerDinero()
    {
        if (GameManager.Instance == null) return 0;
        if (GameManager.Instance.gameData == null) return 0;
        return GameManager.Instance.gameData.dineroTotal;
    }

    private void ActualizarTextoDinero()
    {
        if (textoDineroDisponible != null)
            textoDineroDisponible.text = $"${ObtenerDinero()}";
    }

    public void SalirDeLaTienda()
    {
        Debug.Log($"Saliendo de la tienda. Volviendo a: {nombreEscenaNoche}");
        SceneManager.LoadScene(nombreEscenaNoche);
    }
}