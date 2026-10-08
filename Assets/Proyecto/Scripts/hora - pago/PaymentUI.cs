using UnityEngine;
using TMPro;

public class PaymentUI : MonoBehaviour
{
    
    public TextMeshProUGUI textoCosto;
    public TextMeshProUGUI textoDinero;
    public PaymentManager paymentManager;
    private void Start()
    {
        Debug.Log($"PaymentUI iniciado");
        
    }
    private void OnEnable()
    {
        ActualizarTextos();
        if (GameManager.Instance != null)
            GameManager.Instance.gameData.OnDataChanged += ActualizarTextos;
    }

    private void OnDisable()
    {
        
        if (GameManager.Instance != null && GameManager.Instance.GameData != null)
            GameManager.Instance.gameData.OnDataChanged -= ActualizarTextos;
    }

    private void Update()
    {
        
        ActualizarTextos();
    }

    private void ActualizarTextos()
    {
        if (paymentManager == null) return;
        if (GameManager.Instance == null) return;

        int costo = paymentManager.costoPago;
        int dinero = GameManager.Instance.gameData.dineroTotal;

        if (textoCosto != null)
        {
            textoCosto.text = $"¡Debes pagar ${costo}!";
        }

        if (textoDinero != null)
        {
            textoDinero.text = $"Dinero actual: ${dinero}";

            //por ahora solo en colores
            textoDinero.color = dinero >= costo ? Color.green : Color.red;
        }
    }
}