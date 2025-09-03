using System.Collections.Generic;
using System.Threading.Tasks;
using MultiTenantApi.Domain.Categories;

namespace MultiTenantApi.Application.Categories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
}
