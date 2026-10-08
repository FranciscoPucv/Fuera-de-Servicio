using System;

[Serializable]
public class ResumenDiaData
{
    
    public int dineroInicioDia;
    public int pasajerosInicioDia;
    public float limpiezaInicioDia;
    public float poleaInicioDia;
    public float puertaInicioDia;

    
    public int eventosOcurridos;
    public int basuraTirada;
    public int grafitisHechos;
    public int pasajerosTotalesTransportados;  

    
    public int dineroFinDia;
    public int pasajerosFinDia;

    public int DineroGanado => dineroFinDia - dineroInicioDia;
    public int PasajerosTransportados => pasajerosTotalesTransportados;

    
    public void IniciarDia(int dineroActual, int pasajerosActuales,
                            float limpieza, float polea, float puerta)
    {
        dineroInicioDia = dineroActual;
        pasajerosInicioDia = pasajerosActuales;
        limpiezaInicioDia = limpieza;
        poleaInicioDia = polea;
        puertaInicioDia = puerta;

        eventosOcurridos = 0;
        basuraTirada = 0;
        grafitisHechos = 0;
        pasajerosTotalesTransportados = 0;

        dineroFinDia = dineroActual;
        pasajerosFinDia = pasajerosActuales;

        UnityEngine.Debug.Log($"  Dinero inicial: ${dineroInicioDia}");
    }

    public void TerminarDia(int dineroActual, int pasajerosActuales)
    {
        dineroFinDia = dineroActual;
        pasajerosFinDia = pasajerosActuales;

        UnityEngine.Debug.Log($" Dinero final: ${dineroFinDia}. Ganancia: ${DineroGanado}");
    }

 
    public void RegistrarEvento() { eventosOcurridos++; }
    public void RegistrarBasura() { basuraTirada++; }
    public void RegistrarGrafiti() { grafitisHechos++; }
    public void RegistrarPasajeroTransportado() { pasajerosTotalesTransportados++; }
}