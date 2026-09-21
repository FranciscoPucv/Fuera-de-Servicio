using UnityEngine;
using System.Collections.Generic;

public class PasajeroSpawner : MonoBehaviour
{
    public GameObject pasajeroPrefab;
    public AscensorController ascensor;

    public Transform spawnInicio;
    public Transform spawnFinal;

    public float intervaloSpawn = 3f;
    public int maxPasajeros = 5;

    private float timer = 0f;
    private int pasajerosActuales = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= intervaloSpawn)
        {
            timer = 0f;

            
            Debug.Log($"Spawner: pasajerosActuales = {pasajerosActuales}/{maxPasajeros}");

            if (pasajerosActuales < maxPasajeros)
            {
                SpawnearPasajero();
            }
        }
    }

    private void SpawnearPasajero()
    {
        bool spawnearAbajo = Random.value > 0.5f;

        GameObject nuevo = Instantiate(pasajeroPrefab);
        Pasajero p = nuevo.GetComponent<Pasajero>();

        if (p != null)
        {
            if (spawnearAbajo)
                p.Inicializar(spawnInicio, RolPasajero.SubeEnInicio, ascensor);
            else
                p.Inicializar(spawnFinal, RolPasajero.SubeEnFinal, ascensor);

            pasajerosActuales++;

            p.OnPasajeroTerminado += CuandoPasajeroTermina;
        }
    }

    private void CuandoPasajeroTermina(Pasajero p)
    {
        pasajerosActuales--;
        p.OnPasajeroTerminado -= CuandoPasajeroTermina; // Desuscribirse
        Debug.Log($" Spawner: pasajero terminó. Quedan {pasajerosActuales}");
    }
}