using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    [SerializeField] public GameData gameData;   
    public GameData GameData => gameData;



    public Transform targetLimpieza;
    public Transform targetPolea;
    public Transform targetPuerta;
    public Transform targetDinero;





    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            if (gameData != null)
                gameData.CapturarValoresIniciales();

            Debug.Log($"GameManager registrado y persistente. Escena: {gameObject.scene.name}");
        }
        else if (Instance != this)
        {
            Debug.LogWarning($"GameManager duplicado en {gameObject.name}. DESTRUYENDO.");

            
            Destroy(gameObject);
        }
    }
    public void ResetearJuego()
    {
        if (gameData != null)
            gameData.ResetData();
    }

    public Transform ObtenerTarget(TipoSistema tipo)
    {
        return tipo switch
        {
            TipoSistema.Limpieza => targetLimpieza,
            TipoSistema.Polea => targetPolea,
            TipoSistema.Puerta => targetPuerta,
            TipoSistema.Dinero => targetDinero,
            _ => null
        };
    }

    public void ModificarLimpieza(float cantidad)
    {
        gameData.contadorLimpieza = Mathf.Clamp(gameData.contadorLimpieza + cantidad, 0, 100);
        FloatingTextManager.Instance.MostrarTexto(cantidad, TipoSistema.Limpieza);
        gameData.OnDataChanged?.Invoke();
    }

    public void ModificarPolea(float cantidad)
    {
        gameData.contadorPolea = Mathf.Clamp(gameData.contadorPolea + cantidad, 0, 100);
        FloatingTextManager.Instance.MostrarTexto(cantidad, TipoSistema.Polea);
        gameData.OnDataChanged?.Invoke();
    }

    public void ModificarPuerta(float cantidad)
    {
        gameData.contadorPuerta = Mathf.Clamp(gameData.contadorPuerta + cantidad, 0, 100);
        FloatingTextManager.Instance.MostrarTexto(cantidad, TipoSistema.Puerta);
        gameData.OnDataChanged?.Invoke();
    }

    
    public bool IntentarSubirPasajero()
    {
        if (!gameData.HayEspacio)
        {
            Debug.Log(" Ascensor lleno");
            return false;
        }

        gameData.pasajerosTotales++;
        gameData.OnDataChanged?.Invoke();
        Debug.Log($" Pasajeros: {gameData.pasajerosTotales}/{gameData.capacidadMaxima}");
        return true;
    }

    
    public void SumarDinero(int cantidad)
    {
        gameData.dineroTotal += cantidad;
        FloatingTextManager.Instance.MostrarTexto(cantidad, TipoSistema.Dinero);
        gameData.OnDataChanged?.Invoke();
        Debug.Log($" Dinero total: {gameData.dineroTotal}");
    }


    public void BajarTodosLosPasajeros()
    {
        int cantidad = gameData.pasajerosTotales;
        gameData.pasajerosTotales = 0;
        gameData.OnDataChanged?.Invoke();
        Debug.Log($" Bajaron {cantidad} pasajeros");
    }
  
    public void PasajeroCompleto()
    {
        gameData.pasajerosTotales = Mathf.Max(0, gameData.pasajerosTotales - 1);
        gameData.dineroTotal += gameData.pagoPorPasajero;
        gameData.OnDataChanged?.Invoke();

        Debug.Log($" +{gameData.pagoPorPasajero} dinero. Pasajeros: {gameData.pasajerosTotales}");
    }


    public void PasajeroSubio()
    {
        if (!gameData.HayEspacio)
        {
            Debug.Log(" Ascensor lleno, no puede subir más pasajeros");
            return;
        }

        gameData.pasajerosTotales++;
        gameData.OnDataChanged?.Invoke();
        Debug.Log($" Pasajero subió. Total: {gameData.pasajerosTotales}");
    }
    private void OnDestroy()
    {
        Debug.Log($"+ [{Time.time:F2}s] GameManager.OnDestroy() en escena: '{gameObject.scene.name}'");
        Debug.Log($"+Este GameObject: {gameObject.name}");
        Debug.Log($"+ Instance actual: {Instance}");
        Debug.Log($"+ ¿Soy el Instance?: {Instance == this}");

        if (Instance == this)
        {
            Instance = null;
        }

        // 🔑 Imprimir el stack trace para saber QUIÉN lo destruyó
        Debug.Log($"🔴 Stack: {System.Environment.StackTrace}");
    }

}

public enum TipoSistema
{
    Limpieza,
    Polea,
    Puerta,
    Dinero
}