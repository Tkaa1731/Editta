using Blazored.LocalStorage;
using System.Text;
using System.Text.Json;

namespace Pobytne.Client.Extensions
{
    public static class LocalStorageService
    {
        public const string USER_SESSION = "UserSession";
        public const string MODULE_SESSION = "ActiveModule";
        public static async Task SaveItemEncrypted<T>(this ILocalStorageService localStorageService, string key, T value)
        {
            var itemJson = JsonSerializer.Serialize(value);
            var itemJsonBytes = Encoding.UTF8.GetBytes(itemJson);
            var base64Json = Convert.ToBase64String(itemJsonBytes);
            await localStorageService.SetItemAsync(key, base64Json);
        }
        public static async Task<T?> ReadEncryptedItem<T>(this ILocalStorageService localStorageService, string key)
        {
            var base64Json = await localStorageService.GetItemAsync<string>(key);
            var itemJsonBytes = Convert.FromBase64String(base64Json);
            var itemJson = Encoding.ASCII.GetString(itemJsonBytes); 
            var item = JsonSerializer.Deserialize<T>(itemJson);
            return item;
        }
    }
}
