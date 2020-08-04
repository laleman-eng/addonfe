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

        private static SAPbouiCOM.Form oForm = null;

        public WebAppForm(string title, int width, int height, string url)
        {
            oForm.Title = title;
            oForm.Width = width;
            oForm.Height = height;
            CargarWeb(url, width, height);
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

        private void CargarWeb(string url, int width, int height)
        {
            SAPbouiCOM.Item oItem = this.UIAPIRawForm.Items.Add("WebBrowser", SAPbouiCOM.BoFormItemTypes.it_WEB_BROWSER);
            oItem.Left = 10;
            oItem.Top = 10;
            oItem.Width = width;
            oItem.Height = height;
            SAPbouiCOM.WebBrowser oWebBrouser = ((SAPbouiCOM.WebBrowser)(oItem.Specific));
            oWebBrouser.Url = url;
        }

      

        private void OnCustomInitialize()
        {
            oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);
        }
    }
}
