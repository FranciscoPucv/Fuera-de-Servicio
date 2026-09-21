using UnityEngine;
using System;

public class ClockManager : MonoBehaviour
{
    public float segundosPorHora = 10f;

    public int horasPorCobro = 10;

    public int horaActual = 8;        
    public int diasTranscurridos = 0;
    public int horasParaProximoCobro = 10;

    // Eventos
    public event Action<int> OnHoraCambio;         
    public event Action OnCobroRequerido;          

    private float timer = 0f;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            horaActual = GameManager.Instance.GameData.horaActual;
        }

        horasParaProximoCobro = horasPorCobro;

        Debug.Log($"Reloj iniciado a las {horaActual}:00");
    }

    private void Update()
    {
        if (GameManager.Instance.GameData.eventoOcurriendo) return;

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
        horasParaProximoCobro--;

        if (horaActual >= 24)
        {
            horaActual = 0;
            diasTranscurridos++;
            Debug.Log($"Nuevo día: {diasTranscurridos}");
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameData.horaActual = horaActual;
        }

        OnHoraCambio?.Invoke(horaActual);
        Debug.Log($"Hora: {horaActual}:00 | Próximo cobro en: {horasParaProximoCobro} horas");

        if (horasParaProximoCobro <= 0)
        {
            horasParaProximoCobro = horasPorCobro;
            DispararCobro();
        }
    }

    private void DispararCobro()
    {
        Debug.Log("¡COBRO!");
        OnCobroRequerido?.Invoke();
    }

    public int GetHorasParaCobro() => horasParaProximoCobro;
}