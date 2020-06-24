using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

namespace AddonFE
{
    public class Webserver
    {
        private readonly HttpListener oListener = new HttpListener();
        private readonly Func<HttpListenerRequest, string> oResponderMethod;

        public Webserver(IReadOnlyCollection<string> prefixes, Func<HttpListenerRequest, string> method)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException("Sistema operativo no soportado");
            }

            if (prefixes == null || prefixes.Count == 0)
            {
                throw new ArgumentException("URI prefixes son requeridos");
            }

            if (method == null)
            {
                throw new ArgumentException("responder method requerido");
            }

            foreach (var s in prefixes)
            {
                oListener.Prefixes.Add(s);
            }

            oResponderMethod = method;
            oListener.Start();
        }

        public Webserver(Func<HttpListenerRequest, string> method, params string[] prefixes)
         : this(prefixes, method)
         {
         }

        public void Run()
        { 
            ThreadPool.QueueUserWorkItem(o =>
            {
                Console.WriteLine("Servidor web esta corriendo...");
                try
                {
                    while (oListener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem(c =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                if (ctx == null)
                                {
                                    return;
                                }

                                var rstr = oResponderMethod(ctx.Request);
                                var buf = Encoding.UTF8.GetBytes(rstr);

                                ctx.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                                ctx.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                                ctx.Response.AddHeader("Access-Control-Max-Age", "1728000");

                                ctx.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                            finally
                            {
                                // siempre cerrar el stream
                                if (ctx != null)
                                {
                                    ctx.Response.OutputStream.Close();
                                }
                            }
                        }, oListener.GetContext());
                    }
                }
                catch (Exception ex)
                {
                    // ignorada
                }
            });
        }

        public void Stop()
        {
            oListener.Stop();
            oListener.Close();
        }

    }
}
