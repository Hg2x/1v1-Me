using ICKT.Services;
using ICKT.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[AutoRegisteredService]
public class InputHandler : MonoBehaviour, IRegisterable
{
	private PlayerInputAsset _Input;

	public Vector2 InputMoveVector { get; private set; }

	public bool IsPersistent() => true;

	private void Awake()
	{
		_Input = new PlayerInputAsset();
	}

	private void OnEnable()
	{
		_Input.Default.Enable();

		_Input.Default.Move.performed += SetInputMoveVector;
		_Input.Default.Move.canceled += SetInputMoveVector;
		_Input.Default.Jump.started += JumpStarted;

#if UNITY_EDITOR
		_Input.Default.ShowDebugMenu.started += ShowDebugMenu;
#endif
	}

	private void SetInputMoveVector(InputAction.CallbackContext context)
	{
		InputMoveVector = context.ReadValue<Vector2>();
	}

	private void JumpStarted(InputAction.CallbackContext context)
	{
		
	}

#if UNITY_EDITOR
	private void ShowDebugMenu(InputAction.CallbackContext context)
	{
		UIManager.Show<UIDebugMenu>();
	}
#endif

	private void OnDisable()
	{
		_Input.Default.Move.performed -= SetInputMoveVector;
		_Input.Default.Move.canceled -= SetInputMoveVector;
		_Input.Default.Jump.started -= JumpStarted;

#if UNITY_EDITOR
		_Input.Default.ShowDebugMenu.started -= ShowDebugMenu;
#endif
	}
}
