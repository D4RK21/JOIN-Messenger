using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core;
using DAL.Abstractions.Interfaces;
using DAL.Worker;
using Microsoft.Extensions.Options;

namespace DAL.Repository
{
    public class GenericRepositoryJson<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity

    {
        private readonly AppSettings _appSettings;
        private readonly IJsonWorker _jsonWorker;
        private List<TEntity> _allData;

        public GenericRepositoryJson(IJsonWorker jsonWorker, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _jsonWorker = jsonWorker;
            _allData = new List<TEntity>();
        }

        private async Task<IEnumerable<TEntity>> FindAllAsync()
        {
            var file = GetFile(typeof(TEntity));

            return await _jsonWorker.LoadFromFileAsync<IEnumerable<TEntity>>(_appSettings.JsonDirectory + file);
        }

        public IList<TEntity> FindByCondition(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TEntity>> selector = null)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetEntityById(int id)
        {
            var entities = await FindByConditionAsync(entity => entity.Id == id);

            return entities.FirstOrDefault();
        }
        
        public async Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression)
        {
            var m = await FindAllAsync();
            return m.Where(expression.Compile());
        }

        public async Task CreateAsync(TEntity entity)
        {
            var file = GetFile(typeof(TEntity));

            try
            {
                _allData = (await FindAllAsync()).ToList();
                _allData.Add(entity);
                await _jsonWorker.SaveToFileAsync(_allData, _appSettings.JsonDirectory + file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async void Update(TEntity entity)
        {
            Delete(entity);
            await CreateAsync(entity);
        }

        public async void Delete(TEntity entity)
        {
            var file = GetFile(typeof(TEntity));

            _allData = (await FindAllAsync()).ToList();
            var k = _allData.FirstOrDefault(o => o.Id == entity.Id);
            _allData.Remove(k);
            await _jsonWorker.SaveToFileAsync(_allData, _appSettings.JsonDirectory + file);
        }

        private string GetFile(Type fileType)
        {
            if (fileType == typeof(User))
            {
                return "user.json";
            }
            else if (fileType == typeof(Room))
            {
                return "room.json";
            }
            else if (fileType == typeof(InviteLink))
            {
                return "urls.json";
            }
            else if (fileType == typeof(Role))
            {
                return "role.json";
            }
            else if (fileType == typeof(TextChannel))
            {
                return "textChannel.json";
            }

            else if (fileType == typeof(PersonalChat))
            {
                return "personalChats.json";
            }
            return string.Empty;
        }
    }
}
