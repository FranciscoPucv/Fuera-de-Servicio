using UnityEngine;
using UnityEngine.SceneManagement;

public class DayNightController : MonoBehaviour
{
  
    public string nombreEscenaDia = "Escena_Dia";    
    public string nombreEscenaNoche = "Escena_Noche"; 

    public ClockManager clockManager;
    public bool enEscenaNoche = false;

    private void Start()
    {
        
        string escenaActual = SceneManager.GetActiveScene().name;
        enEscenaNoche = (escenaActual == nombreEscenaNoche);

        if (enEscenaNoche)
        {
           
            if (clockManager != null)
                clockManager.relojActivo = false;
        }
        else
        {
            
            if (clockManager != null)
                clockManager.OnDiaTerminado += IrAEscenaNoche;
        }
    }

    private void OnDestroy()
    {
        if (clockManager != null)
            clockManager.OnDiaTerminado -= IrAEscenaNoche;
    }

    private void IrAEscenaNoche()
    {
        
        if (clockManager != null)
            clockManager.ReiniciarDia();

        SceneManager.LoadScene(nombreEscenaNoche);
    }

   
    public void IrAEscenaDia()
    {
        SceneManager.LoadScene(nombreEscenaDia);
    }
}