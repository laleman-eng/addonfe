using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace AddonFE
{
    [FormAttribute("AddonFE.Forms.MonitorEstadoDocumentoEnviado", "Forms/MonitorEstadoDocumentoEnviado.b1f")]
    public class MonitorEstadoDocumentoEnviado : UserFormBase
    {
        private static SAPbouiCOM.Form oForm = null;
        
        public MonitorEstadoDocumentoEnviado(string title ,int width, int height, string url)
        {
            oForm.Title = title;
            oForm.Width = width;
            oForm.Height = height;
            oForm.MaxHeight = height;
            oForm.MaxWidth = width;
            CargarWeb(url);
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
            this.LoadBefore += new LoadBeforeHandler(this.Form_LoadBefore);

        }


        private void Form_LoadBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
          
        }


        private void OnCustomInitialize()
        {
            oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);
        }



        private void CargarWeb(string url)
        {
            SAPbouiCOM.Item oItem = this.UIAPIRawForm.Items.Add("WebBrowser", SAPbouiCOM.BoFormItemTypes.it_WEB_BROWSER);
            oItem.Left = 10;
            oItem.Top = 10;
            oItem.Width = 1400;
            oItem.Height = 800;
            SAPbouiCOM.WebBrowser oWebBrouser = ((SAPbouiCOM.WebBrowser)(oItem.Specific));
            oWebBrouser.Url = url;
        }

    }
}
