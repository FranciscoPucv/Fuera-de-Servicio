using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameData data; 

    private void Awake() { Instance = this; DontDestroyOnLoad(gameObject); }

    public void ModificarLimpieza(float cantidad) {}
    public void SumarPasajero(int cantidad) {  }
}
