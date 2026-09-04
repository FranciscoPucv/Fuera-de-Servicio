using UnityEngine;

public class GameData : ScriptableObject
{

    int pasajerosTotales;
    float contadorLimpieza;
    float contadorPolea;
    float contadorPuerta;
    int horaActual;
    enum ClimaActual { Soleado, Lluvioso, Ventoso, Neblinoso }

    bool eventoOcurriendo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
