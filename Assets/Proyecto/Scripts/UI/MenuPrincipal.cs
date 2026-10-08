using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    
    public GameObject contenidoPrincipal;
    public GameObject panelOpciones;
    public GameObject panelConfirmacionNuevaPartida; 

   
    public Button botonContinuar;
    public Button botonNuevaPartida;

    public string nombreEscenaDia = "EscenaDia";
    public string clavePartidaGuardada = "PartidaIniciada";

    private void Start()
    {
        
        if (panelConfirmacionNuevaPartida != null)
            panelConfirmacionNuevaPartida.SetActive(false);

       
        ActualizarBotones();
    }

    private void ActualizarBotones()
    {
        
        bool hayPartidaGuardada = PlayerPrefs.HasKey(clavePartidaGuardada);

      
        if (botonContinuar != null)
            botonContinuar.interactable = hayPartidaGuardada;

    }

    public void NuevaPartida()
    {
        Debug.Log("Iniciando nueva partida...");

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs borrados");

       
        PlayerPrefs.SetInt(clavePartidaGuardada, 1);
        PlayerPrefs.Save();

    
        if (GameManager.Instance != null && GameManager.Instance.gameData != null)
        {
            GameManager.Instance.gameData.ResetData();
            Debug.Log("GameData reseteado");
        }

        SceneManager.LoadScene(nombreEscenaDia);
    }

    public void ContinuarPartida()
    {
       

        if (!PlayerPrefs.HasKey(clavePartidaGuardada))
        {
            Debug.LogWarning("No hay partida guardada. Iniciando nueva.");
            NuevaPartida();
            return;
        }

        SceneManager.LoadScene(nombreEscenaDia);
    }

  
    public void MostrarConfirmacionNuevaPartida()
    {
        if (panelConfirmacionNuevaPartida != null)
            panelConfirmacionNuevaPartida.SetActive(true);
    }

    public void CancelarNuevaPartida()
    {
        if (panelConfirmacionNuevaPartida != null)
            panelConfirmacionNuevaPartida.SetActive(false);
    }

    public void ConfirmarNuevaPartida()
    {
        if (panelConfirmacionNuevaPartida != null)
            panelConfirmacionNuevaPartida.SetActive(false);

        NuevaPartida();
    }

    public void AbrirOpciones()
    {
        if (contenidoPrincipal != null) contenidoPrincipal.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        if (panelOpciones != null) panelOpciones.SetActive(false);
        if (contenidoPrincipal != null) contenidoPrincipal.SetActive(true);

        ActualizarBotones();
    }


    public void Salir()
    {
        Debug.Log("Cerrando el juego...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}