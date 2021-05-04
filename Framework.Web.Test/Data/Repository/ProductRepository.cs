using Framework.Core.Repository;
using Framework.Core.Repository.Interfaces;
using Framework.Web.Test.Models;

namespace Framework.Web.Test.Data.Repository
{
    public class ProductRepository : BaseMongoRepository<Product>, ITestRepository
    {

        public ProductRepository(IMongoContext context) : base(context)
        {
        }


    }
}
