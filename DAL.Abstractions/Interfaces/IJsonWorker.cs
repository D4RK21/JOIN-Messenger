using System.Threading.Tasks;

namespace DAL.Abstractions.Interfaces
{
    public interface IJsonWorker
    {
        public Task SaveToFileAsync<T>(T obj, string fileName);

        public Task<T> LoadFromFileAsync<T>(string fileName);
    }
}
