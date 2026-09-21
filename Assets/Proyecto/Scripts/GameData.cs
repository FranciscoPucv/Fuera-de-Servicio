using UnityEngine;

[CreateAssetMenu(fileName = "Datos", menuName = "GameData")]

public class GameData : ScriptableObject
{
    public int pasajerosTotales = 0;
    public int dineroTotal = 0;
    public int capacidadMaxima = 10;

    [Range(0, 100)] public float contadorPopularidad = 50f;
    [Range(0, 100)] public float contadorLimpieza = 100f;
    [Range(0, 100)] public float contadorPolea = 100f;
    [Range(0, 100)] public float contadorPuerta = 100f;

   
    public float spawnBase = 2f;
    public int pagoPorPasajero = 10;
    [Range(0f, 1f)] public float probabilidadPago = 0.5f;

   
    public int horaActual = 8;
    public ClimaEnum climaActual = ClimaEnum.Soleado;


    public bool eventoOcurriendo = false;
    public bool ascensorEnMovimiento = false;

   
    public bool HayEspacio => pasajerosTotales < capacidadMaxima;
    public bool EstaVacio => pasajerosTotales == 0;

   
    public System.Action OnDataChanged;

    [System.NonSerialized] private bool valoresInicialesCapturados = false;
    [System.NonSerialized] private int _pasajerosTotalesInicial;
    [System.NonSerialized] private int _dineroTotalInicial;
    [System.NonSerialized] private float _popularidadInicial;
    [System.NonSerialized] private float _limpiezaInicial;
    [System.NonSerialized] private float _poleaInicial;
    [System.NonSerialized] private float _puertaInicial;
    [System.NonSerialized] private int _horaInicial;
    [System.NonSerialized] private ClimaEnum _climaInicial;

  
    public void CapturarValoresIniciales()
    {
        _pasajerosTotalesInicial = pasajerosTotales;
        _dineroTotalInicial = dineroTotal;
        _popularidadInicial = contadorPopularidad;
        _limpiezaInicial = contadorLimpieza;
        _poleaInicial = contadorPolea;
        _puertaInicial = contadorPuerta;
        _horaInicial = horaActual;
        _climaInicial = climaActual;

        valoresInicialesCapturados = true;

        Debug.Log("Valores iniciales capturados");
    }

    public void ResetData()
    {
        if (!valoresInicialesCapturados)
            CapturarValoresIniciales();

        pasajerosTotales = _pasajerosTotalesInicial;
        dineroTotal = _dineroTotalInicial;
        contadorPopularidad = _popularidadInicial;
        contadorLimpieza = _limpiezaInicial;
        contadorPolea = _poleaInicial;
        contadorPuerta = _puertaInicial;
        horaActual = _horaInicial;
        climaActual = _climaInicial;

        eventoOcurriendo = false;
        ascensorEnMovimiento = false;

        OnDataChanged?.Invoke();

        Debug.Log("GameData reseteado a valores iniciales");
    }
}

public enum ClimaEnum
{
    Soleado,
    Lluvioso,
    Ventoso,
    Neblinoso
}