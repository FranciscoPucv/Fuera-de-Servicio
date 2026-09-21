using UnityEngine;

public class CameraFollow : MonoBehaviour
{
 
    [SerializeField] private Transform target; 
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); 

    
    [SerializeField] private float suavizado = 5f;
    [SerializeField] private bool seguirEnX = true;
    [SerializeField] private bool seguirEnY = true;

   
    [SerializeField] private bool usarLimites = false;
    [SerializeField] private Vector2 limiteMin = new Vector2(-10, -10);
    [SerializeField] private Vector2 limiteMax = new Vector2(10, 10);

    private void LateUpdate()
    {
        if (target == null) return;

 
        Vector3 posicionObjetivo = target.position + offset;

    
        if (usarLimites)
        {
            posicionObjetivo.x = Mathf.Clamp(posicionObjetivo.x, limiteMin.x, limiteMax.x);
            posicionObjetivo.y = Mathf.Clamp(posicionObjetivo.y, limiteMin.y, limiteMax.y);
        }

  
        transform.position = Vector3.Lerp(transform.position, posicionObjetivo, suavizado * Time.deltaTime);
    }

  
    public void SetTarget(Transform nuevoTarget)
    {
        target = nuevoTarget;
    }
}