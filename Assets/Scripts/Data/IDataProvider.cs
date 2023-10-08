namespace Data
{
    public interface IDataProvider<T>
    {
        void Save();
        T GetData();
        void DeleteSave();
    }
}