using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Options;

namespace WebApiCore.Infrastructure.Data
{
    public abstract class DapperDbContext : IContext
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public bool IsTransactionStarted { get; private set; }
        private int? _commandTimeout = null;
        private readonly DapperDbContextOptions _options;
        protected abstract IDbConnection CreateConnection(string connectionString);

        protected DapperDbContext(IOptions<DapperDbContextOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
            _connection = CreateConnection(_options.Configuration);
            _connection.Open();

            DebugPrint("Connection started.");
        }

        public void BeginTransaction()
        {
            if (IsTransactionStarted)
            {
                throw new InvalidOperationException("Transaction is already started.");
            }
            _transaction = _connection.BeginTransaction();
            IsTransactionStarted = true;

            DebugPrint("Transaction started.");
        }

        public void Commit()
        {
            if (!IsTransactionStarted)
            {
                throw new InvalidOperationException("No transaction started.");
            }

            _transaction.Commit();
            _transaction = null;
            IsTransactionStarted = false;

            DebugPrint("Transaction committed.");
        }

        public void Rollback()
        {
            if (!IsTransactionStarted)
            {
                throw new InvalidOperationException("No transaction started.");
            }

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;

            IsTransactionStarted = false;

            DebugPrint("Transaction rollbacked and disposed.");
        }

        public void Dispose()
        {
            if (IsTransactionStarted)
            {
                Rollback();
            }

            _connection.Close();
            _connection.Dispose();
            _connection = null;

            DebugPrint("Connection closed and disposed.");
        }

        #region Dapper Execute & Query

        public async Task<bool> AddEntityAsync<T>(T entity) where T : class
        {
            return await _connection.InsertAsync(entity, _transaction, _commandTimeout) > 0;
        }

        public async Task<bool> ModifyEntityAsync<T>(T entity) where T : class
        {
            return await _connection.UpdateAsync(entity, _transaction, _commandTimeout);
        }


        public async Task<int> ExecuteAsync(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return await _connection.ExecuteAsync(sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return await _connection.QueryAsync<T>(sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>() where T : class
        {
            return await _connection.GetAllAsync<T>(_transaction, _commandTimeout);
        }

        public async Task<T> QueryByKeyAsync<T>(object key) where T : class
        {
            return await _connection.GetAsync<T>(key, _transaction, _commandTimeout);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, param, _transaction, _commandTimeout, commandType);
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id", CommandType commandType = CommandType.Text)
        {
            return await _connection.QueryAsync(sql, map, param, _transaction, true, splitOn, _commandTimeout, commandType);
        }

        #endregion Dapper Execute & Query

        private void DebugPrint(string message)
        {
#if DEBUG
            Debug.Print(">>> UnitOfWorkWithDapper - Thread {0}: {1}", Thread.CurrentThread.ManagedThreadId, message);
#endif
        }
    }
}
