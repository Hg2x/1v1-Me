using ICKT.Services;
using ICKT.UI;
using UnityEngine;
using UnityEngine.InputSystem;

[AutoRegisteredService]
public class InputHandler : MonoBehaviour, IRegisterable
{
	private PlayerInputAsset _Input;

	public Vector2 InputMoveVector { get; private set; }
	public bool DoJump = false;
	public bool DoDodge = false;
	public bool DoAttack = false;
	public bool DoSkill1 = false;
	public bool DoSkill2 = false;
	public bool DoSkill3 = false;

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
		_Input.Default.Dodge.started += DodgeStarted;
		_Input.Default.Attack.started += AttackStarted;
		_Input.Default.Skill1.started += Skill1Started;
		_Input.Default.Skill2.started += Skill2Started;
		_Input.Default.Skill3.started += Skill3Started;

#if UNITY_EDITOR
		_Input.Default.ShowDebugMenu.started += ShowDebugMenu;
#endif
	}

	private void OnDisable()
	{
		_Input.Default.Move.performed -= SetInputMoveVector;
		_Input.Default.Move.canceled -= SetInputMoveVector;
		_Input.Default.Jump.started -= JumpStarted;
		_Input.Default.Attack.started -= AttackStarted;
		_Input.Default.Skill1.started -= Skill1Started;
		_Input.Default.Skill2.started -= Skill2Started;
		_Input.Default.Skill3.started -= Skill3Started;

#if UNITY_EDITOR
		_Input.Default.ShowDebugMenu.started -= ShowDebugMenu;
#endif
	}

	public void StopButtonsInput()
	{
		DoJump = false;
		DoDodge = false;
		DoAttack = false;
		DoSkill1 = false;
	}

	private void SetInputMoveVector(InputAction.CallbackContext context)
	{
		InputMoveVector = context.ReadValue<Vector2>();
	}

	private void JumpStarted(InputAction.CallbackContext context)
	{
		DoJump = true;
	}

	private void DodgeStarted(InputAction.CallbackContext context)
	{
		DoDodge = true;
	}

	private void AttackStarted(InputAction.CallbackContext context)
	{
		DoAttack = true;
	}

	private void Skill1Started(InputAction.CallbackContext context)
	{
		DoSkill1 = true;
	}

	private void Skill2Started(InputAction.CallbackContext context)
	{
		DoSkill2 = true;
	}

	private void Skill3Started(InputAction.CallbackContext context)
	{
		DoSkill3 = true;
	}

#if UNITY_EDITOR
	private void ShowDebugMenu(InputAction.CallbackContext context)
	{
		UIManager.Show<UIDebugMenu>();
	}
#endif
}
