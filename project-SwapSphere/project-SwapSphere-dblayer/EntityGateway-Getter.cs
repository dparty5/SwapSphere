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


      

        public IEnumerable<Swap> GetSwaps(Func<Swap, bool> predicate) =>
           Context.Swaps.Where(predicate).ToArray();
        public IEnumerable<Swap> GetSwaps() =>
            GetSwaps((x) => true);



      

    }
}
