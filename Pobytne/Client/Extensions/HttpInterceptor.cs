namespace Pobytne.Client.Extensions
{
    public static class HttpInterceptor
    {
        public static async Task<HttpResponse<T>> PostAsJsonAsyncAndIntercept<T>(this HttpClient client)
        {

            
            return new HttpResponse<T>();
        }
    }
}
