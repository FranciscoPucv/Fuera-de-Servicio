using UnityEngine;
using System;

public class ClockManager : MonoBehaviour
{
    
    public float segundosPorHora = 10f;
    public int horaInicio = 8;
    public int horaFin = 17;

    public int horaActual = 8;
    public bool relojActivo = true;

  
    public event Action<int> OnHoraCambio;         
    public event Action OnDiaTerminado;            

    private float timer = 0f;

    private void Start()
    {
        
        if (GameManager.Instance != null)
        {
            horaActual = GameManager.Instance.GameData.horaActual;
        }
        else
        {
            horaActual = horaInicio;
        }

        Debug.Log($"Reloj iniciado a las {horaActual}:00");
    }

    private void Update()
    {
        if (!relojActivo) return;
        if (GameManager.Instance != null && GameManager.Instance.GameData.eventoOcurriendo) return;
        if (GameManager.Instance != null && GameManager.Instance.GameData.ascensorEnMovimiento) return;
        if (Time.timeScale == 0f) return;

        timer += Time.deltaTime;

        if (timer >= segundosPorHora)
        {
            timer = 0f;
            AvanzarHora();
        }
    }

    private void AvanzarHora()
    {
        horaActual++;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameData.horaActual = horaActual;
        }

        OnHoraCambio?.Invoke(horaActual);
        Debug.Log($"Hora actual: {horaActual}:00");

        if (horaActual >= horaFin)
        {
            TerminarDia();
        }
    }


    private void TerminarDia()
    {
        relojActivo = false;

     
        if (GameManager.Instance != null)
        {
            var data = GameManager.Instance.gameData;
            data.resumenDia.TerminarDia(data.dineroTotal, data.pasajerosTotales);
            data.resumenNocheMostrado = false;
        }

        OnDiaTerminado?.Invoke();
    }

    public int GetHoraActual() => horaActual;

    public void ReiniciarDia()
    {
        horaActual = horaInicio;
        timer = 0f;
        relojActivo = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameData.horaActual = horaActual;
        }

        Debug.Log($"Nuevo día comenzado a las {horaActual}:00");
    }
}