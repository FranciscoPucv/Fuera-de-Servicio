using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PausaManager : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject panelOpcionesPausa;

    [Header("Input del Juego")]
    [Tooltip("Arrastra aqui el mismo Input Actions Asset que usa la Palanca")]
    public InputActionAsset inputActionsAsset;

    public static bool juegoPausado = false;

    void Update()
    {

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        if (panelOpcionesPausa != null) panelOpcionesPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;

        // Desactiva el mapa de controles del ascensor para que no se mueva con el menu abierto
        if (inputActionsAsset != null)
        {
            inputActionsAsset.FindActionMap("Ascensor")?.Disable();
        }
    }

    public void Reanudar()
    {
        if (panelOpcionesPausa != null) panelOpcionesPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;

        // Reactiva los controles de la palanca al volver a jugar
        if (inputActionsAsset != null)
        {
            inputActionsAsset.FindActionMap("Ascensor")?.Enable();
        }
    }

    public void VolverAlMenuPrincipal()
    {
        Time.timeScale = 1f;
        juegoPausado = false;
        SceneManager.LoadScene("Menu inicio");
    }
}