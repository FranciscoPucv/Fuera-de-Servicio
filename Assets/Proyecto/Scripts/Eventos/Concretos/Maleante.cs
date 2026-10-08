using UnityEngine;
using System;

public class Maleante : MonoBehaviour
{
    //se me ocurrio nuevo objeto, una alarma contra los maleantes, si se pilla justo cunado haran un grafiti estos huyen...
    public float tiempoAntesDeRayar = 1.5f;
    public float velocidadAcercamiento = 3f;
    public float distanciaRayado = 1.5f;
    public GameObject grafitiPrefab;
    public int dañoALimpieza = 15;

   
    public GameObject efectoRayadoPrefab;

   
    private Transform ascensorObjetivo;     
    private AscensorManager ascensorManager;
    private int ascensorIndex = 0;         

    private Vector3 posicionObjetivo;
    private bool yaRayo = false;
    private bool yaTermino = false;
    private float timer = 0f;

    public Action<Maleante> OnMaleanteTerminado;

    private void Start()
    {
        ascensorManager = FindObjectOfType<AscensorManager>();

        if (ascensorManager == null)
        {
          
            Desaparecer();
            return;
        }

       
        ascensorIndex = ElegirAscensorObjetivo();
        ascensorObjetivo = ascensorManager.ascensores[ascensorIndex].ascensorTransform;

        if (ascensorObjetivo == null)
        {
            
            Desaparecer();
            return;
        }

       
        Vector3 direccion = UnityEngine.Random.value > 0.5f ? Vector3.left : Vector3.right;
        posicionObjetivo = ascensorObjetivo.position + direccion * distanciaRayado + Vector3.up * 0.3f;

        Debug.Log($"Maleante apareció y atacará al ascensor {ascensorIndex}");
    }


    private int ElegirAscensorObjetivo()
    {
        

        for (int i = 0; i < ascensorManager.ascensores.Count; i++)
        {
            var asc = ascensorManager.ascensores[i];
            
            if (!asc.estaAbajo && asc.ascensorTransform != null)
            {
                return i;
            }
        }

        return 0;
    }

    private void Update()
    {
        if (ascensorObjetivo == null || yaRayo || yaTermino) return;

        if (ascensorManager != null &&
            ascensorManager.ascensores[ascensorIndex].enMovimiento)
        {
            
            Debug.Log("El ascensor se movió. El maleante huye.");
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

        if (grafitiPrefab != null && ascensorObjetivo != null)
        {
       
            GameObject grafiti = Instantiate(grafitiPrefab, ascensorObjetivo);

            float offsetX = UnityEngine.Random.Range(-0.5f, 0.5f);
            float offsetY = UnityEngine.Random.Range(-0.3f, 0.3f);
            grafiti.transform.localPosition = new Vector3(offsetX, offsetY, 0);
            grafiti.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-15f, 15f));
            grafiti.transform.localScale = Vector3.one * UnityEngine.Random.Range(0.8f, 1.2f);

       
        }

        if (efectoRayadoPrefab != null)
            Instantiate(efectoRayadoPrefab, transform.position, Quaternion.identity);

        if (GameManager.Instance != null)
            GameManager.Instance.ModificarLimpieza(-dañoALimpieza);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.gameData.resumenDia.RegistrarGrafiti();
        }
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