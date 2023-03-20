using UnityEngine;

namespace ICKT.UI
{
	[CreateAssetMenu(fileName = "UIManagerData", menuName = "ScriptableObject/ManagerData/UIManagerData")]
	public class UIManagerData : ScriptableObject
	{
		public UIBase[] UICollection;
	}
}
