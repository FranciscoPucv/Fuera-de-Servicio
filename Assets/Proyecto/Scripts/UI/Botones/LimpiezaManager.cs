using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LimpiezaManager : MonoBehaviour
{
    
    public int costoLimpiar = 20;      
    public int limpiezaRecuperada = 20; 
    public string tagGrafiti = "Grafiti"; 

    
    public System.Action OnEstadoCambio;

  
    private Transform ascensorTransform;

    private void Start()
    {
        AscensorController asc = FindObjectOfType<AscensorController>();
        if (asc != null)
            ascensorTransform = asc.transform;
        else
            Debug.LogError("LimpiezaManager: no se encontró el AscensorController");
    }

  
    public bool SePuedeLimpiar()
    {
        if (GameManager.Instance == null) return false;

     
        if (GameManager.Instance.GameData.dineroTotal < costoLimpiar) return false;

       
        if (ContarGrafitis() <= 0) return false;

        return true;
    }

   
    public int ContarGrafitis()
    {
        if (ascensorTransform == null) return 0;

        int contador = 0;
        foreach (Transform hijo in ascensorTransform)
        {
            if (hijo.CompareTag(tagGrafiti))
                contador++;
        }
        return contador;
    }

    
    public void IntentarLimpiar()
    {
        if (!SePuedeLimpiar())
        {
            Debug.Log("No se puede limpiar: falta dinero o no hay grafitis");
            return;
        }

       
        GameManager.Instance.GameData.dineroTotal -= costoLimpiar;

        
        GameManager.Instance.ModificarLimpieza(limpiezaRecuperada);

       
        DestruirUnGrafiti();

       
        GameManager.Instance.GameData.OnDataChanged?.Invoke();

        OnEstadoCambio?.Invoke();

        
    }

    private void DestruirUnGrafiti()
    {
        if (ascensorTransform == null) return;

        List<Transform> grafitis = new List<Transform>();
        foreach (Transform hijo in ascensorTransform)
        {
            if (hijo.CompareTag(tagGrafiti))
                grafitis.Add(hijo);
        }

        if (grafitis.Count == 0) return;

        
        Transform elegido = grafitis[Random.Range(0, grafitis.Count)];

        Debug.Log($"Eliminando grafiti: {elegido.name}");
        Destroy(elegido.gameObject);
    }
}