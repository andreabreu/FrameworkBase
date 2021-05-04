using Framework.Core.Domain;
using Framework.Core.Repository.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Web.Test.Data
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _db;

        private IClientSessionHandle Session;

        private readonly MongoClient _mongoClient;

        private readonly List<Func<Task>> _commands;

        private MongoSettings _mongoSettings;

        public MongoContext(IOptions<MongoSettings> mongoSettings)
        {
            _commands = new List<Func<Task>>();

            _mongoSettings = mongoSettings.Value;

            _mongoClient = new MongoClient(_mongoSettings.Connection);
            _db = _mongoClient.GetDatabase(_mongoSettings.DatabaseName);
        }

        public async Task<int> SaveChanges()
        {
            using (Session = await _mongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }
            return _commands.Count;
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }
    }
}
