using UnityEngine;

[System.Serializable]
public class EventoConfig
{
    
    public string nombre;
    public GameObject prefab;

    [Range(0f, 1f)]
    public float peso = 1f;

    [Tooltip("El evento solo se lanza si el ascensor está detenido")]
    public bool requiereAscensorDetenido = true;

    [Tooltip("El evento solo se lanza si hay algún ascensor cerca (arriba)")]
    public bool requiereAscensorCerca = false;

    [Tooltip("El evento solo se lanza si hay algún ascensor lejos (abajo)")]
    public bool requiereAscensorLejos = false;
}