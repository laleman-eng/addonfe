using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace AddonFE
{
    [FormAttribute("AddonFE.WebAppForm", "Forms/WebAppForm.b1f")]
    class WebAppForm : UserFormBase
    {
        public WebAppForm()
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
            this.LoadBefore += new LoadBeforeHandler(this.Form_LoadBefore);

        }

        private void Form_LoadBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            CargarWeb();
        }

        private void CargarWeb()
        {
            SAPbouiCOM.Item oItem = this.UIAPIRawForm.Items.Add("WebBrowser", SAPbouiCOM.BoFormItemTypes.it_WEB_BROWSER);
            oItem.Left = 10;
            oItem.Top = 10;
            oItem.Width = 1400;
            oItem.Height = 800;
            SAPbouiCOM.WebBrowser oWebBrouser = ((SAPbouiCOM.WebBrowser)(oItem.Specific));
            oWebBrouser.Url = "http://portal.easydoc.cl/index.aspx";
        }

      

        private void OnCustomInitialize()
        {

        }
    }
}
