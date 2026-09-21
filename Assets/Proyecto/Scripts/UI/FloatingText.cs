using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;

    private float cantidad;
    private float duracion;
    private float velocidadSubida;
    private Color colorPositivo;
    private Color colorNegativo;
    private float tiempoTranscurrido = 0f;

    private bool inicializado = false;

    private void Awake()
    {
       
        if (textMesh == null)
        {
            textMesh = GetComponent<TextMeshPro>();
        }

     
        if (textMesh == null)
        {
            textMesh = GetComponentInChildren<TextMeshPro>();
        }

        if (textMesh == null)
        {
            Debug.LogError(" DONDE ESTA EL CONDENADO TextMeshPro aaaaaaahhhhh!!!!");
        }
    }

    public void Configurar(float cantidad, float duracion, float velocidad, Color colorPos, Color colorNeg)
    {
       
        if (textMesh == null)
        {
            Debug.LogError("  textMesh es null");
            Destroy(gameObject);
            return;
        }

        this.cantidad = cantidad;
        this.duracion = duracion;
        this.velocidadSubida = velocidad;
        this.colorPositivo = colorPos;
        this.colorNegativo = colorNeg;

    
        string signo = cantidad > 0 ? "+" : "";
        textMesh.text = $"{signo}{cantidad:F1}";

   
        textMesh.color = cantidad > 0 ? colorPositivo : colorNegativo;

        inicializado = true;
    }

    private void Update()
    {
    
        if (!inicializado) return;

        if (textMesh == null || duracion <= 0f) return;

        tiempoTranscurrido += Time.deltaTime;

        transform.position += Vector3.up * velocidadSubida * Time.deltaTime;

  
        float alpha = Mathf.Lerp(1f, 0f, tiempoTranscurrido / duracion);
        Color c = textMesh.color;
        c.a = alpha;
        textMesh.color = c;

   
        if (tiempoTranscurrido >= duracion)
        {
            Destroy(gameObject);
        }
    }
}