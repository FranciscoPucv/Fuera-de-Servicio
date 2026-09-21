using UnityEngine;
using System;

public class Maleante : MonoBehaviour
{
    [Header("Config")]
    public float tiempoAntesDeRayar = 1.5f;
    public float velocidadAcercamiento = 3f;
    public float distanciaRayado = 1.5f;
    public GameObject grafitiPrefab;
    public int dañoALimpieza = 15;

    [Header("Efectos futuros")]
    public GameObject efectoRayadoPrefab;

    private Transform ascensor;
    private AscensorController ascensorController; 
    private Vector3 posicionObjetivo;
    private bool yaRayo = false;
    private bool yaTermino = false; 
    private float timer = 0f;

    public Action<Maleante> OnMaleanteTerminado;

    private void Start()
    {
        // revisar actualizaciones
        ascensorController = FindObjectOfType<AscensorController>();

        if (ascensorController != null)
        {
            ascensor = ascensorController.transform;
        }
        else
        {
            Debug.LogError("El Maleante encontró el AscensorController");
            Desaparecer();
            return;
        }

        Vector3 direccion = UnityEngine.Random.value > 0.5f ? Vector3.left : Vector3.right;
        posicionObjetivo = ascensor.position + direccion * distanciaRayado + Vector3.up * 0.3f;

        Debug.Log($"Maleante apareció.");
    }

    private void Update()
    {
        if (ascensor == null || yaRayo || yaTermino) return;

     
        if (ascensorController != null && ascensorController.enMovimiento)
        {
            Debug.Log($" El maleante huye.");
            Desaparecer();
            return;
        }

       
        transform.position = Vector3.MoveTowards(
            transform.position,
            posicionObjetivo,
            velocidadAcercamiento * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, posicionObjetivo) < 0.05f)
        {
            timer += Time.deltaTime;

            if (timer >= tiempoAntesDeRayar)
            {
                RayarAscensor();
            }
        }
    }

    private void RayarAscensor()
    {
        if (yaTermino) return;
        yaRayo = true;

        if (grafitiPrefab != null && ascensor != null)
        {
            GameObject grafiti = Instantiate(grafitiPrefab, ascensor);

            float offsetX = UnityEngine.Random.Range(-0.5f, 0.5f);
            float offsetY = UnityEngine.Random.Range(-0.3f, 0.3f);
            grafiti.transform.localPosition = new Vector3(offsetX, offsetY, 0);
            grafiti.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-15f, 15f));
            grafiti.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.8f, 1.2f);

            Debug.Log($" Maleante rayó el ascensor");
        }

        if (efectoRayadoPrefab != null)
            Instantiate(efectoRayadoPrefab, transform.position, Quaternion.identity);

        if (GameManager.Instance != null)
            GameManager.Instance.ModificarLimpieza(-dañoALimpieza);

        Desaparecer();
    }

 
    private void Desaparecer()
    {
        if (yaTermino) return;
        yaTermino = true;

        OnMaleanteTerminado?.Invoke(this);
        Destroy(gameObject);
    }
}