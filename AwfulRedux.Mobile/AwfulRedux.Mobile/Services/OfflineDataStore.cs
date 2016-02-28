using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AwfulRedux.UI.Models.Forums;
using Newtonsoft.Json;
using PCLStorage;
using Plugin.EmbeddedResource;

namespace AwfulRedux.Mobile.Services
{
    public class OfflineDataStore
    {
        public async Task<IEnumerable<Category>> GetDefaultForumList()
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            var json = ResourceLoader.GetEmbeddedResourceString(Assembly.Load(new AssemblyName("AwfulRedux.Mobile")), "forum.txt");
            return await Task.Run(() => JsonConvert.DeserializeObject<List<Category>>(json));
        }
    }
}
