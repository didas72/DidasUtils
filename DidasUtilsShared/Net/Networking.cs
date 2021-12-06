using System.Net;

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
            bool ret = true;

            WebClient client = new WebClient();

            try
            {
                client.DownloadFile(url, path);
            }
            catch
            {
                ret = false;
            }

            client.Dispose();

            return ret;
        }



        /// <summary>
        /// Checks wether a given URL is accessible.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrlUp(string url)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);

            request.Method = "HEAD";

            try
            {
                request.GetResponse();
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
