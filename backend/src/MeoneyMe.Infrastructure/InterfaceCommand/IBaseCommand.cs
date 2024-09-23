

namespace MeoneyMe.Infrastructure;

public interface IBaseCommand<TModel, TValue>
{
    Task<TValue> AddAsync(TModel model);

    Task<TValue> UpdateAsync(TModel model);


}
