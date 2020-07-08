using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace AddonFE.Forms
{
    [FormAttribute("133", "Forms/FacturaDeudores.b1f")]
    class SystemForm2 : SystemFormBase
    {
        private static SAPbouiCOM.Form oForm = null;
        private static SAPbobsCOM.Company oCompany = Conexion.oCompany;
        private SAPbobsCOM.Documents oDocument;
        private static string classid = "Factura Deudores";
        
        public SystemForm2()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataAddAfter += new DataAddAfterHandler(this.Form_DataAddAfter);
            this.DataAddBefore += new DataAddBeforeHandler(this.Form_DataAddBefore);

        }

        private void Form_DataAddAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            try
            {

                var json = Newtonsoft.Json.Linq.JObject.Parse("{}");

                SAPbouiCOM.DBDataSource oDBDSOINV = oForm.DataSources.DBDataSources.Item("OINV");
                SAPbouiCOM.DBDataSource oDBDSINV1 = oForm.DataSources.DBDataSources.Item("INV1");

                XmlDocument xmlOINV = new XmlDocument();
                xmlOINV.LoadXml(oDBDSOINV.GetAsXML());

                XmlDocument xmlINV1 = new XmlDocument();
                xmlINV1.LoadXml(oDBDSINV1.GetAsXML());

                json["OINV"] = Newtonsoft.Json.Linq.JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlOINV));
                json["INV1"] = Newtonsoft.Json.Linq.JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlINV1));

                var jsonFactura = Newtonsoft.Json.JsonConvert.SerializeObject(json);

                var result = enviarJsonFactura(jsonFactura);

                if (result.ToLower().StartsWith("error"))
                {
                    Application.SBO_Application.StatusBar.SetText(" " + result, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    Application.SBO_Application.MessageBox(result);
                    //analizar el mecanismo de mensaje hacia el usuario y soporte
                }
                else
                {
                    //objecto DocEntry 
                    //asigno el folio 
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(classid + ": " + ex.Message);
            }
        }

        private void OnCustomInitialize()
        {
            oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);
        }

        private void Form_DataAddBefore(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                var json = Newtonsoft.Json.Linq.JObject.Parse("{}");

                SAPbouiCOM.DBDataSource oDBDSOINV = oForm.DataSources.DBDataSources.Item("OINV");
                SAPbouiCOM.DBDataSource oDBDSINV1 = oForm.DataSources.DBDataSources.Item("INV1");
                SAPbouiCOM.DBDataSource oDBDSOCRD = oForm.DataSources.DBDataSources.Item("OCRD");

                //oDBDSOINV.Query("SELECT * FROM OINV");
                //SapDataTableToDotNetDataTable(oDBDSOINV.GetAsXML());

                XmlDocument xmlOINV = new XmlDocument();
                xmlOINV.LoadXml(oDBDSOINV.GetAsXML());

                xmlOINV.Save("C:/factura.xml");

                XmlDocument xmlINV1 = new XmlDocument();
                xmlINV1.LoadXml(oDBDSINV1.GetAsXML());

                XmlDocument xmlOCRD = new XmlDocument();
                xmlOCRD.LoadXml(oDBDSOCRD.GetAsXML());


                json["OINV"] = Newtonsoft.Json.Linq.JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlOINV));
                json["OCRD"] = Newtonsoft.Json.Linq.JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlOCRD));
                json["INV1"] = Newtonsoft.Json.Linq.JObject.Parse(Newtonsoft.Json.JsonConvert.SerializeXmlNode(xmlINV1));

                var jsonFactura = Newtonsoft.Json.JsonConvert.SerializeObject(json);

                var result = validarJsonFactura(jsonFactura);

                if (result.ToLower().StartsWith("error"))
                {
                    Application.SBO_Application.StatusBar.SetText(result, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    Application.SBO_Application.MessageBox(result);
                    BubbleEvent = false;
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(classid + ": " + ex.Message);
                BubbleEvent = false;
            }
        }


        private string validarJsonFactura(string json)
        {
            Jint.Native.JsValue result = null;
            var engine = new Jint.Engine(cfg => cfg.AllowClr());
            engine.SetValue("jsonString", json);
            var p = new SboJint();
            engine.SetValue("sbo", p);
            try
            {
                string script = System.IO.File.ReadAllText("script/ValFacturaApirestFE3.jint");
           
                engine.Execute(script);
                result = engine.Invoke("execute");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(classid + ": " + ex.Message);
            }

            return result.ToString();
        }


        private string enviarJsonFactura(string json)
        {
            Jint.Native.JsValue result = null;
            var engine = new Jint.Engine(cfg => cfg.AllowClr());
            engine.SetValue("jsonString", json);
            var p = new SboJint();
            engine.SetValue("sbo", p);
            string script = System.IO.File.ReadAllText("script/val33DC.jint");
            try
            {
                engine.Execute(script);
                result = engine.Invoke("execute");
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return result.ToString();
        }


        public DataTable SapDataTableToDotNetDataTable(string xmlSap)
        {
            var DT = new DataTable();

            //var XMLstream = new System.IO.FileStream(pathToXmlFile, FileMode.Open);

            var XDoc = System.Xml.Linq.XDocument.Load(xmlSap);

            var Columns = XDoc.Element("DataTable").Element("Columns").Elements("Column");

            foreach (var Column in Columns)
            {
                DT.Columns.Add(Column.Attribute("Uid").Value);
            }

            var Rows = XDoc.Element("DataTable").Element("Rows").Elements("Row");

            var Names = new List<string>();
            foreach (var Row in Rows)
            {
                var DTRow = DT.NewRow();

                var Cells = Row.Element("Cells").Elements("Cell");
                foreach (var Cell in Cells)
                {
                    var ColName = Cell.Element("ColumnUid").Value;
                    var ColValue = Cell.Element("Value").Value;
                    DTRow[ColName] = ColValue;
                }

                DT.Rows.Add(DTRow);
            }

            return DT;
        }




    }
}
