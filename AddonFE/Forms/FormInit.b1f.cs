using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace AddonFE
{
    [FormAttribute("AddonFE.Forms.FormInit", "Forms/FormInit.b1f")]
    public class FormInit : UserFormBase
    {

        public SAPbouiCOM.Form oForm;
        public SAPbouiCOM.EditText oEditText;
        public SAPbouiCOM.LinkedButton oLinkedButton;
        private string classid = "FormInit";
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.Button Button0;

        public FormInit()
        {

        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_1").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_2").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {


        }



        private void OnCustomInitialize()
        {
            try
            {
                oForm = Application.SBO_Application.Forms.Item(this.UIAPIRawForm.UniqueID);
                oForm.Items.Add("oET01", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oForm.Items.Item("oET01").Top = 20;
                oForm.Items.Item("oET01").Left = 100;
                oForm.Items.Item("oET01").Width = 50;

                SAPbouiCOM.Item oLinkItem = oForm.Items.Add("oLB01", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
                oLinkItem.Left = 80;
                oLinkItem.Top = 20;

                oLinkItem.LinkTo = "oET01";
                oLinkedButton = oLinkItem.Specific as SAPbouiCOM.LinkedButton;
                oLinkedButton.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_BusinessPartner;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(classid + ": " + ex.Message);
            }

        }

        public void LevantarVentaSAP(string code, string objectSap)
        {
            try
            {
                oForm.Settings.Enabled = false;
                SAPbouiCOM.Item oItemLinkedButtton = oForm.Items.Item("oLB01");
                SAPbouiCOM.Item oItemEditText = oForm.Items.Item("oET01");
                oEditText = (SAPbouiCOM.EditText)oItemEditText.Specific;
                oEditText.Value = code;
                oEditText.Active = false;
                oLinkedButton = (SAPbouiCOM.LinkedButton)oItemLinkedButtton.Specific;
                oLinkedButton.LinkedObject = ReturnObjectSAP(objectSap);
                oItemLinkedButtton.Click();
                oForm.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(classid + ": " + ex.Message);
            }
        }


        public SAPbouiCOM.BoLinkedObject ReturnObjectSAP(string objectSap)
        {
            SAPbouiCOM.BoLinkedObject obolinkedObject = SAPbouiCOM.BoLinkedObject.lf_BusinessPartner;

            switch (objectSap)
            {
                case "BusinessPartner":
                    {
                        obolinkedObject = SAPbouiCOM.BoLinkedObject.lf_BusinessPartner;
                        break;
                    }
                case "Invoice":
                    {
                        obolinkedObject = SAPbouiCOM.BoLinkedObject.lf_Invoice;
                        break;
                    }
                case "CreditMemo":
                    {
                        obolinkedObject = SAPbouiCOM.BoLinkedObject.lf_InvoiceCreditMemo;
                        break;
                    }


            }
            
            return obolinkedObject;


        }

    }




}

