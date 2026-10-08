using UnityEngine;
using System;

public abstract class EventoBase : MonoBehaviour
{
    public event Action OnTerminado;

 
    public float duracionMaxima = 10f;

    protected float timer = 0f;
    protected bool terminado = false;

    protected virtual void Start() { }

    protected virtual void Update()
    {
        
    }


    protected void Terminar()
    {
        if (terminado) return;
        terminado = true;

        OnTerminado?.Invoke();

        Destroy(gameObject);
    }
}