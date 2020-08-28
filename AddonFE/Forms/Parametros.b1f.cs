using System;
using System.Collections.Generic;
using System.Xml;
using SAPbouiCOM.Framework;

namespace AddonFE
{
    [FormAttribute("AddonFE.Parametros", "Forms/Parametros.b1f")]
    public class Parametros : UserFormBase
    {

        public SAPbouiCOM.Form oForm;
        public SAPbouiCOM.EditText oEditText;
        public SAPbouiCOM.LinkedButton oLinkedButton;
        private string classid = "Parametros";

        public Parametros()
        {

        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_2").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_4").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("Item_6").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("Item_7").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("Item_8").Specific));
            this.Button2.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.Button2_ClickAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {


        }

        private SAPbouiCOM.Button Button0;

        private void OnCustomInitialize()
        {
            try
            {
                string test = this.UIAPIRawForm.UniqueID;
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
                oLinkedButton.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_Invoice;


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }



        }

        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText EditText2;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;

        private void Button2_ClickAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                SAPbouiCOM.Item oItemLinkedButtton = oForm.Items.Item("oLB01");
                SAPbouiCOM.Item oItemEditText = oForm.Items.Item("oET01");
                oEditText = (SAPbouiCOM.EditText)oItemEditText.Specific;
                oEditText.Value = "P995695103";
                oEditText.Active = false;

                //P995695103
                oItemLinkedButtton.Click();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(classid + ": " + ex.Message);
            }

        }
    }
}