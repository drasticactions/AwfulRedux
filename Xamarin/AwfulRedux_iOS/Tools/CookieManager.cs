using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AwfulForumsLibrary.Tools;
using PCLStorage;

namespace AwfulRedux_iOS
{
	public class CookieManager
	{
		public async Task<bool> SaveCookie(string filename, CookieContainer rcookie, Uri uri)
		{
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			IFile file = await rootFolder.CreateFileAsync("cookie.txt", CreationCollisionOption.ReplaceExisting);

			try
			{
				using (var transaction = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
				{
					CookieSerializer.Serialize(rcookie.GetCookies(uri), uri, transaction);
					await transaction.WriteAsync(ReadFully((Stream)transaction), 0, (int)transaction.Length - 1);
				}

			}
			catch (Exception ex)
			{
				//Debug.WriteLine(string.Format("Failed to save cookies used for logging in. {0}", ex.Message));
			}

			return true;
		}

		public async Task<CookieContainer> LoadCookie(string filename)
		{
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			try
			{
				IFile file = await rootFolder.GetFileAsync("cookie.txt");
				using (Stream stream = await file.OpenAsync(PCLStorage.FileAccess.Read))
				{
					return CookieSerializer.Deserialize(new Uri("https://fake.forums.somethingawful.com"), stream);
				}
			}
			catch
			{
				//Ignore, we will ask for log in information.
			}
			return new CookieContainer();
		}

		public async Task<bool> RemoveCookies(string filename)
		{
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			try
			{
				IFile file = await rootFolder.GetFileAsync("cookie.txt");
				await file.DeleteAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static byte[] ReadFully(Stream input)
		{
			var buffer = new byte[16 * 1024];
			using (var ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}
	}
}
