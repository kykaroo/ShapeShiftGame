namespace Data
{
    public interface IDataProvider
    {
        void Save();
        bool TryLoad();
        void DeleteSave();
    }
}