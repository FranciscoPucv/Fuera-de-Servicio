using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Paneles de la Interfaz")]
    public GameObject contenidoPrincipal; // Contenedor con Titulo, Jugar, Salir, etc.
    public GameObject panelOpciones;       // Panel transparente de ajustes

    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene");
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