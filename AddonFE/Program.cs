using System;
using System.Collections.Generic;
using SAPbouiCOM.Framework;
using Jint;
using System.Data;
using System.Xml;
using System.Net;
using System.IO;

namespace AddonFE
{
 
    public class Conexion
    {
        public static SAPbouiCOM.Application oApplication;
        public static SAPbobsCOM.Company oCompany;
        public static SAPbobsCOM.SBObob oSBObob;

        public static string sCodUsuActual;
        public static string sAliasUsuActual;
        public static string sNomUsuActual;
        public static string sCurrentCompanyDB;
       
        public static void Conectar_Aplicacion()
        {


            SAPbouiCOM.SboGuiApi SboGuiApi = new SAPbouiCOM.SboGuiApi();
            oCompany = new SAPbobsCOM.Company();
            SboGuiApi.Connect("0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056");
            oApplication = SboGuiApi.GetApplication();
            oCompany = (SAPbobsCOM.Company)oApplication.Company.GetDICompany();

            oSBObob = (SAPbobsCOM.SBObob)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);

            sCodUsuActual = oCompany.UserSignature.ToString();
            //sNomUsuActual = Funciones.Current_User_Name();
            sAliasUsuActual = oCompany.UserName;
            sCurrentCompanyDB = oCompany.CompanyDB;
        }
    }

  public  class Program
  {
        static Webserver ws;
        static int contador = 0;
        public static string SendResponse(HttpListenerRequest request)
        {
            string responseString = "<HTML><BODY> Estoy activo!</BODY></HTML>";
            var engine = new Engine(cfg => cfg.AllowClr());
            string script = "";
            if (request.HasEntityBody)
            {
                System.IO.Stream body = request.InputStream;
                System.Text.Encoding encoding = request.ContentEncoding;
                System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding) { };
                script = reader.ReadToEnd();
                body.Close();
                reader.Close();
                reader.Dispose();
            }
            if (script == "")
            {
                script = @"
                    var file = new System.IO.StreamWriter('log.txt');
                    file.WriteLine('Hello World !');
                    file.Dispose();
                    (x * x).toString();
                ";
                engine.SetValue("x", 3);
            }
            var p = new SboJint();
            engine.SetValue("sbo", p);
            engine.Execute(script);
            responseString = engine.GetCompletionValue().AsString();
            return responseString;
        }

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application oApp = null;
                if (args.Length < 1)
                {
                    oApp = new Application();
                }
                else
                {
                    oApp = new Application(args[0]);
                }

                Menu MyMenu = new Menu();
                MyMenu.AddMenuItems();
                oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);

                var engine = new Jint.Engine(cfg => cfg.AllowClr());
                var p = new SboJint();

                //p.incremetar();
                string query = @"SELECT |DocEntry| , |DocNum| FROM |OINV| WHERE |DocNum| = 2006704";
                query = query.Replace("|", "\"");
                SAPbobsCOM.Recordset oRs = p.doQuery(query);
                int a = oRs.RecordCount;

                engine.SetValue("sbo", p);

                string script = @"
                var result;
                function execute()
                {
                    var query ='SELECT |DocEntry|,|DocNum| FROM |OINV| WHERE |DocNum| = 2006704';   
                    var rs = sbo.doJson(query);  
                    result = rs;
                    return result;
                }";
                script = script.Replace("|", "\"");

                engine.Execute(script);
                Jint.Native.JsValue v = engine.Invoke("execute");
                var result = engine.GetValue("result");
                //Jint.Native.JsValue v = engine.GetCompletionValue();

                // Console.WriteLine(engine.Execute("2+2").GetCompletionValue());

                Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);

                Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);

                //habilitacion del Webserver
                string prefix = "http://+:50200/";
                var ws = new Webserver(SendResponse, prefix);
                ws.Run();

                script = @"
                    var file = new System.IO.StreamWriter('log.txt');
                    file.WriteLine('Hello World !');
                    file.Dispose();
                    (3 * 3).toString();
                ";

                var url = "http://localhost:50200";
                var test2 = p.GetResponse(url, script);

                //validarJson();

                oApp.Run();
                System.Windows.Forms.MessageBox.Show("test");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    ws.Stop();
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    ws.Stop();
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    ws.Stop();
                    System.Windows.Forms.Application.Exit();
                    break;

                default:
                    break;
            }
        }

        static void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            SAPbouiCOM.Form oForm = null;
            SAPbouiCOM.EditText oEdit = null;
            SAPbouiCOM.DataTable oDataTable = null;

            try
            {
                contador++;
                if (contador ==1)
                {
                    var sboJint = new SboJint();
                    sboJint.levantarVentanaSAP("form1", 2);
                }

            }

            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }


        static void validarJson()
        {
            string json = "{\"item\":1}";
            var engine = new Jint.Engine(cfg => cfg.AllowClr());
            engine.SetValue("items", json);
            var p = new SboJint();
            engine.SetValue("sbo", p);
            string script = System.IO.File.ReadAllText("script/validar.jint");
            try
            {
                engine.Execute(script);
                Jint.Native.JsValue v = engine.Invoke("execute");
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
            var result = engine.GetValue("result");
        }

    }
}
