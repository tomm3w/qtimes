namespace iVeew.common.api
{
	public interface IFactory
	{
		T GetInstance<T>();
	}
}