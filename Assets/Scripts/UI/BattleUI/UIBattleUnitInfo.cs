using UnityEngine;
using UnityEngine.UI;

public class UIBattleUnitInfo : MonoBehaviour
{
    private UnitDataBase _Data;
	[SerializeField] private Image _HealthBar;

	public void Initialize(UnitDataBase data)
	{
		if (data != null)
		{
			_Data = data;
			Subscribe(_Data);

			UpdateHealthBar(_Data.MaxHealth, _Data.MaxHealth); // TODO: fix this later, update to correct proper health data
		}
	}

	private void OnEnable()
	{
		if (_Data != null)
		{
			Subscribe(_Data);
		}
	}

	private void OnDisable()
	{
		if (_Data != null)
		{
			Unsubscribe(_Data);
		}
	}

	private void Subscribe(UnitDataBase data)
	{
		data.OnHealthChanged += UpdateHealthBar;
	}

	private void Unsubscribe(UnitDataBase data)
	{
		data.OnHealthChanged -= UpdateHealthBar;
	}

	private void UpdateHealthBar(float currentHealth, float maxHealth)
	{
		_HealthBar.fillAmount = currentHealth / maxHealth;
	}
}
