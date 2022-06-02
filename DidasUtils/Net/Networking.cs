using System.IO;
using System.Net.Http;

namespace DidasUtils.Net
{
    /// <summary>
    /// Holds methods related to networking.
    /// </summary>
    public static class Networking
    {
        /// <summary>
        /// Attempts to download a file.
        /// </summary>
        /// <param name="url">The URL to downlaod from.</param>
        /// <param name="path">The path to store the file.</param>
        /// <returns>Boolean indicating the operation's success.</returns>
        public static bool TryDownloadFile(string url, string path)
        {
            HttpClient cli = new();
            HttpRequestMessage req = new(HttpMethod.Get, url);
            HttpResponseMessage resp = cli.Send(req);

            cli.Dispose();
            req.Dispose();

            if (!resp.IsSuccessStatusCode) return false;

            try
            {
                HttpContent cont = resp.Content;
                FileStream file = File.OpenWrite(path);
                cont.ReadAsStream().CopyTo(file);
                cont.Dispose();
                file.Close();
            }
            catch { return false; }

            resp.Dispose();

            return true;
        }
        /// <summary>
        /// Checks wether a given URL is accessible.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrlUp(string url)
        {
            HttpClient cli = new();
            HttpRequestMessage request = new(HttpMethod.Head, url);
            var resp = cli.Send(request);
            bool ret = resp.IsSuccessStatusCode;

            resp.Dispose();
            request.Dispose();
            cli.Dispose();

            return ret;
        }
    }
}
