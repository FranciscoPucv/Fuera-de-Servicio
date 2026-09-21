using UnityEngine;
using System;

public class PaymentManager : MonoBehaviour
{
    public int costoPago = 100;             
    public int horasParaPagar = 3;          

    public GameObject panelPago;          
    public GameObject panelDerrota;          

    public bool esperandoPago = false;
    public int horasRestantesParaPagar = 0;

    // Eventos
    public event Action OnPagoExitoso;
    public event Action OnDerrota;

    private ClockManager clockManager;

    private void Start()
    {
        clockManager = FindObjectOfType<ClockManager>();

        if (clockManager != null)
        {
            clockManager.OnCobroRequerido += IniciarCobro;
            clockManager.OnHoraCambio += AlAvanzarHora;
        }

        if (panelPago != null) panelPago.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);
    }

    private void OnDestroy()
    {
        if (clockManager != null)
        {
            clockManager.OnCobroRequerido -= IniciarCobro;
            clockManager.OnHoraCambio -= AlAvanzarHora;
        }
    }

    private void IniciarCobro()
    {
        esperandoPago = true;
        horasRestantesParaPagar = horasParaPagar;

        if (panelPago != null) panelPago.SetActive(true);

        Debug.Log($"¡Debes pagar {costoPago} dinero. Tienes {horasRestantesParaPagar} horas.");
    }

    private void AlAvanzarHora(int hora)
    {
        if (!esperandoPago) return;

        horasRestantesParaPagar--;
        Debug.Log($" Tiempo para pagar: {horasRestantesParaPagar} horas");

   
        if (horasRestantesParaPagar <= 0)
        {
            Debug.Log("Se acabó el tiempo para pagar");
            Derrota();
        }
    }

    public void IntentarPagar()
    {
        if (!esperandoPago) return;

        int dinero = GameManager.Instance.GameData.dineroTotal;

        if (dinero >= costoPago)
        {
            GameManager.Instance.GameData.dineroTotal -= costoPago;
            GameManager.Instance.GameData.OnDataChanged?.Invoke();

            Debug.Log($"Pagaste {costoPago}. Dinero restante: {GameManager.Instance.GameData.dineroTotal}");

            esperandoPago = false;
            if (panelPago != null) panelPago.SetActive(false);

            OnPagoExitoso?.Invoke();
        }
        else
        {
            Debug.Log($" No tienes suficiente dinero ({dinero}/{costoPago})");
            Derrota();
        }
    }
    private void Derrota()
    {
        esperandoPago = false;
        if (panelPago != null) panelPago.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log(" GAME OVER");
        OnDerrota?.Invoke();
    }

    public int GetDineroActual() => GameManager.Instance.GameData.dineroTotal;
}