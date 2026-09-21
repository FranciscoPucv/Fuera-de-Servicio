using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PalancaController : MonoBehaviour
{
  
    public AscensorController ascensorController;

    public InputActionAsset inputActionsAsset;
   
    public string actionMapName = "Ascensor";
  
    public string actionName = "MoverAscensor";

    
    private InputAction moverAscensorAction;

  
    public float anguloIzquierda = 0f;
    public float anguloDerecha = 180f;
    public float velocidadRotacion = 360f;
    public float duracionAnimacion = 0f;

   
    public bool palancaADerecha = false;
    public bool enAnimacion = false;

    private void Awake()
    {
       
        if (inputActionsAsset != null)
        {
            InputActionMap map = inputActionsAsset.FindActionMap(actionMapName, true);
            if (map != null)
            {
                moverAscensorAction = map.FindAction(actionName, true);

                if (moverAscensorAction == null)
                {
                    Debug.LogError($"No se encontró la acción '{actionName}' en el Action Map '{actionMapName}'");
                }
            }
            else
            {
                Debug.LogError($" No se encontró el Action Map '{actionMapName}'");
            }
        }
        else
        {
            Debug.LogError(" Input Action Asset no asignado en el Inspector");
        }
    }

    private void OnEnable()
    {
        
        if (moverAscensorAction != null)
        {
            moverAscensorAction.Enable();
            moverAscensorAction.performed += OnMoverAscensor;
        }
    }

    private void OnDisable()
    {
      
        if (moverAscensorAction != null)
        {
            moverAscensorAction.performed -= OnMoverAscensor;
            moverAscensorAction.Disable();
        }
    }

 
    private void OnMoverAscensor(InputAction.CallbackContext context)
    {
       
        if (enAnimacion) return;

        
        if (ascensorController != null && ascensorController.enMovimiento)
        {
            Debug.Log(" No puedes mover la palanca mientras el ascensor está en movimiento");
            return;
        }

        MoverPalanca();
    }

    private void MoverPalanca()
    {
        palancaADerecha = !palancaADerecha;
        float anguloObjetivo = palancaADerecha ? anguloDerecha : anguloIzquierda;

        StartCoroutine(RotarPalanca(anguloObjetivo));

        if (ascensorController != null)
        {
            ascensorController.IniciarViaje();
        }
    }

    private IEnumerator RotarPalanca(float anguloObjetivo)
    {
        enAnimacion = true;

        Quaternion rotacionInicial = transform.localRotation;
        Quaternion rotacionFinal = Quaternion.Euler(0, anguloObjetivo, 0);
        float tiempoTranscurrido = 0f;

        float duracion = duracionAnimacion;
        if (velocidadRotacion > 0f)
        {
            float diferenciaAngulo = Quaternion.Angle(rotacionInicial, rotacionFinal);
            duracion = diferenciaAngulo / velocidadRotacion;
        }

        while (tiempoTranscurrido < duracion)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = Mathf.Clamp01(tiempoTranscurrido / duracion);
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.localRotation = Quaternion.Slerp(rotacionInicial, rotacionFinal, t);
            yield return null;
        }

        transform.localRotation = rotacionFinal;
        enAnimacion = false;

        Debug.Log($"Palanca movida a {(palancaADerecha ? "DERECHA" : "IZQUIERDA")}");
    }

    public void ResetearPalanca()
    {
        palancaADerecha = false;
        transform.localRotation = Quaternion.Euler(0, anguloIzquierda, 0);
    }
}