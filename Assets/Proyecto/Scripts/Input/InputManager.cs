using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   
    public AscensorController ascensorController;

 
    private AscensorControls controls;

    private void Awake()
    {
        controls = new AscensorControls();

      
        //controls.Gameplay.MoverAscensor.performed += OnMoverAscensor;
        controls.Gameplay.Limpieza.performed += OnLimpieza;
        controls.Gameplay.Polea.performed += OnPolea;
        controls.Gameplay.Puerta.performed += OnPuerta;
    }

    private void OnEnable()
    {
        
        controls?.Enable();
    }

    private void OnDisable()
    {
        
        controls?.Disable();
    }

    private void OnDestroy()
    {
      
        if (controls != null)
        {
            //controls.Gameplay.MoverAscensor.performed -= OnMoverAscensor;
            controls.Gameplay.Limpieza.performed -= OnLimpieza;
            controls.Gameplay.Polea.performed -= OnPolea;
            controls.Gameplay.Puerta.performed -= OnPuerta;
        }
    }

    

    private void OnMoverAscensor(InputAction.CallbackContext context)
    {
        
       /* if (context.performed)
        {
            ascensorController.IniciarViaje();
            Debug.Log("Tecla presionada: Mover Ascensor");
        }*/
    }

    private void OnLimpieza(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.ModificarLimpieza(5);
            Debug.Log("+5 Limpieza (Tecla E)");
        }
    }

    private void OnPolea(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.ModificarPolea(5);
            Debug.Log("+5 Polea (Tecla W)");
        }
    }

    private void OnPuerta(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.ModificarPuerta(5);
            Debug.Log(" +5 Puerta (Tecla Q)");
        }
    }
}