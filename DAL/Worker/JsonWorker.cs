using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DAL.Abstractions.Interfaces;

namespace DAL.Worker
{
    public class JsonWorker : IJsonWorker
    {
        public async Task SaveToFileAsync<T>(T obj, string fileName)
        {
            var json = JsonConvert.SerializeObject(obj);
            await File.WriteAllTextAsync(fileName,json);
            
        }

        public async Task<T> LoadFromFileAsync<T>(string fileName)
        {
            var json = await File.ReadAllTextAsync(fileName);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
