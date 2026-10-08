using UnityEngine;
using System;

public enum EstadoJuego
{
    Jugando,        
    EnDialogo,      
    EnPausa,        
    EnMenu,         
    GameOver        
}

public class EstadoJuegoManager : MonoBehaviour
{
    public static EstadoJuegoManager Instance { get; private set; }

    public EstadoJuego estadoActual = EstadoJuego.Jugando;

    public event Action<EstadoJuego, EstadoJuego> OnEstadoCambio; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

  
    public void CambiarEstado(EstadoJuego nuevoEstado)
    {
        if (estadoActual == nuevoEstado) return;

        EstadoJuego anterior = estadoActual;
        estadoActual = nuevoEstado;

       
        Time.timeScale = (nuevoEstado == EstadoJuego.Jugando) ? 1f : 0f;

        Debug.Log($"Estado cambiado: {anterior} → {nuevoEstado}");
        OnEstadoCambio?.Invoke(anterior, nuevoEstado);
    }

    
    public bool EstaJugando() => estadoActual == EstadoJuego.Jugando;
    public bool EstaEnDialogo() => estadoActual == EstadoJuego.EnDialogo;
    public bool EstaPausado() => estadoActual != EstadoJuego.Jugando;
}