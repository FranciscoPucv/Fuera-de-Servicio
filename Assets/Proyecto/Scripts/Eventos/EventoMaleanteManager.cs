using UnityEngine;
using System;
using System.Collections.Generic;

public class EventoMaleanteManager : MonoBehaviour
{

    public GameObject maleantePrefab;
    public float tiempoMinEntreEventos = 20f;
    public float tiempoMaxEntreEventos = 40f;
    public AscensorManager ascensorManager;

    public event Action<GameObject> OnEventoTerminado;

    private GameObject maleanteInstanciado;
    private bool eventoEnCurso = false;

    public bool IntentarLanzarEvento()
    {
        if (eventoEnCurso) return false;
        if (ascensorManager == null) return false;
        if (maleantePrefab == null) return false;

        if (ascensorManager.enMovimiento) return false;
        bool hayCerca = false;
        foreach (var asc in ascensorManager.ascensores)
        {
            if (!asc.estaAbajo) { hayCerca = true; break; }
        }

        if (!hayCerca) return false;
        LanzarMaleante();
        return true;
    }

    private void LanzarMaleante()
    {
        eventoEnCurso = true;

        Debug.Log("Lanzando evento de Maleante");

        Vector3 posSpawn = CalcularSpawn();
        maleanteInstanciado = Instantiate(maleantePrefab, posSpawn, Quaternion.identity);

        Maleante maleante = maleanteInstanciado.GetComponent<Maleante>();
        if (maleante != null)
        {
            maleante.OnMaleanteTerminado += AlTerminarMaleante;
        }
    }

    private void AlTerminarMaleante(Maleante m)
    {
        if (m != null)
            m.OnMaleanteTerminado -= AlTerminarMaleante;

        eventoEnCurso = false;
        maleanteInstanciado = null;

        OnEventoTerminado?.Invoke(null);
    }

    private Vector3 CalcularSpawn()
    {
        Transform ascensorRef = ascensorManager.ObtenerAscensorCerca();
        if (ascensorRef == null && ascensorManager.ascensores.Count > 0)
            ascensorRef = ascensorManager.ascensores[0].ascensorTransform;

        if (ascensorRef == null) return Vector3.zero;

        float ladoX = UnityEngine.Random.value > 0.5f ? -8f : 8f;
        float offsetY = UnityEngine.Random.Range(-1f, 1f);

        return new Vector3(ladoX, ascensorRef.position.y + offsetY, 0);
    }
}