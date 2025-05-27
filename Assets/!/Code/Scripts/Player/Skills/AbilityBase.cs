using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbilityBase : MonoBehaviour
{
    public float cooldown = 1f;
    protected bool ableToUse = true;

    [SerializeField]
    protected string inputActionName;

    private InputAction _inputAction;

    protected virtual void Awake()
    {
        if (string.IsNullOrEmpty(inputActionName))
            Debug.LogWarning($"{gameObject.name}: inputActionName is not set on {GetType().Name}");
    }

    protected virtual void OnEnable()
    {
        if (!string.IsNullOrEmpty(inputActionName))
        {
            _inputAction = InputSystem.actions.FindAction(inputActionName);
            if (_inputAction != null)
                _inputAction.performed += OnInputPerformed;
        }
    }

    protected virtual void OnDisable()
    {
        if (_inputAction != null)
            _inputAction.performed -= OnInputPerformed;
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (ableToUse)
            StartCoroutine(UseAbility());
    }

    private IEnumerator UseAbility()
    {
        ableToUse = false;
        Execute();
        yield return new WaitForSeconds(cooldown);
        ableToUse = true;
    }

    protected abstract void Execute();
}
