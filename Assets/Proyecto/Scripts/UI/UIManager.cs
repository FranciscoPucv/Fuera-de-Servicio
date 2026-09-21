using UnityEngine;
using TMPro; 

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI textoLimpieza;
    public TextMeshProUGUI textoPolea;
    public TextMeshProUGUI textoPuerta;
    public TextMeshProUGUI textoPasajeros;
    public TextMeshProUGUI textoEstadoAscensor;
    public TextMeshProUGUI textoDinero;

    private GameData gameData;

    private void Start()
    {
        gameData = GameManager.Instance.gameData;

      
        gameData.OnDataChanged += ActualizarUI;

       
        ActualizarUI();
    }

    private void OnDestroy()
    {
        
        if (gameData != null)
            gameData.OnDataChanged -= ActualizarUI;
    }

    private void ActualizarUI()
    {
        if (textoLimpieza != null)
            textoLimpieza.text = $" Limpieza: {gameData.contadorLimpieza:F0}%";

        if (textoPolea != null)
            textoPolea.text = $"Mantenimiento: {gameData.contadorPolea:F0}%";

        if (textoPuerta != null)
            textoPuerta.text = $" Puerta: {gameData.contadorPuerta:F0}%";

        if (textoPasajeros != null)
           textoPasajeros.text = $" Pasajeros: {gameData.pasajerosTotales}/10";

        if (textoDinero != null)
            textoDinero.text = $" Dinero: {gameData.dineroTotal:F0}";

        if (textoEstadoAscensor != null)
        {
            string estado = gameData.ascensorEnMovimiento ? " EN MOVIMIENTO" : " DETENIDO";
            textoEstadoAscensor.text = estado;
        }
    }
}