

namespace MeoneyMe.Infrastructure;

public interface IBaseQuery<TModel, TValue>
{
    Task<TModel?> GetByIDAsync(TValue value);

}
