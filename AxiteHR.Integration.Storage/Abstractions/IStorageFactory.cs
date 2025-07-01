namespace AxiteHR.Integration.Storage.Abstractions
{
    public interface IStorageFactory
	{
		IObjectStorageService Get(ObjectStorageType type);
	}
}
