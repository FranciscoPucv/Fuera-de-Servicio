using UnityEngine;

public class OpcionesManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelOpciones;

    private bool menuAbierto = false;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuAbierto)
            {
                CerrarOpciones();
            }
            else
            {
                AbrirOpciones();
            }
        }
    }

    public void AbrirOpciones()
    {
        panelOpciones.SetActive(true);
        menuAbierto = true;
        Time.timeScale = 0f;
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
        menuAbierto = false;
        Time.timeScale = 1f;
    }
}
