using UnityEngine;
using TMPro;

public class PaymentUI : MonoBehaviour
{
    public TextMeshProUGUI textoCosto;
    public TextMeshProUGUI textoDinero;
    public PaymentManager paymentManager;

    private void OnEnable()
    {
        ActualizarTextos();
    }

    private void Update()
    {
        ActualizarTextos();
    }

    private void ActualizarTextos()
    {
        if (paymentManager == null) return;

        int costo = paymentManager.costoPago;
        int dinero = paymentManager.GetDineroActual();
        int horasRestantes = paymentManager.horasRestantesParaPagar;

        if (textoCosto != null)
            textoCosto.text = $"¡Debes pagar ${costo}!\nTiempo restante: {horasRestantes}h";

        if (textoDinero != null)
        {
            textoDinero.text = $"Dinero actual: ${dinero}";
            textoDinero.color = dinero >= costo ? Color.green : Color.red;
        }
    }
}