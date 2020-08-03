﻿using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;
using System.IO;
using Newtonsoft.Json;
using AddonFE.Models;



namespace AddonFE
{
    class Menu
    {
        public void AddMenuItems()
        {

            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "AddonFE";
            oCreationPackage.String = "Factura Electronica";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;
           
            //string filePath = @"C:\icon.jpg";
            //oCreationPackage.Image = filePath;


            oMenus = oMenuItem.SubMenus;

            try
            {
                if (Application.SBO_Application.Menus.Exists(oCreationPackage.UniqueID))
                    Application.SBO_Application.Menus.RemoveEx(oCreationPackage.UniqueID);
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception e)
            {

            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("AddonFE");
                oMenus = oMenuItem.SubMenus;

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "AddonFE.FormParametros";
                oCreationPackage.String = "Parametros";
                oMenus.AddEx(oCreationPackage);

                //levante el naveador 
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "AddonFE.Navegador";
                oCreationPackage.String = "Portal EasyDoc";
                oMenus.AddEx(oCreationPackage);

                //levante el naveador 
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "AddonFE.FormInit";
                oCreationPackage.String = "FormInit";
                oMenus.AddEx(oCreationPackage);

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "AddonFE.Estado documentos enviados";
                oCreationPackage.String = "Estado documentos enviados";
                oMenus.AddEx(oCreationPackage);

                //string jsonMenuPath = @"menu.json";
                //using (StreamReader jsonStram = File.OpenText(jsonMenuPath))
                //{
                //    var json = jsonStram.ReadToEnd();
                //    //MenuModel menu = JsonConvert.DeserializeObject<MenuModel>(json);
                //    Root list = JsonConvert.DeserializeObject<Root>(json);

                //    foreach (var modulo in list.Modulo)
                //    {
                //        if (modulo.type == "string")
                //            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;

                //        oCreationPackage.UniqueID = modulo.UniqueID;
                //        oCreationPackage.String = modulo.String;
                //        oMenus.AddEx(oCreationPackage);
                //    }
                //}
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {


                if (pVal.BeforeAction && pVal.MenuUID == "AddonFE.FormParametros")
                {
                    Parametros activeForm = new Parametros();
                    activeForm.Show();
                }
                if (pVal.BeforeAction && pVal.MenuUID == "AddonFE.Navegador")
                {
                    WebAppForm webAppForm = new WebAppForm();
                    webAppForm.Show();
                }
                if (pVal.BeforeAction && pVal.MenuUID == "AddonFE.FormInit")
                {
                    FormInit formInit = new FormInit();
                    formInit.Show();
                }
                if (pVal.BeforeAction && pVal.MenuUID == "AddonFE.Estado documentos enviados")
                {
                    MonitorEstadoDocumentoEnviado monitor = new MonitorEstadoDocumentoEnviado("Estado documentos enviados", 1400, 800, "http://portal.easydoc.cl/index.aspx");
                    monitor.Show();
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }



    }
}
