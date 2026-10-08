using UnityEngine;
using System.Collections.Generic;

public class PasajeroSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnConfig
    {
        
        public string nombre;
        public Transform spawnPoint;
        public int ascensorIndex = 0;
        public RolPasajero rol;

     
        [Tooltip(" (1 = normal, 0.5 = mitad, etc.)")]
        public float escalaEnSpawn = 1f;
    }

   
    public GameObject pasajeroPrefab;
    public AscensorManager ascensorManager;

    
    public List<SpawnConfig> spawns = new List<SpawnConfig>();

   
    public float intervaloSpawn = 3f;
    public int maxPasajeros = 5;
    public bool spawnAutomatico = true;

    private float timer = 0f;
    private int pasajerosActuales = 0;

    private void Update()
    {
        if (!spawnAutomatico) return;
        if (ascensorManager == null) return;
        if (spawns.Count == 0) return;

        timer += Time.deltaTime;
        if (timer >= intervaloSpawn)
        {
            timer = 0f;

          

            if (pasajerosActuales < maxPasajeros)
            {
                SpawnearPasajero();
            }
        }
    }

    private void SpawnearPasajero()
    {
        if (pasajeroPrefab == null)
        {
        
            return;
        }

        SpawnConfig config = spawns[Random.Range(0, spawns.Count)];

        if (config.ascensorIndex < 0 || config.ascensorIndex >= ascensorManager.ascensores.Count)
        {
            
            return;
        }

        GameObject nuevo = Instantiate(pasajeroPrefab);
        Pasajero p = nuevo.GetComponent<Pasajero>();

        if (p == null)
        {
            
            Destroy(nuevo);
            return;
        }

     
        nuevo.transform.localScale = Vector3.one * config.escalaEnSpawn;

       
        p.Inicializar(
            config.spawnPoint,
            config.rol,
            config.ascensorIndex,
            ascensorManager
        );

        pasajerosActuales++;
        p.OnPasajeroTerminado += CuandoPasajeroTermina;

        
    }

    private void CuandoPasajeroTermina(Pasajero p)
    {
        pasajerosActuales--;
        p.OnPasajeroTerminado -= CuandoPasajeroTermina;
        
    }
}