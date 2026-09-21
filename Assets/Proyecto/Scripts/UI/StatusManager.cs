using UnityEngine;

public class StatusManager : MonoBehaviour
{
 
    public float intervaloLimpeza = 10f;
    public float intervaloPolea = 15f;

  
    public float decrementoLimpieza = 5f;
    public float decrementoPolea = 5f;
    public float decrementoPuerta = 10f;


    private float timerLimpieza = 0f;
    private float timerPolea = 0f;


    private AscensorController ascensor;
    private bool estadoAnteriorMovimiento = false;

    private void Start()
    {
      
        ascensor = FindObjectOfType<AscensorController>();
        if (ascensor != null)
        {
            estadoAnteriorMovimiento = ascensor.enMovimiento;
        }
    }

    private void Update()
    {

        if (GameManager.Instance.gameData.eventoOcurriendo) return;


        timerLimpieza += Time.deltaTime;
        if (timerLimpieza >= intervaloLimpeza)
        {
            GameManager.Instance.ModificarLimpieza(-decrementoLimpieza);
            timerLimpieza = 0f;
        }

   
        timerPolea += Time.deltaTime;
        if (timerPolea >= intervaloPolea)
        {
            GameManager.Instance.ModificarPolea(-decrementoPolea);
            timerPolea = 0f;
        }

  
        DetectarCambioEstadoAscensor();
    }

    private void DetectarCambioEstadoAscensor()
    {
        if (ascensor == null) return;

        bool estadoActual = ascensor.enMovimiento;

        if (estadoActual != estadoAnteriorMovimiento)
        {
           
            if (!estadoActual || estadoActual) 
            {
                GameManager.Instance.ModificarPuerta(-decrementoPuerta);
                Debug.Log($" Descuento de puerta por cambio de estado: -{decrementoPuerta}");
            }

            estadoAnteriorMovimiento = estadoActual;
        }
    }
}