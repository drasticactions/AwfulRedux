using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Windows.Storage;
using AwfulRedux.Core.Tools;

namespace AwfulRedux.Notifications
{
    public class CookieManager
    {
        public static async Task<bool> SaveCookie(string filename, CookieContainer rcookie, Uri uri)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var sampleFile = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            try
            {
                using (StorageStreamTransaction transaction = await sampleFile.OpenTransactedWriteAsync())
                {
                    CookieSerializer.Serialize(rcookie.GetCookies(uri), uri, transaction.Stream.AsStream());
                    await transaction.CommitAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static async Task<CookieContainer> LoadCookie(string filename)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile;
            try
            {
                sampleFile = await localFolder.GetFileAsync(filename);
            }
            catch
            {
                return null;
            }

            using (var stream = await sampleFile.OpenStreamForReadAsync())
            {
                return CookieSerializer.Deserialize(new Uri(EndPoints.CookieDomainUrl), stream);
            }
        }
    }
}
