using UnityEngine;

public class IntroTrigger : MonoBehaviour
{
   
    public string idDialogo = "introduccion";
    public float retrasoAntesDeEmpezar = 0.5f;

    private void Start()
    {
        if (GameManager.Instance == null) return;

        if (PlayerPrefs.GetInt("IntroVista", 0) == 1) return;

        PlayerPrefs.SetInt("IntroVista", 1);
        PlayerPrefs.Save();

        Invoke(nameof(EmpezarIntro), retrasoAntesDeEmpezar);
    }

    private void EmpezarIntro()
    {
        if (DialogoManager.Instance != null)
            DialogoManager.Instance.MostrarDialogo(idDialogo);
    }
}