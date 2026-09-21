using UnityEngine;
using TMPro;


public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance { get; private set; }

    
    public GameObject floatingTextPrefab;

    
    public float duracionTexto = 1f;       
    public float velocidadSubida = 1.5f;  
    public float distanciaAleatoria = 0.3f; 

   
    public Color colorPositivo = Color.green;
    public Color colorNegativo = Color.red;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

   
    public void MostrarTexto(float cantidad, TipoSistema tipo)
    {
        
        if (Mathf.Abs(cantidad) < 0.1f) return;

        Transform target = GameManager.Instance.ObtenerTarget(tipo);

        if (target == null)
        {
            Debug.LogWarning($" No hay target asignado para {tipo}");
            return;
        }

        Vector3 offsetAleatorio = new Vector3(
            Random.Range(-distanciaAleatoria, distanciaAleatoria),
            0,
            0
        );

        GameObject nuevoTexto = Instantiate(
            floatingTextPrefab,
            target.position + offsetAleatorio,
            Quaternion.identity
        );

        FloatingText ft = nuevoTexto.GetComponent<FloatingText>();
        if (ft != null)
        {
            ft.Configurar(cantidad, duracionTexto, velocidadSubida, colorPositivo, colorNegativo);
        }
    }
}