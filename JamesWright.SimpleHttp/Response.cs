using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace JamesWright.SimpleHttp
{
    public class Response
    {
        private HttpListenerResponse httpListenerResponse;

        public string Content { get; set; }
        public string ContentType { get; set; }

        public Image img { get; set; }

        internal Response(HttpListenerResponse httpListenerResponse)
        {
            this.httpListenerResponse = httpListenerResponse;
        }

        public void addHeader(string head)
        {
            this.httpListenerResponse.Headers.Add(head);
        }

        public void addCookie(string name, string value)
        {
            Cookie cookie = new Cookie();
            cookie.Name = name;
            cookie.Value = value;
            httpListenerResponse.Cookies.Add(cookie);
        }

        public async Task SendAsync()
        {
            byte[] responseBuffer = Encoding.UTF8.GetBytes(Content);
            this.httpListenerResponse.ContentType = ContentType;
           

            if (ContentType.Equals("image/jpeg"))
            {
                img = Image.FromFile(Content);
                byte[] arr;
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    arr = ms.ToArray();
                }
                using (Stream output = this.httpListenerResponse.OutputStream)
                {
                    await output.WriteAsync(arr, 0, arr.Length);
                }
                
            }
            else { 
            if (this.httpListenerResponse.ContentLength64 == 0)
                this.httpListenerResponse.ContentLength64 = responseBuffer.Length;
/*
            httpListenerResponse.StatusCode = (int)HttpStatusCode.NotFound;
            httpListenerResponse.ContentType = "application/javascript";
*/
            // test code here
        /*    if (this.httpListenerResponse.ContentType == "message/http")
            {
                httpListenerResponse.StatusCode = (int)HttpStatusCode.NoContent;
                
            }*/
           // Console.WriteLine(this.httpListenerResponse.ContentType);
             //this.httpListenerResponse.Headers.Add("answer_status:roie dominitz");
            /*Cookie cookie = new Cookie();
            cookie.Name = "test_cookie";
            cookie.Value = "test_value";
            httpListenerResponse.Cookies.Add(cookie);*/
            using (Stream output = this.httpListenerResponse.OutputStream)
            { 
                await output.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                
            }

            Console.WriteLine("{0}: Responded to request with {1} bytes of data.", DateTime.Now, responseBuffer.Length);
            this.httpListenerResponse.Close();
            }
        }
    }
}
