using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using System.Xml;
using System.Net;
using System.IO;

namespace AddonFE
{
    public class SboJint
    {
        public int counter = 1;
        public Application oAplication;

        public int incremetar()
        {
            return counter++;
        }

        public string doJson(string query)
        {
            Conexion.Conectar_Aplicacion();
            SAPbobsCOM.Recordset oRecordSet = ((SAPbobsCOM.Recordset)Conexion.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset));

            oRecordSet.DoQuery(query);

            if (oRecordSet.RecordCount > 0)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(oRecordSet.GetAsXML());

                string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

                return json;
            }
            return "";
        }

        public SAPbobsCOM.Recordset doQuery(string query)
        {
            Conexion.Conectar_Aplicacion();
            SAPbobsCOM.Recordset oRecordSet = ((SAPbobsCOM.Recordset)Conexion.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset));
            oRecordSet.DoQuery(query);

            return oRecordSet;
        }

        public void testCount(object value)
        {
            SAPbobsCOM.Recordset oRecordSet = (SAPbobsCOM.Recordset)value;
            int a = oRecordSet.RecordCount;
        }

        private HttpWebRequest GetWebRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/text";
            return request;
        }

        private Stream GetResponseStream(HttpWebRequest request, string body = "")
        {
            if (body != "")
            {
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(body);
                requestWriter.Close();
            }
            try
            {
                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                return webStream;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GetResponse(string url, string body = "")
        {
            var result = "";
            try
            {
                var request = GetWebRequest(url);
                Stream webStream = GetResponseStream(request, body);
                StreamReader responseReader = new StreamReader(webStream);
                result = responseReader.ReadToEnd();
                responseReader.Close();
                return result;
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        public void levantarVentanaSAP(string docEntry, string objectSAP)
        {

            FormInit formInit = new FormInit();
            formInit.Show();
           // formInit.LevantarVentaSAP("1", "Invoice");

            formInit.LevantarVentaSAP(docEntry, objectSAP);

           // Application.SBO_Application.MessageBox(string.Format("Formlario: {0}, Numero Documento: {1}", form, DocEntry.ToString()));


        }

}
}
