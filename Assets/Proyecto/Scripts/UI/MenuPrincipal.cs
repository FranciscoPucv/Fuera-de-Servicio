using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("SampleScene");
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