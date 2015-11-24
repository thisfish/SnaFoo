using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SnaFoo.Data
{
    public class SnacksServiceHelper
    {
        static public string GetSnacks()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}?ApiKey={1}",ConfigurationManager.AppSettings["ApiUrl"], ConfigurationManager.AppSettings["ApiKey"]));
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                return "";
            }
        }

        static public string AddSnack(string suggestion)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}?ApiKey={1}", ConfigurationManager.AppSettings["ApiUrl"], ConfigurationManager.AppSettings["ApiKey"]));
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(suggestion);
                    streamWriter.Flush();
                    streamWriter.Close();
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                int code = 0;
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        code = (int)response.StatusCode;
                    }
                    else
                    {
                        // no http status code available
                    }
                }
                return code.ToString();
            }

        }
    }
}