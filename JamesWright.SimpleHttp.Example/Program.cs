using JamesWright.SimpleHttp;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Web;
using System.IO;
using System.Net;
using Server;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using JamesWright.SimpleHttp.Example.HTML;
using messagesClasses;




namespace JamesWright.SimpleHttp.Example
{
    class Program
    {
        private const string SUCCESS = "success";

        static void Main(string[] args)
        {
            App app = new App();
            IServer server = new ServerImpl();
            /**********404 page**************/
            #region 404
            app.Get("/404", async (req, res) =>
            {
                res.Content = "<h1>404 page not found</h1>";
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            /*         404 page           */
            #endregion

            /**********home page**************/
            #region home page
            app.Get("/home", async (req, res) =>
            {
                await req.GetBodyAsync();
                string html = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"..\\..\\HTML\homepage.html"));
                res.Content = html;
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            #endregion

            #region main page
            app.Get("/main", async (req, res) =>
            {
                /*Cookie[] cookies = req.getCookis();
                if (cookies.Length > 0)
                {
                    // Console.WriteLine("cookie in get method: " + cookies[0]);
                }*/
                await req.GetBodyAsync();
                string html = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"..\\..\\HTML\userMain.html"));
                res.Content = html;
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            #endregion

            #region question demonstration page
            app.Get("/question", async (req, res) =>
            {
                await req.GetBodyAsync();
                string html = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"..\\..\\HTML\question.html"));
                res.Content = html;
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            #endregion

            /**********register page**************/
            #region register page
            app.Get("/register", async (req, res) =>
            {
                string html = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"..\\..\\HTML\registerpage.html"));
                res.Content = html;
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            #endregion

            /**********java script lib for autocomplete**************/
            #region tokenize
            app.Get("/jquery.tokenize.js", async (req, res) =>
            {
                string html = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"..\\..\\JS\jquery.tokenize.js"));
                res.Content = html;
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            #endregion

            /**********CSS for JS lib for autocomplete**************/
            #region tokenize css
            app.Get("/jquery.tokenize.css", async (req, res) =>
            {
                string html = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"..\\..\\CSS\jquery.tokenize.css"));
                res.Content = html;
                //res.Content = "<p>You did a GET HOME.</p>";
                res.ContentType = "text/css";
                await res.SendAsync();
            });
            #endregion

            /**********java script lib for home page**************/
            #region home page lib
            app.Get("/loginpagescript.js", async (req, res) =>
            {
                string html = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"..\\..\\JS\loginScript.js"));
                res.Content = html;
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            #endregion

            /**********java script lib for register page**************/
            #region register page lib
            app.Get("/registerScript.js", async (req, res) =>
            {
                string html = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"..\\..\\JS\registerScript.js"));
                res.Content = html;
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            #endregion

            /***********java script lib for questions pages*************/
            #region question page lib
            app.Get("/questionsScript.js", async (req, res) =>
            {
                string html = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, @"..\\..\\JS\questionsScript.js"));
                res.Content = html;
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            #endregion

            #region login POST
            app.Post("/login", async (req, res) =>
            {
                string body = await req.GetBodyAsync();
                res.ContentType = "text/plain";
                res.Content = "default";

                JavaScriptSerializer oJS = new JavaScriptSerializer();
                loginData ans1 = new loginData();
                ans1 = oJS.Deserialize<loginData>(body);
                res.ContentType = "text/plain";
                res.Content = "default";

                Tuple<string,int> ans = server.login(ans1.mail, ans1.password);
                if (ans.Item1.Contains(SUCCESS))
                {
                    string sID = cryptor.Encode(ans.Item2);
                    res.addCookie("sID", sID);
                    res.addHeader("answer_status:success");
                    await res.SendAsync();
                }
                else
                {
                    res.addHeader("error_message:" + ans);
                    res.addHeader("answer_status:error");
                    await res.SendAsync();
                }
            });
            #endregion

            #region ans POST
            app.Post("/ans", async (req, res) =>
            {
                string body = await req.GetBodyAsync();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                answerData ansData = new answerData();
                ansData = jss.Deserialize<answerData>(body);
                res.ContentType = "text/plain";
                res.Content = "default";
               
                if (ansData.isDataReasonable().Contains("success"))
                {
                    string qID = req.getCookie("qID");
                    string ans = "";
                    if (qID.Equals("null"))
                    {
                        ans = "error, question id is invalid";
                    }
                    else
                    {
                        ans = server.AnswerAQuestion(cryptor.Decode(req.getSessionID()), Convert.ToInt32(req.getCookie("qID")), 
                            ansData.normalOrNot, ansData.sure1, ansData.diagnosis, ansData.sure2);
                    }
                    //string ans = SUCCESS;
                    if (ans.Contains(SUCCESS))
                    {
                        res.addHeader("answer_status:success");
                        await res.SendAsync();
                    }
                    else
                    {
                        res.addHeader("error_message:" + ans);
                        res.addHeader("answer_status:error");
                        await res.SendAsync();
                    }
                }
                else
                {
                    res.addHeader(ansData.isDataReasonable());
                    res.addHeader("answer_status:error");
                    await res.SendAsync();
                }
            });
            #endregion

            #region forgot password POST
            app.Post("/forgotpass", async (req, res) =>
            {
                string body = await req.GetBodyAsync();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                forgotPasswordData forgotPassData = new forgotPasswordData();
                forgotPassData = jss.Deserialize<forgotPasswordData>(body);
                res.ContentType = "text/plain";
                res.Content = "deafault";
                if (body == null)
                { //write error return

                }
                if (forgotPassData.mail.Length == 0)
                {
                    res.addHeader("error_message:error, missing data");
                    res.addHeader("answer_status:error");
                    await res.SendAsync();
                }
                else
                {
                    string ans = server.restorePassword(forgotPassData.mail);
                    if (ans.Contains(SUCCESS))
                    {
                        res.addHeader("answer_status:success");
                        await res.SendAsync();
                    }
                    else
                    {
                        res.addHeader("error_message:" + ans);
                        res.addHeader("answer_status:error");
                        await res.SendAsync();
                    }
                }


            });

            #endregion

            #region register POST
            app.Post("/register", async (req, res) =>
            {
                string body = await req.GetBodyAsync();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                registerData regData = new registerData();
                regData = jss.Deserialize<registerData>(body);
                res.ContentType = "text/plain";
                res.Content = "deafault";
                if (body == null)
                { //write error return

                }

                Tuple<string,int> ans = server.register(regData.mail, regData.password, regData.medTraining, regData.fname, regData.lname);
                if (ans.Item1.Contains(SUCCESS))
                {
                    string sID = cryptor.Encode(ans.Item2);
                    res.addCookie("sID", sID);
                    res.addHeader("answer_status:success");
                    await res.SendAsync();
                }
                else
                {
                    res.addHeader("error_message:" + ans);
                    res.addHeader("answer_status:error");
                    await res.SendAsync();
                }
            });
            #endregion

            #region dynamic code
            app.Post("/dinami", async (req, res) =>
            {
                string temp = await req.GetBodyAsync();

                /* Console.WriteLine("paramaters:");
                 Console.WriteLine(str);
                 Console.WriteLine("body:");
                 Console.WriteLine(temp);*/


                string[] arr = temp.Split('&');
                string[] arr1 = arr[0].Split('=');
                string[] arr2 = arr[1].Split('=');
                Console.WriteLine(arr1[1]);
                Console.WriteLine(arr2[1]);

                app.Get("/" + arr1[1], async (req1, res1) =>
                {
                    res1.Content = "<p>You did a GET.<br>your second word was " + arr2[1] + "</p>";
                    res1.ContentType = "text/html";
                    await res1.SendAsync();
                });

                //Console.WriteLine("this is what i got: {0}", temp);
                string html = File.ReadAllText(@"C:\Users\roie dominitz\Desktop\ניסויים\simple http server\simple-http-develop\JamesWright.SimpleHttp.Example\HTMLPage2.html");
                //res.Content = html;
                res.Content = "aaaaaaaaaaa";
                //res.Content = "<p>You did a GET HOME.</p>";
                res.ContentType = "text/plain";
                await res.SendAsync();
            });

            #endregion

            #region others
            app.Get("/img.img", async (req, res) =>
            {
                string location = Path.Combine(Environment.CurrentDirectory, @"..\\..\\IMG\img.jpg");
                res.Content = location;
                res.ContentType = "image/jpeg";
                await res.SendAsync();
            });


            app.Put("/", async (req, res) =>
            {
                res.Content = "<p>You did a PUT: " + await req.GetBodyAsync() + "</p>";
                res.ContentType = "text/html";
                await res.SendAsync();
            });

            app.Delete("/", async (req, res) =>
            {
                res.Content = "<p>You did a DELETE: " + await req.GetBodyAsync() + "</p>";
                res.ContentType = "text/html";
                await res.SendAsync();
            });
            #endregion

            server.register("a@a.com", "aa123", "aaa", "aa", "aa");
            app.Start();
        }
    }
}
