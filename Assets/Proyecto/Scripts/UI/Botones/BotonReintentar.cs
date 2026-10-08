using UnityEngine;
using UnityEngine.SceneManagement;



public class BotonReintentar : MonoBehaviour
{
    public void Reintentar()
    {
        Debug.Log(" Reiniciando juego...");

   
        Time.timeScale = 1f;

      
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetearJuego();
        }
       

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}