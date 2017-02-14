using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using omnilistService.DataObjects;
using omnilistService.Models;

namespace omnilistService.Controllers
{
    [Authorize]
    public class GroceryController : TableController<Grocery>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            omnilistContext context = new omnilistContext();
            DomainManager = new EntityDomainManager<Grocery>(context, Request);
        }

        // GET tables/Grocery
        public IQueryable<Grocery> GetAllGrocery()
        {
            return Query(); 
        }

        // GET tables/Grocery/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Grocery> GetGrocery(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Grocery/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Grocery> PatchGrocery(string id, Delta<Grocery> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Grocery
        public async Task<IHttpActionResult> PostGrocery(Grocery item)
        {
            Grocery current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Grocery/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteGrocery(string id)
        {
             return DeleteAsync(id);
        }
    }
}
