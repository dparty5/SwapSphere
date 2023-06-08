using project_SwapSphere_models;
using project_SwapSphere_models.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace project_SwapSphere_dblayer
{
    public partial class EntityGateway : IDisposable
    {
        internal ProjectManagerContext Context { get; set; } = new ProjectManagerContext();

        public Guid AddOrUpdate(IEntity entity)
        {

            if (entity.Id == Guid.Empty)
                Context.Add(entity);
            else
                Context.Update(entity);
            Context.SaveChanges();
            return entity.Id;

        }

        public void AddOrUpdate(params IEntity[] entities)
        {
            var toAdd = entities.Where(x => x.Id == Guid.Empty);
            var toUpdate = entities.Except(toAdd);
            Context.AddRange(toAdd);
            Context.UpdateRange(toUpdate);
            Context.SaveChanges();
        }


        public int EntitiesInSwap(ActionType action,Guid swapId, params Guid[] entitiesIds)
        {
            var swap = Context.Swaps.FirstOrDefault(x => x.Id == swapId)
                 ?? throw new Exception("Swap is not found.");

            var entities = Context.Entities.Where(x =>
                entitiesIds.Contains(x.Id)).ToArray();

           

            AddOrUpdate(swap);
            Context.SaveChanges();
            return entities.Length;
        }

        public int SwapsInCategory(ActionType action, Guid categoryId, params Guid[] swapsIds) 
        {
            var category = Context.Categories.FirstOrDefault(x => x.Id == categoryId)
                                        ?? throw new Exception("Category is not found.");
            var swaps = Context.Swaps.Where(x => swapsIds.Contains(x.Id)).Except(category.Swaps).ToArray();

            foreach (var swap in swaps)
                if (action == ActionType.Add)
                    category.Swaps.Add(swap);
                else
                    category.Swaps.Remove(swap);
            AddOrUpdate(category);
            Context.SaveChanges();
            return swaps.Length;
        }




        public void Delete(params IEntity[] entities)
        {
            Context.RemoveRange(entities);
            Context.SaveChanges();
        }




        #region IDisposable implementation
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion


        public enum ActionType
        {
            Add,
            Remove
        }
    }
}
