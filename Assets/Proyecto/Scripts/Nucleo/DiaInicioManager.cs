using UnityEngine;

public class DiaInicioManager : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(IniciarResumen), 0.1f); 
    }

    private void IniciarResumen()
    {
        if (GameManager.Instance == null) return;

        var data = GameManager.Instance.gameData;
        data.resumenDia.IniciarDia(
            data.dineroTotal,
            data.pasajerosTotales,
            data.contadorLimpieza,
            data.contadorPolea,
            data.contadorPuerta
        );

        Debug.Log("Resumen del día iniciado");
    }
}