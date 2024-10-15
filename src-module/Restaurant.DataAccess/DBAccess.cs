using System.Linq.Expressions;
using LiteDB;
using Microsoft.Extensions.Logging;
using Restaurant.DataAccess.Entities;

namespace Restaurant.DataAccess;

public class DBAccess<T> : IDataAccess<T>, IDataService<T> where T : new()
{
    private ILiteCollection<T> _collection;
    private ILogger _logger;
    private ILiteDatabase _liteDatabase;

    public ILiteCollection<T> Collection
    {
        get;
        set;
    }
    
    public DBAccess(ILiteDatabase liteDB, ILogger logger) 
    {
        _liteDatabase = liteDB;
        _logger = logger;
        _collection = liteDB.GetCollection<T>();
        Collection = _collection;
    }

    #region Update Operation
    public async Task<bool> Insert(T data)
    {
        bool result = false;
        try
        {
            await _liteDatabase.OpenAsync();
            await _collection.InsertAsync(data);
            _logger.LogInformation($"Inserted Data -> {data.ToString()}");
            result = true;
        }
        catch(Exception ex)
        {
            _logger.LogError(exception: ex, message: "DataAccess -> Insert");
            result = false;
        }
        finally
        {
           await _liteDatabase.CheckpointAsync();
           await _liteDatabase.DisposeAsync();
           _logger.LogInformation($"Insert -> Connection Closed and Disposed.");
           
        }
        return result;
    }
    
    public async Task<bool> Update(T data)
    {
        bool result = false;
        try
        {
            await _liteDatabase.OpenAsync();
            await _collection.UpdateAsync(data);
            _logger.LogInformation($"Update Data -> {data.ToString()}");
            result = true;
        }
        catch(Exception ex)
        {
            _logger.LogError(exception: ex, message: "DataAccess -> Update");
            result = false;
        }
        finally
        {
            await _liteDatabase.CheckpointAsync();
            await _liteDatabase.DisposeAsync();
            _logger.LogInformation($"Update -> Connection Closed and Disposed.");
           
        }
        return result;
    }
    
    public async Task<bool> Delete(int objId)
    {
        bool result = false;
        try
        {
            await _liteDatabase.OpenAsync();
            await _collection.DeleteAsync(objId);
            _logger.LogInformation($"Delete Data -> {objId}");
            result = true;
        }
        catch(Exception ex)
        {
            _logger.LogError(exception: ex, message: "DataAccess -> Delete");
            result = false;
        }
        finally
        {
            await _liteDatabase.CheckpointAsync();
            await _liteDatabase.DisposeAsync();
            _logger.LogInformation($"Delete -> Connection Closed and Disposed.");
           
        }
        return result;
    }

    #endregion

    #region Get Operations
    
    public async Task<T> GetOne(Expression<Func<T, bool>> expression)
    {
        T result ;
        try
        {
            await _liteDatabase.OpenAsync();
            result = await _collection.Query()
                .Where(expression).FirstOrDefaultAsync();
            _logger.LogInformation($"GetOne Data");
            
        }
        catch(Exception ex)
        {
            _logger.LogError(exception: ex, message: "GetOne");
            result = new T();
        }
        finally
        {
            await _liteDatabase.CheckpointAsync();
            await _liteDatabase.DisposeAsync();
            _logger.LogInformation($"GetOne -> Connection Closed and Disposed.");
           
        }
        return result;
    }
    
    public async Task<IList<T>> GetData(Expression<Func<T, bool>> expression)
    {
        IList<T> result ;
        try
        {
            await _liteDatabase.OpenAsync();
            result = await _collection.Query()
                .Where(expression).ToListAsync();
            _logger.LogInformation($"GetData Data");
        }
        catch(Exception ex)
        {
            _logger.LogError(exception: ex, message: "GetData");
            result = new List<T>();
        }
        finally
        {
            await _liteDatabase.CheckpointAsync();
            await _liteDatabase.DisposeAsync();
            _logger.LogInformation($"GetData -> Connection Closed and Disposed.");
        }
        return result;
    }
    
    public async Task<IList<T>> ExecuteAny(ILiteQueryable<T> queryExpression)
    {
        IList<T> result ;
        try
        {
            await _liteDatabase.OpenAsync();
            result = await queryExpression.ToListAsync();
            _logger.LogInformation($"ExecuteAny Data");
        }
        catch(Exception ex)
        {
            _logger.LogError(exception: ex, message: "ExecuteAny");
            result = new List<T>();
        }
        finally
        {
            await _liteDatabase.CheckpointAsync();
            await _liteDatabase.DisposeAsync();
            _logger.LogInformation($"ExecuteAny -> Connection Closed and Disposed.");
        }
        return result;
    }
    public async Task<IList<T>> GetAll()
    {
        IList<T> result ;
        try
        {
            await _liteDatabase.OpenAsync();
            var data = _collection.FindAllAsync();
            var list = new List<T>();
            await foreach (var item in data)
            {
                list.Add(item);
            }
            _logger.LogInformation("GetAll Data: {ListCount}", list?.Count);
            result = list;
        }
        catch(Exception ex)
        {
            _logger.LogError(exception: ex, message: "GetAll");
            result = new List<T>();
        }
        finally
        {
            await _liteDatabase.CheckpointAsync();
            await _liteDatabase.DisposeAsync();
            _logger.LogInformation($"GetAll -> Connection Closed and Disposed.");
        }
        return result;
    }

    #endregion
    
}
public interface IDataAccess<in T>
{
    public Task<bool> Insert(T data);
    public Task<bool> Update(T data);
    public Task<bool> Delete(int objId);
    
}
public interface IDataService<T>
{
    Task<T> GetOne(Expression<Func<T, bool>> expression);
    Task<IList<T>> GetData(Expression<Func<T, bool>> expression);
    Task<IList<T>> ExecuteAny(ILiteQueryable<T> queryExpression);
    Task<IList<T>> GetAll();
}