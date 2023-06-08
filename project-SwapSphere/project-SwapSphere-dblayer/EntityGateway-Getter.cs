using project_SwapSphere_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_SwapSphere_dblayer
{
    public partial class EntityGateway
    {
        public IEnumerable<Category> GetCategories(Func<Category, bool> predicate) =>
          Context.Categories.Where(predicate).ToArray();
        public IEnumerable<Category> GetCategories() =>
            GetCategories((x) => true);


        public IEnumerable<Client> GetClients(Func<Client,bool> predicate) =>
            Context.Clients.Where(predicate).ToArray();
        public IEnumerable<Client> GetClients() =>
            GetClients((x) => true);


        public IEnumerable<Entity_Swap> GetEntity_Swaps(Func<Entity_Swap,bool> predicate) =>
            Context.Entities.Where(predicate).ToArray();
        public IEnumerable<Entity_Swap> GetEntity_Swaps() =>
            GetEntity_Swaps((x) => true);


        public IEnumerable<Swap> GetSwaps(Func<Swap, bool> predicate) =>
           Context.Swaps.Where(predicate).ToArray();
        public IEnumerable<Swap> GetSwaps() =>
            GetSwaps((x) => true);


        public IEnumerable<Order> GetOrders(Func<Order, bool> predicate) =>
           Context.Orders.Where(predicate).ToArray();
        public IEnumerable<Order> GetOrders() =>
            GetOrders((x) => true);


        public IEnumerable<Placeswap> GetPlaceswap(Func<Placeswap, bool> predicate) =>
           Context.Platforms.Where(predicate).ToArray();
        public IEnumerable<Placeswap> GetPlaceswap() =>
            GetPlaceswap((x) => true);

    }
}
