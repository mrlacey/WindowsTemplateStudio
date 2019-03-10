using System.Collections.Generic;
using System.Threading.Tasks;
using Param_RootNamespace.Core.Models;

namespace Param_RootNamespace.Core.Services
{
    // TODO WTS: Change your code to use this instead of the SampleDataService.
    public class MobileAppDataService : HttpDataService
    {
        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await GetAsync<IEnumerable<Item>>("api/item", forceRefresh);
        }
        
        public async Task<Item> GetItemAsync(string id)
        {
            if (id != null)
            {
                return await GetAsync<Item>($"api/item/{id}");
            }

            return null;
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            if (item == null)
            {
                return false;
            }

            return await PostAsJsonAsync($"api/item", item);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            if (item == null || item.Id == null)
            {
                return false;
            }

            return await PutAsync($"api/item/{item.Id}", item);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }

            return await DeleteAsync($"api/item/{id}");
        }
    }
}
