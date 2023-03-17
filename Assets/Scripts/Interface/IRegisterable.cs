namespace ICKT.Services
{
	public interface IRegisterable
	{
		public bool IsPersistent(); // only works when it's a MonoBehaviour, will be in ServiceLocator DontDestroyOnLoad() if true
	}
}
