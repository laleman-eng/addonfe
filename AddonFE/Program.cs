using System;
using System.Collections.Generic;
using SAPbouiCOM.Framework;
using Jint;
using System.Data;
using System.Xml;
using System.Net;
using System.IO;
using SBOFunctions;
using Newtonsoft.Json;
using AddonFE.Models;

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
        private static string classid = "Program";

        public static SAPbouiCOM.Application SBO_Application = null;
        public static SAPbobsCOM.Company oCompany = null;

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
                //                script = @"sbo.levantarVentanaSAP('1','Invoice')                  
                //                ";
            }
            else
            {
                try
                {
                    var p = new SboJint();
                    engine.SetValue("sbo", p);
                    engine.Execute(script);
                    responseString = engine.GetCompletionValue().AsString();
                }
                catch (Exception ex)
                {
                    responseString = "error: " + ex.Message;
                }
                
            }

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

                //string jsonSAP = "{\"OINV\":{\"xml\":{\"version\":\"1.0\",\"encoding\":\"UTF-16\"},\"dbDataSources\":{\"uid\":\"OINV\",\"rows\":{\"row\":{\"cells\":{\"cell\":[{\"uid\":\"DocNum\",\"value\":\"2003209\"},{\"uid\":\"DocType\",\"value\":\"I\"},{\"uid\":\"CANCELED\",\"value\":\"N\"},{\"uid\":\"Handwrtten\",\"value\":\"N\"},{\"uid\":\"Printed\",\"value\":\"N\"},{\"uid\":\"DocStatus\",\"value\":\"O\"},{\"uid\":\"InvntSttus\",\"value\":\"O\"},{\"uid\":\"Transfered\",\"value\":\"N\"},{\"uid\":\"ObjType\",\"value\":\"13\"},{\"uid\":\"DocDate\",\"value\":\"20200706\"},{\"uid\":\"DocDueDate\",\"value\":\"20200706\"},{\"uid\":\"CardCode\",\"value\":\"C010534722\"},{\"uid\":\"CardName\",\"value\":\"FREDY OROZCO GARCIA\"},{\"uid\":\"Address\",\"value\":\"CALLE 9C  5025 OF 709  COLOMBIACOLOMBIA\"},{\"uid\":\"NumAtCard\",\"value\":\"\"},{\"uid\":\"VatPercent\",\"value\":\"0.0\"},{\"uid\":\"VatSum\",\"value\":\"10.000000\"},{\"uid\":\"VatSumFC\",\"value\":\"0.0\"},{\"uid\":\"DiscPrcnt\",\"value\":\"0.0\"},{\"uid\":\"DiscSum\",\"value\":\"0.0\"},{\"uid\":\"DiscSumFC\",\"value\":\"0.0\"},{\"uid\":\"DocCur\",\"value\":\"$\"},{\"uid\":\"DocRate\",\"value\":\"1.000000\"},{\"uid\":\"DocTotal\",\"value\":\"60.000000\"},{\"uid\":\"DocTotalFC\",\"value\":\"0.0\"},{\"uid\":\"PaidToDate\",\"value\":\"0.0\"},{\"uid\":\"PaidFC\",\"value\":\"0.0\"},{\"uid\":\"GrosProfit\",\"value\":\"50.000000\"},{\"uid\":\"GrosProfFC\",\"value\":\"0.0\"},{\"uid\":\"Ref1\",\"value\":\"2003208\"},{\"uid\":\"Comments\",\"value\":\"\"},{\"uid\":\"JrnlMemo\",\"value\":\"Facturas clientes - C010534722\"},{\"uid\":\"GroupNum\",\"value\":\"5\"},{\"uid\":\"SlpCode\",\"value\":\"9\"},{\"uid\":\"TrnspCode\",\"value\":\"-1\"},{\"uid\":\"PartSupply\",\"value\":\"Y\"},{\"uid\":\"Confirmed\",\"value\":\"Y\"},{\"uid\":\"GrossBase\",\"value\":\"-5\"},{\"uid\":\"ImportEnt\",\"value\":\"0\"},{\"uid\":\"CreateTran\",\"value\":\"Y\"},{\"uid\":\"SummryType\",\"value\":\"N\"},{\"uid\":\"UpdInvnt\",\"value\":\"I\"},{\"uid\":\"UpdCardBal\",\"value\":\"B\"},{\"uid\":\"Instance\",\"value\":\"0\"},{\"uid\":\"Flags\",\"value\":\"0\"},{\"uid\":\"InvntDirec\",\"value\":\"X\"},{\"uid\":\"CntctCode\",\"value\":\"0\"},{\"uid\":\"ShowSCN\",\"value\":\"N\"},{\"uid\":\"SysRate\",\"value\":\"860.000000\"},{\"uid\":\"CurSource\",\"value\":\"L\"},{\"uid\":\"VatSumSy\",\"value\":\"0.0\"},{\"uid\":\"DiscSumSy\",\"value\":\"0.0\"},{\"uid\":\"DocTotalSy\",\"value\":\"0.0\"},{\"uid\":\"PaidSys\",\"value\":\"0.0\"},{\"uid\":\"FatherType\",\"value\":\"P\"},{\"uid\":\"GrosProfSy\",\"value\":\"0.0\"},{\"uid\":\"IsICT\",\"value\":\"N\"},{\"uid\":\"CreateDate\",\"value\":\"20200706\"},{\"uid\":\"Volume\",\"value\":\"0.0\"},{\"uid\":\"VolUnit\",\"value\":\"4\"},{\"uid\":\"Weight\",\"value\":\"0.0\"},{\"uid\":\"WeightUnit\",\"value\":\"3\"},{\"uid\":\"Series\",\"value\":\"50\"},{\"uid\":\"TaxDate\",\"value\":\"20200706\"},{\"uid\":\"DataSource\",\"value\":\"I\"},{\"uid\":\"StampNum\",\"value\":\"\"},{\"uid\":\"isCrin\",\"value\":\"N\"},{\"uid\":\"FinncPriod\",\"value\":\"84\"},{\"uid\":\"UserSign\",\"value\":\"1\"},{\"uid\":\"selfInv\",\"value\":\"N\"},{\"uid\":\"VatPaid\",\"value\":\"0.0\"},{\"uid\":\"VatPaidFC\",\"value\":\"0.0\"},{\"uid\":\"VatPaidSys\",\"value\":\"0.0\"},{\"uid\":\"UserSign2\",\"value\":\"1\"},{\"uid\":\"WddStatus\",\"value\":\"-\"},{\"uid\":\"draftKey\",\"value\":\"-1\"},{\"uid\":\"TotalExpns\",\"value\":\"0.0\"},{\"uid\":\"TotalExpFC\",\"value\":\"0.0\"},{\"uid\":\"TotalExpSC\",\"value\":\"0.0\"},{\"uid\":\"Address2\",\"value\":\"CALLE 9C  5025 OF 709  COLOMBIACOLOMBIA\"},{\"uid\":\"LogInstanc\",\"value\":\"0\"},{\"uid\":\"Exported\",\"value\":\"N\"},{\"uid\":\"StationID\",\"value\":\"82\"},{\"uid\":\"NetProc\",\"value\":\"N\"},{\"uid\":\"AqcsTax\",\"value\":\"0.0\"},{\"uid\":\"AqcsTaxFC\",\"value\":\"0.0\"},{\"uid\":\"AqcsTaxSC\",\"value\":\"0.0\"},{\"uid\":\"CashDiscPr\",\"value\":\"0.0\"},{\"uid\":\"CashDiscnt\",\"value\":\"0.0\"},{\"uid\":\"CashDiscFC\",\"value\":\"0.0\"},{\"uid\":\"CashDiscSC\",\"value\":\"0.0\"},{\"uid\":\"ShipToCode\",\"value\":\"DESPACHO\"},{\"uid\":\"LicTradNum\",\"value\":\"55555555-5\"},{\"uid\":\"PaymentRef\",\"value\":\"\"},{\"uid\":\"WTSum\",\"value\":\"0.0\"},{\"uid\":\"WTSumFC\",\"value\":\"0.0\"},{\"uid\":\"WTSumSC\",\"value\":\"0.0\"},{\"uid\":\"RoundDif\",\"value\":\"0.0\"},{\"uid\":\"RoundDifFC\",\"value\":\"0.0\"},{\"uid\":\"RoundDifSy\",\"value\":\"0.0\"},{\"uid\":\"submitted\",\"value\":\"N\"},{\"uid\":\"PoPrss\",\"value\":\"N\"},{\"uid\":\"Rounding\",\"value\":\"N\"},{\"uid\":\"RevisionPo\",\"value\":\"N\"},{\"uid\":\"Segment\",\"value\":\"0\"},{\"uid\":\"PickStatus\",\"value\":\"N\"},{\"uid\":\"Pick\",\"value\":\"N\"},{\"uid\":\"BlockDunn\",\"value\":\"N\"},{\"uid\":\"PeyMethod\",\"value\":\"\"},{\"uid\":\"PayBlock\",\"value\":\"N\"},{\"uid\":\"MaxDscn\",\"value\":\"N\"},{\"uid\":\"Reserve\",\"value\":\"N\"},{\"uid\":\"Max1099\",\"value\":\"60.000000\"},{\"uid\":\"PickRmrk\",\"value\":\"\"},{\"uid\":\"ExpAppl\",\"value\":\"0.0\"},{\"uid\":\"ExpApplFC\",\"value\":\"0.0\"},{\"uid\":\"ExpApplSC\",\"value\":\"0.0\"},{\"uid\":\"DeferrTax\",\"value\":\"N\"},{\"uid\":\"LetterNum\",\"value\":\"\"},{\"uid\":\"WTApplied\",\"value\":\"0.0\"},{\"uid\":\"WTAppliedF\",\"value\":\"0.0\"},{\"uid\":\"BoeReserev\",\"value\":\"N\"},{\"uid\":\"WTAppliedS\",\"value\":\"0.0\"},{\"uid\":\"EquVatSum\",\"value\":\"0.0\"},{\"uid\":\"EquVatSumF\",\"value\":\"0.0\"},{\"uid\":\"EquVatSumS\",\"value\":\"0.0\"},{\"uid\":\"Installmnt\",\"value\":\"1\"},{\"uid\":\"VATFirst\",\"value\":\"N\"},{\"uid\":\"NnSbAmnt\",\"value\":\"0.0\"},{\"uid\":\"NnSbAmntSC\",\"value\":\"0.0\"},{\"uid\":\"NbSbAmntFC\",\"value\":\"0.0\"},{\"uid\":\"ExepAmnt\",\"value\":\"0.0\"},{\"uid\":\"ExepAmntSC\",\"value\":\"0.0\"},{\"uid\":\"ExepAmntFC\",\"value\":\"0.0\"},{\"uid\":\"CEECFlag\",\"value\":\"N\"},{\"uid\":\"BaseAmnt\",\"value\":\"0.0\"},{\"uid\":\"BaseAmntSC\",\"value\":\"0.0\"},{\"uid\":\"BaseAmntFC\",\"value\":\"0.0\"},{\"uid\":\"CtlAccount\",\"value\":\"1-1-04-01-001\"},{\"uid\":\"SumAbsId\",\"value\":\"-1\"},{\"uid\":\"PIndicator\",\"value\":\"Valor de p\"},{\"uid\":\"UseShpdGd\",\"value\":\"N\"},{\"uid\":\"BaseVtAt\",\"value\":\"0.0\"},{\"uid\":\"BaseVtAtSC\",\"value\":\"0.0\"},{\"uid\":\"BaseVtAtFC\",\"value\":\"0.0\"},{\"uid\":\"NnSbVAt\",\"value\":\"0.0\"},{\"uid\":\"NnSbVAtSC\",\"value\":\"0.0\"},{\"uid\":\"NbSbVAtFC\",\"value\":\"0.0\"},{\"uid\":\"ExptVAt\",\"value\":\"0.0\"},{\"uid\":\"ExptVAtSC\",\"value\":\"0.0\"},{\"uid\":\"ExptVAtFC\",\"value\":\"0.0\"},{\"uid\":\"LYPmtAt\",\"value\":\"0.0\"},{\"uid\":\"LYPmtAtSC\",\"value\":\"0.0\"},{\"uid\":\"LYPmtAtFC\",\"value\":\"0.0\"},{\"uid\":\"ExpAnSum\",\"value\":\"0.0\"},{\"uid\":\"ExpAnSys\",\"value\":\"0.0\"},{\"uid\":\"ExpAnFrgn\",\"value\":\"0.0\"},{\"uid\":\"DocSubType\",\"value\":\"--\"},{\"uid\":\"DpmStatus\",\"value\":\"O\"},{\"uid\":\"DpmAmnt\",\"value\":\"0.0\"},{\"uid\":\"DpmAmntSC\",\"value\":\"0.0\"},{\"uid\":\"DpmAmntFC\",\"value\":\"0.0\"},{\"uid\":\"DpmDrawn\",\"value\":\"N\"},{\"uid\":\"DpmPrcnt\",\"value\":\"0.0\"},{\"uid\":\"PaidSum\",\"value\":\"0.0\"},{\"uid\":\"PaidSumFc\",\"value\":\"0.0\"},{\"uid\":\"PaidSumSc\",\"value\":\"0.0\"},{\"uid\":\"DpmAppl\",\"value\":\"0.0\"},{\"uid\":\"DpmApplFc\",\"value\":\"0.0\"},{\"uid\":\"DpmApplSc\",\"value\":\"0.0\"},{\"uid\":\"Posted\",\"value\":\"Y\"},{\"uid\":\"BPChCntc\",\"value\":\"0\"},{\"uid\":\"PayToCode\",\"value\":\"FACTURACION\"},{\"uid\":\"IsPaytoBnk\",\"value\":\"N\"},{\"uid\":\"isIns\",\"value\":\"N\"},{\"uid\":\"VersionNum\",\"value\":\"9.30.230.13\"},{\"uid\":\"LangCode\",\"value\":\"25\"},{\"uid\":\"BPNameOW\",\"value\":\"N\"},{\"uid\":\"BillToOW\",\"value\":\"N\"},{\"uid\":\"ShipToOW\",\"value\":\"N\"},{\"uid\":\"RetInvoice\",\"value\":\"N\"},{\"uid\":\"Model\",\"value\":\"0\"},{\"uid\":\"TaxOnExp\",\"value\":\"0.0\"},{\"uid\":\"TaxOnExpFc\",\"value\":\"0.0\"},{\"uid\":\"TaxOnExpSc\",\"value\":\"0.0\"},{\"uid\":\"TaxOnExAp\",\"value\":\"0.0\"},{\"uid\":\"TaxOnExApF\",\"value\":\"0.0\"},{\"uid\":\"TaxOnExApS\",\"value\":\"0.0\"},{\"uid\":\"LndCstNum\",\"value\":\"0\"},{\"uid\":\"UseCorrVat\",\"value\":\"N\"},{\"uid\":\"BlkCredMmo\",\"value\":\"N\"},{\"uid\":\"OpenForLaC\",\"value\":\"Y\"},{\"uid\":\"Excised\",\"value\":\"O\"},{\"uid\":\"SrvGpPrcnt\",\"value\":\"0.0\"},{\"uid\":\"DutyStatus\",\"value\":\"Y\"},{\"uid\":\"AutoCrtFlw\",\"value\":\"N\"},{\"uid\":\"VatJENum\",\"value\":\"-1\"},{\"uid\":\"DpmVat\",\"value\":\"0.0\"},{\"uid\":\"DpmVatFc\",\"value\":\"0.0\"},{\"uid\":\"DpmVatSc\",\"value\":\"0.0\"},{\"uid\":\"DpmAppVat\",\"value\":\"0.0\"},{\"uid\":\"DpmAppVatF\",\"value\":\"0.0\"},{\"uid\":\"DpmAppVatS\",\"value\":\"0.0\"},{\"uid\":\"InsurOp347\",\"value\":\"N\"},{\"uid\":\"IgnRelDoc\",\"value\":\"N\"},{\"uid\":\"BuildDesc\",\"value\":\"\"},{\"uid\":\"ResidenNum\",\"value\":\"1\"},{\"uid\":\"CopyNumber\",\"value\":\"0\"},{\"uid\":\"PQTGrpHW\",\"value\":\"N\"},{\"uid\":\"DocManClsd\",\"value\":\"N\"},{\"uid\":\"ClosingOpt\",\"value\":\"1\"},{\"uid\":\"Ordered\",\"value\":\"N\"},{\"uid\":\"NTSApprov\",\"value\":\"N\"},{\"uid\":\"PayDuMonth\",\"value\":\"N\"},{\"uid\":\"ExtraMonth\",\"value\":\"0\"},{\"uid\":\"ExtraDays\",\"value\":\"0\"},{\"uid\":\"CdcOffset\",\"value\":\"0\"},{\"uid\":\"EDocGenTyp\",\"value\":\"N\"},{\"uid\":\"OnlineQuo\",\"value\":\"N\"},{\"uid\":\"EDocStatus\",\"value\":\"C\"},{\"uid\":\"EDocProces\",\"value\":\"C\"},{\"uid\":\"EDocCancel\",\"value\":\"N\"},{\"uid\":\"EDocTest\",\"value\":\"N\"},{\"uid\":\"DpmAsDscnt\",\"value\":\"N\"},{\"uid\":\"GTSRlvnt\",\"value\":\"N\"},{\"uid\":\"BaseDisc\",\"value\":\"0.0\"},{\"uid\":\"BaseDiscSc\",\"value\":\"0.0\"},{\"uid\":\"BaseDiscFc\",\"value\":\"0.0\"},{\"uid\":\"BaseDiscPr\",\"value\":\"0.0\"},{\"uid\":\"CreateTS\",\"value\":\"123900\"},{\"uid\":\"UpdateTS\",\"value\":\"123901\"},{\"uid\":\"SrvTaxRule\",\"value\":\"N\"},{\"uid\":\"AssetDate\",\"value\":\"20200706\"},{\"uid\":\"ReqType\",\"value\":\"12\"},{\"uid\":\"OriginType\",\"value\":\"M\"},{\"uid\":\"IsReuseNum\",\"value\":\"N\"},{\"uid\":\"IsReuseNFN\",\"value\":\"N\"},{\"uid\":\"DocDlvry\",\"value\":\"0\"},{\"uid\":\"PaidDpm\",\"value\":\"0.0\"},{\"uid\":\"PaidDpmF\",\"value\":\"0.0\"},{\"uid\":\"PaidDpmS\",\"value\":\"0.0\"},{\"uid\":\"EnvTypeNFe\",\"value\":\"-1\"},{\"uid\":\"IsAlt\",\"value\":\"N\"},{\"uid\":\"AltBaseTyp\",\"value\":\"-1\"},{\"uid\":\"PrintSEPA\",\"value\":\"N\"},{\"uid\":\"FreeChrg\",\"value\":\"0.0\"},{\"uid\":\"FreeChrgFC\",\"value\":\"0.0\"},{\"uid\":\"FreeChrgSC\",\"value\":\"0.0\"},{\"uid\":\"NfeValue\",\"value\":\"0.0\"},{\"uid\":\"RelatedTyp\",\"value\":\"-1\"},{\"uid\":\"NfePrntFo\",\"value\":\"0\"},{\"uid\":\"FoCTax\",\"value\":\"0.0\"},{\"uid\":\"FoCTaxFC\",\"value\":\"0.0\"},{\"uid\":\"FoCTaxSC\",\"value\":\"0.0\"},{\"uid\":\"FoCFrght\",\"value\":\"0.0\"},{\"uid\":\"FoCFrghtFC\",\"value\":\"0.0\"},{\"uid\":\"FoCFrghtSC\",\"value\":\"0.0\"},{\"uid\":\"InterimTyp\",\"value\":\"0\"},{\"uid\":\"SplitTax\",\"value\":\"0.0\"},{\"uid\":\"SplitTaxFC\",\"value\":\"0.0\"},{\"uid\":\"SplitTaxSC\",\"value\":\"0.0\"},{\"uid\":\"PoDropPrss\",\"value\":\"N\"},{\"uid\":\"ExclTaxRep\",\"value\":\"N\"},{\"uid\":\"Revision\",\"value\":\"N\"},{\"uid\":\"BaseType\",\"value\":\"-1\"},{\"uid\":\"ComTrade\",\"value\":\"E\"},{\"uid\":\"UseBilAddr\",\"value\":\"N\"},{\"uid\":\"IssReason\",\"value\":\"1\"},{\"uid\":\"ComTradeRt\",\"value\":\"N\"},{\"uid\":\"SplitPmnt\",\"value\":\"N\"},{\"uid\":\"SelfPosted\",\"value\":\"N\"},{\"uid\":\"DPPStatus\",\"value\":\"N\"},{\"uid\":\"CtActTax\",\"value\":\"0.0\"},{\"uid\":\"CtActTaxFC\",\"value\":\"0.0\"},{\"uid\":\"CtActTaxSC\",\"value\":\"0.0\"},{\"uid\":\"EDocType\",\"value\":\"F\"},{\"uid\":\"U_SEI_CANT_DOC\",\"value\":\"0.0\"},{\"uid\":\"U_SEI_FEOC\",\"value\":\"20200527\"},{\"uid\":\"U_Bol_Ini\",\"value\":\"0\"},{\"uid\":\"U_Bol_Fin\",\"value\":\"0\"},{\"uid\":\"U_SEI_IMPR\",\"value\":\"0\"},{\"uid\":\"U_SEI_ITRS\",\"value\":\"1\"},{\"uid\":\"U_SEI_CREF\",\"value\":\"0\"},{\"uid\":\"U_SEI_CREF2\",\"value\":\"0\"},{\"uid\":\"U_SEI_CREF3\",\"value\":\"0\"},{\"uid\":\"U_SEI_IND\",\"value\":\"0\"},{\"uid\":\"U_SEI_FENTREGA\",\"value\":\"20200528\"},{\"uid\":\"U_SEI_TTCOMPRA\",\"value\":\"0\"},{\"uid\":\"U_SEI_TTVENTA\",\"value\":\"0\"},{\"uid\":\"U_EstDoc\",\"value\":\"\"},{\"uid\":\"U_FETimbre\",\"value\":\"<TED version=\"},{\"uid\":\"U_EstadoFE\",\"value\":\"A\"},{\"uid\":\"U_90_dias\",\"value\":\"0\"},{\"uid\":\"U_IndServicioBol\",\"value\":\"3\"},{\"uid\":\"U_CodModVenta\",\"value\":\"1\"},{\"uid\":\"U_TotClauVenta\",\"value\":\"0.0\"},{\"uid\":\"U_PesoBruto\",\"value\":\"0.0\"},{\"uid\":\"U_PesoNeto\",\"value\":\"0.0\"},{\"uid\":\"U_TotItems\",\"value\":\"0.0\"},{\"uid\":\"U_TotBultos\",\"value\":\"0.0\"},{\"uid\":\"U_FmaPagExp\",\"value\":\"0\"},{\"uid\":\"U_CantBultos\",\"value\":\"0.0\"},{\"uid\":\"U_EstadoSII\",\"value\":\"\"}]}}}}},\"OCRD\":{\"xml\":{\"version\":\"1.0\",\"encoding\":\"UTF-16\"},\"dbDataSources\":{\"uid\":\"OCRD\",\"rows\":{\"row\":{\"cells\":{\"cell\":[{\"uid\":\"CardCode\",\"value\":\"C010534722\"},{\"uid\":\"CardName\",\"value\":\"FREDY OROZCO GARCIA\"},{\"uid\":\"CardType\",\"value\":\"C\"},{\"uid\":\"GroupCode\",\"value\":\"102\"},{\"uid\":\"CmpPrivate\",\"value\":\"C\"},{\"uid\":\"Address\",\"value\":\"CALLE 9C  5025 OF 709\"},{\"uid\":\"MailAddres\",\"value\":\"CALLE 9C  5025 OF 709\"},{\"uid\":\"Balance\",\"value\":\"67599.000000\"},{\"uid\":\"ChecksBal\",\"value\":\"0.0\"},{\"uid\":\"DNotesBal\",\"value\":\"0.0\"},{\"uid\":\"OrdersBal\",\"value\":\"0.0\"},{\"uid\":\"GroupNum\",\"value\":\"5\"},{\"uid\":\"CreditLine\",\"value\":\"1000000.000000\"},{\"uid\":\"DebtLine\",\"value\":\"1000000.000000\"},{\"uid\":\"Discount\",\"value\":\"0.0\"},{\"uid\":\"VatStatus\",\"value\":\"Y\"},{\"uid\":\"LicTradNum\",\"value\":\"55555555-5\"},{\"uid\":\"DdctStatus\",\"value\":\"N\"},{\"uid\":\"DdctPrcnt\",\"value\":\"0.0\"},{\"uid\":\"ListNum\",\"value\":\"1\"},{\"uid\":\"DNoteBalFC\",\"value\":\"0.0\"},{\"uid\":\"OrderBalFC\",\"value\":\"0.0\"},{\"uid\":\"DNoteBalSy\",\"value\":\"0.0\"},{\"uid\":\"OrderBalSy\",\"value\":\"0.0\"},{\"uid\":\"Transfered\",\"value\":\"N\"},{\"uid\":\"BalTrnsfrd\",\"value\":\"N\"},{\"uid\":\"IntrstRate\",\"value\":\"0.0\"},{\"uid\":\"Commission\",\"value\":\"0.0\"},{\"uid\":\"CommGrCode\",\"value\":\"0\"},{\"uid\":\"SlpCode\",\"value\":\"9\"},{\"uid\":\"PrevYearAc\",\"value\":\"N\"},{\"uid\":\"Currency\",\"value\":\"$\"},{\"uid\":\"RateDifAct\",\"value\":\"\"},{\"uid\":\"BalanceSys\",\"value\":\"76.000000\"},{\"uid\":\"BalanceFC\",\"value\":\"0.0\"},{\"uid\":\"Protected\",\"value\":\"N\"},{\"uid\":\"City\",\"value\":\"COLOMBIA\"},{\"uid\":\"County\",\"value\":\"COLOMBIA\"},{\"uid\":\"Country\",\"value\":\"CO\"},{\"uid\":\"MailCity\",\"value\":\"COLOMBIA\"},{\"uid\":\"MailCounty\",\"value\":\"COLOMBIA\"},{\"uid\":\"MailCountr\",\"value\":\"CO\"},{\"uid\":\"BankCode\",\"value\":\"-1\"},{\"uid\":\"FatherType\",\"value\":\"P\"},{\"uid\":\"QryGroup1\",\"value\":\"N\"},{\"uid\":\"QryGroup2\",\"value\":\"N\"},{\"uid\":\"QryGroup3\",\"value\":\"N\"},{\"uid\":\"QryGroup4\",\"value\":\"N\"},{\"uid\":\"QryGroup5\",\"value\":\"N\"},{\"uid\":\"QryGroup6\",\"value\":\"N\"},{\"uid\":\"QryGroup7\",\"value\":\"N\"},{\"uid\":\"QryGroup8\",\"value\":\"N\"},{\"uid\":\"QryGroup9\",\"value\":\"N\"},{\"uid\":\"QryGroup10\",\"value\":\"N\"},{\"uid\":\"QryGroup11\",\"value\":\"N\"},{\"uid\":\"QryGroup12\",\"value\":\"N\"},{\"uid\":\"QryGroup13\",\"value\":\"N\"},{\"uid\":\"QryGroup14\",\"value\":\"N\"},{\"uid\":\"QryGroup15\",\"value\":\"N\"},{\"uid\":\"QryGroup16\",\"value\":\"N\"},{\"uid\":\"QryGroup17\",\"value\":\"N\"},{\"uid\":\"QryGroup18\",\"value\":\"N\"},{\"uid\":\"QryGroup19\",\"value\":\"N\"},{\"uid\":\"QryGroup20\",\"value\":\"N\"},{\"uid\":\"QryGroup21\",\"value\":\"N\"},{\"uid\":\"QryGroup22\",\"value\":\"N\"},{\"uid\":\"QryGroup23\",\"value\":\"N\"},{\"uid\":\"QryGroup24\",\"value\":\"N\"},{\"uid\":\"QryGroup25\",\"value\":\"N\"},{\"uid\":\"QryGroup26\",\"value\":\"N\"},{\"uid\":\"QryGroup27\",\"value\":\"N\"},{\"uid\":\"QryGroup28\",\"value\":\"N\"},{\"uid\":\"QryGroup29\",\"value\":\"N\"},{\"uid\":\"QryGroup30\",\"value\":\"N\"},{\"uid\":\"QryGroup31\",\"value\":\"N\"},{\"uid\":\"QryGroup32\",\"value\":\"N\"},{\"uid\":\"QryGroup33\",\"value\":\"N\"},{\"uid\":\"QryGroup34\",\"value\":\"N\"},{\"uid\":\"QryGroup35\",\"value\":\"N\"},{\"uid\":\"QryGroup36\",\"value\":\"N\"},{\"uid\":\"QryGroup37\",\"value\":\"N\"},{\"uid\":\"QryGroup38\",\"value\":\"N\"},{\"uid\":\"QryGroup39\",\"value\":\"N\"},{\"uid\":\"QryGroup40\",\"value\":\"N\"},{\"uid\":\"QryGroup41\",\"value\":\"N\"},{\"uid\":\"QryGroup42\",\"value\":\"N\"},{\"uid\":\"QryGroup43\",\"value\":\"N\"},{\"uid\":\"QryGroup44\",\"value\":\"N\"},{\"uid\":\"QryGroup45\",\"value\":\"N\"},{\"uid\":\"QryGroup46\",\"value\":\"N\"},{\"uid\":\"QryGroup47\",\"value\":\"N\"},{\"uid\":\"QryGroup48\",\"value\":\"N\"},{\"uid\":\"QryGroup49\",\"value\":\"N\"},{\"uid\":\"QryGroup50\",\"value\":\"N\"},{\"uid\":\"QryGroup51\",\"value\":\"N\"},{\"uid\":\"QryGroup52\",\"value\":\"N\"},{\"uid\":\"QryGroup53\",\"value\":\"N\"},{\"uid\":\"QryGroup54\",\"value\":\"N\"},{\"uid\":\"QryGroup55\",\"value\":\"N\"},{\"uid\":\"QryGroup56\",\"value\":\"N\"},{\"uid\":\"QryGroup57\",\"value\":\"N\"},{\"uid\":\"QryGroup58\",\"value\":\"N\"},{\"uid\":\"QryGroup59\",\"value\":\"N\"},{\"uid\":\"QryGroup60\",\"value\":\"N\"},{\"uid\":\"QryGroup61\",\"value\":\"N\"},{\"uid\":\"QryGroup62\",\"value\":\"N\"},{\"uid\":\"QryGroup63\",\"value\":\"N\"},{\"uid\":\"QryGroup64\",\"value\":\"N\"},{\"uid\":\"CreateDate\",\"value\":\"20160408\"},{\"uid\":\"UpdateDate\",\"value\":\"20200626\"},{\"uid\":\"DscntObjct\",\"value\":\"-1\"},{\"uid\":\"DscntRel\",\"value\":\"L\"},{\"uid\":\"SPGCounter\",\"value\":\"0\"},{\"uid\":\"SPPCounter\",\"value\":\"0\"},{\"uid\":\"MinIntrst\",\"value\":\"0.0\"},{\"uid\":\"DataSource\",\"value\":\"I\"},{\"uid\":\"OprCount\",\"value\":\"0\"},{\"uid\":\"Priority\",\"value\":\"-1\"},{\"uid\":\"CreditCard\",\"value\":\"-1\"},{\"uid\":\"CrCardNum\",\"value\":\"/aJkEu9+xYoUrzxQe1zMCw==\"},{\"uid\":\"UserSign\",\"value\":\"9\"},{\"uid\":\"LocMth\",\"value\":\"Y\"},{\"uid\":\"validFor\",\"value\":\"Y\"},{\"uid\":\"frozenFor\",\"value\":\"N\"},{\"uid\":\"sEmployed\",\"value\":\"N\"},{\"uid\":\"DdgKey\",\"value\":\"-1\"},{\"uid\":\"DdtKey\",\"value\":\"-1\"},{\"uid\":\"chainStore\",\"value\":\"N\"},{\"uid\":\"DiscInRet\",\"value\":\"N\"},{\"uid\":\"State1\",\"value\":\"CAL\"},{\"uid\":\"State2\",\"value\":\"CAL\"},{\"uid\":\"LogInstanc\",\"value\":\"0\"},{\"uid\":\"ObjType\",\"value\":\"2\"},{\"uid\":\"DebPayAcct\",\"value\":\"1-1-04-01-001\"},{\"uid\":\"ShipToDef\",\"value\":\"DESPACHO\"},{\"uid\":\"Deleted\",\"value\":\"N\"},{\"uid\":\"DocEntry\",\"value\":\"1730\"},{\"uid\":\"BackOrder\",\"value\":\"Y\"},{\"uid\":\"PartDelivr\",\"value\":\"Y\"},{\"uid\":\"BlockDunn\",\"value\":\"N\"},{\"uid\":\"BankCountr\",\"value\":\"CL\"},{\"uid\":\"CollecAuth\",\"value\":\"N\"},{\"uid\":\"SinglePaym\",\"value\":\"N\"},{\"uid\":\"PaymBlock\",\"value\":\"N\"},{\"uid\":\"HouseBank\",\"value\":\"016\"},{\"uid\":\"PyBlckDesc\",\"value\":\"-1\"},{\"uid\":\"HousBnkCry\",\"value\":\"CL\"},{\"uid\":\"HousBnkAct\",\"value\":\"18023886\"},{\"uid\":\"SysMatchNo\",\"value\":\"-1\"},{\"uid\":\"DeferrTax\",\"value\":\"N\"},{\"uid\":\"MaxAmount\",\"value\":\"0.0\"},{\"uid\":\"AccCritria\",\"value\":\"N\"},{\"uid\":\"Equ\",\"value\":\"N\"},{\"uid\":\"TypWTReprt\",\"value\":\"C\"},{\"uid\":\"IsDomestic\",\"value\":\"Y\"},{\"uid\":\"IsResident\",\"value\":\"Y\"},{\"uid\":\"AutoCalBCG\",\"value\":\"N\"},{\"uid\":\"AliasName\",\"value\":\"CIRUJANO INFANTIL\"},{\"uid\":\"Building\",\"value\":\"\"},{\"uid\":\"MailBuildi\",\"value\":\"\"},{\"uid\":\"BillToDef\",\"value\":\"FACTURACION\"},{\"uid\":\"LangCode\",\"value\":\"25\"},{\"uid\":\"HousActKey\",\"value\":\"2\"},{\"uid\":\"UseShpdGd\",\"value\":\"N\"},{\"uid\":\"InsurOp347\",\"value\":\"N\"},{\"uid\":\"TaxRndRule\",\"value\":\"D\"},{\"uid\":\"ThreshOver\",\"value\":\"N\"},{\"uid\":\"SurOver\",\"value\":\"N\"},{\"uid\":\"OpCode347\",\"value\":\"B\"},{\"uid\":\"ResidenNum\",\"value\":\"1\"},{\"uid\":\"UserSign2\",\"value\":\"1\"},{\"uid\":\"Affiliate\",\"value\":\"N\"},{\"uid\":\"MivzExpSts\",\"value\":\"B\"},{\"uid\":\"HierchDdct\",\"value\":\"N\"},{\"uid\":\"CertWHT\",\"value\":\"N\"},{\"uid\":\"CertBKeep\",\"value\":\"N\"},{\"uid\":\"WHShaamGrp\",\"value\":\"1\"},{\"uid\":\"DatevFirst\",\"value\":\"Y\"},{\"uid\":\"Series\",\"value\":\"2\"},{\"uid\":\"TaxIdIdent\",\"value\":\"3\"},{\"uid\":\"DiscRel\",\"value\":\"L\"},{\"uid\":\"NoDiscount\",\"value\":\"N\"},{\"uid\":\"SCAdjust\",\"value\":\"N\"},{\"uid\":\"SefazCheck\",\"value\":\"N\"},{\"uid\":\"TpCusPres\",\"value\":\"9\"},{\"uid\":\"BlockComm\",\"value\":\"N\"},{\"uid\":\"ExpnPrfFnd\",\"value\":\"0.0\"},{\"uid\":\"EdrsFromBP\",\"value\":\"Y\"},{\"uid\":\"EdrsToBP\",\"value\":\"N\"},{\"uid\":\"CreateTS\",\"value\":\"0\"},{\"uid\":\"UpdateTS\",\"value\":\"11819\"},{\"uid\":\"EffecPrice\",\"value\":\"D\"},{\"uid\":\"TxExMxVdTp\",\"value\":\"I\"},{\"uid\":\"UseBilAddr\",\"value\":\"N\"},{\"uid\":\"NaturalPer\",\"value\":\"N\"},{\"uid\":\"DPPStatus\",\"value\":\"N\"},{\"uid\":\"EnERD4In\",\"value\":\"Y\"},{\"uid\":\"EnERD4Out\",\"value\":\"Y\"},{\"uid\":\"DflCustomr\",\"value\":\"N\"},{\"uid\":\"FCERelevnt\",\"value\":\"N\"},{\"uid\":\"FCEVldte\",\"value\":\"N\"},{\"uid\":\"U_Tipo\",\"value\":\"N\"}]}}}}},\"INV1\":{\"xml\":{\"version\":\"1.0\",\"encoding\":\"UTF-16\"},\"dbDataSources\":{\"uid\":\"INV1\",\"rows\":{\"row\":{\"cells\":{\"cell\":[{\"uid\":\"LineNum\",\"value\":\"1\"},{\"uid\":\"TargetType\",\"value\":\"-1\"},{\"uid\":\"BaseRef\",\"value\":\"\"},{\"uid\":\"BaseType\",\"value\":\"-1\"},{\"uid\":\"LineStatus\",\"value\":\"O\"},{\"uid\":\"ItemCode\",\"value\":\"RPDE\"},{\"uid\":\"Dscription\",\"value\":\"RECARGO POR DESPACHO EXPRESS\"},{\"uid\":\"Quantity\",\"value\":\"1.000000\"},{\"uid\":\"OpenQty\",\"value\":\"0.0\"},{\"uid\":\"Price\",\"value\":\"50.000000\"},{\"uid\":\"Currency\",\"value\":\"$  \"},{\"uid\":\"Rate\",\"value\":\"0.0\"},{\"uid\":\"DiscPrcnt\",\"value\":\"0.0\"},{\"uid\":\"LineTotal\",\"value\":\"50.000000\"},{\"uid\":\"TotalFrgn\",\"value\":\"0.0\"},{\"uid\":\"OpenSum\",\"value\":\"0.0\"},{\"uid\":\"OpenSumFC\",\"value\":\"0.0\"},{\"uid\":\"VendorNum\",\"value\":\"\"},{\"uid\":\"SerialNum\",\"value\":\"\"},{\"uid\":\"WhsCode\",\"value\":\"BOD001\"},{\"uid\":\"SlpCode\",\"value\":\"9\"},{\"uid\":\"Commission\",\"value\":\"0.0\"},{\"uid\":\"TreeType\",\"value\":\"N\"},{\"uid\":\"AcctCode\",\"value\":\"4-1-01-01-021\"},{\"uid\":\"TaxStatus\",\"value\":\"Y\"},{\"uid\":\"GrossBuyPr\",\"value\":\"0.0\"},{\"uid\":\"PriceBefDi\",\"value\":\"50.000000\"},{\"uid\":\"DocDate\",\"value\":\"20200527\"},{\"uid\":\"Flags\",\"value\":\"0\"},{\"uid\":\"OpenCreQty\",\"value\":\"1.000000\"},{\"uid\":\"UseBaseUn\",\"value\":\"N\"},{\"uid\":\"SubCatNum\",\"value\":\"\"},{\"uid\":\"BaseCard\",\"value\":\"C010534722\"},{\"uid\":\"TotalSumSy\",\"value\":\"0.0\"},{\"uid\":\"OpenSumSys\",\"value\":\"0.0\"},{\"uid\":\"InvntSttus\",\"value\":\"O\"},{\"uid\":\"Project\",\"value\":\"\"},{\"uid\":\"VatPrcnt\",\"value\":\"19.000000\"},{\"uid\":\"VatGroup\",\"value\":\"IVA\"},{\"uid\":\"PriceAfVAT\",\"value\":\"59.500000\"},{\"uid\":\"Height1\",\"value\":\"0.0\"},{\"uid\":\"Height2\",\"value\":\"0.0\"},{\"uid\":\"Width1\",\"value\":\"0.0\"},{\"uid\":\"Width2\",\"value\":\"0.0\"},{\"uid\":\"Length1\",\"value\":\"0.0\"},{\"uid\":\"length2\",\"value\":\"0.0\"},{\"uid\":\"Volume\",\"value\":\"0.0\"},{\"uid\":\"VolUnit\",\"value\":\"4\"},{\"uid\":\"Weight1\",\"value\":\"0.0\"},{\"uid\":\"Weight2\",\"value\":\"0.0\"},{\"uid\":\"Factor1\",\"value\":\"1.000000\"},{\"uid\":\"Factor2\",\"value\":\"1.000000\"},{\"uid\":\"Factor3\",\"value\":\"1.000000\"},{\"uid\":\"Factor4\",\"value\":\"1.000000\"},{\"uid\":\"PackQty\",\"value\":\"1.000000\"},{\"uid\":\"UpdInvntry\",\"value\":\"Y\"},{\"uid\":\"BaseAtCard\",\"value\":\"5326-836-CM20\"},{\"uid\":\"SWW\",\"value\":\"\"},{\"uid\":\"VatSum\",\"value\":\"10.000000\"},{\"uid\":\"VatSumFrgn\",\"value\":\"0.0\"},{\"uid\":\"VatSumSy\",\"value\":\"0.0\"},{\"uid\":\"ObjType\",\"value\":\"13\"},{\"uid\":\"LogInstanc\",\"value\":\"0\"},{\"uid\":\"DedVatSum\",\"value\":\"0.0\"},{\"uid\":\"DedVatSumF\",\"value\":\"0.0\"},{\"uid\":\"DedVatSumS\",\"value\":\"0.0\"},{\"uid\":\"IsAqcuistn\",\"value\":\"N\"},{\"uid\":\"DistribSum\",\"value\":\"0.0\"},{\"uid\":\"DstrbSumFC\",\"value\":\"0.0\"},{\"uid\":\"DstrbSumSC\",\"value\":\"0.0\"},{\"uid\":\"GrssProfit\",\"value\":\"50.000000\"},{\"uid\":\"GrssProfSC\",\"value\":\"0.0\"},{\"uid\":\"GrssProfFC\",\"value\":\"0.0\"},{\"uid\":\"INMPrice\",\"value\":\"50.000000\"},{\"uid\":\"PoTrgEntry\",\"value\":\"\"},{\"uid\":\"DropShip\",\"value\":\"N\"},{\"uid\":\"Address\",\"value\":\"7570014 SANTIAGOCHILE\"},{\"uid\":\"TaxCode\",\"value\":\"IVA\"},{\"uid\":\"TaxType\",\"value\":\"Y\"},{\"uid\":\"BackOrdr\",\"value\":\"\"},{\"uid\":\"FreeTxt\",\"value\":\"\"},{\"uid\":\"PickStatus\",\"value\":\"N\"},{\"uid\":\"PickOty\",\"value\":\"0.0\"},{\"uid\":\"TrnsCode\",\"value\":\"-1\"},{\"uid\":\"VatAppld\",\"value\":\"0.0\"},{\"uid\":\"VatAppldFC\",\"value\":\"0.0\"},{\"uid\":\"VatAppldSC\",\"value\":\"0.0\"},{\"uid\":\"BaseQty\",\"value\":\"0.0\"},{\"uid\":\"BaseOpnQty\",\"value\":\"0.0\"},{\"uid\":\"VatDscntPr\",\"value\":\"0.0\"},{\"uid\":\"WtLiable\",\"value\":\"N\"},{\"uid\":\"DeferrTax\",\"value\":\"N\"},{\"uid\":\"EquVatPer\",\"value\":\"0.0\"},{\"uid\":\"EquVatSum\",\"value\":\"0.0\"},{\"uid\":\"EquVatSumF\",\"value\":\"0.0\"},{\"uid\":\"EquVatSumS\",\"value\":\"0.0\"},{\"uid\":\"LineVat\",\"value\":\"10.000000\"},{\"uid\":\"LineVatlF\",\"value\":\"0.0\"},{\"uid\":\"LineVatS\",\"value\":\"0.0\"},{\"uid\":\"NumPerMsr\",\"value\":\"1.000000\"},{\"uid\":\"CEECFlag\",\"value\":\"S\"},{\"uid\":\"ToStock\",\"value\":\"0.0\"},{\"uid\":\"ToDiff\",\"value\":\"0.0\"},{\"uid\":\"ExciseAmt\",\"value\":\"0.0\"},{\"uid\":\"TaxPerUnit\",\"value\":\"0.0\"},{\"uid\":\"TotInclTax\",\"value\":\"0.0\"},{\"uid\":\"StckDstSum\",\"value\":\"0.0\"},{\"uid\":\"ReleasQtty\",\"value\":\"0.0\"},{\"uid\":\"LineType\",\"value\":\"R\"},{\"uid\":\"StockPrice\",\"value\":\"0.0\"},{\"uid\":\"ConsumeFCT\",\"value\":\"N\"},{\"uid\":\"LstByDsSum\",\"value\":\"0.0\"},{\"uid\":\"StckINMPr\",\"value\":\"0.0\"},{\"uid\":\"LstBINMPr\",\"value\":\"0.0\"},{\"uid\":\"StckDstFc\",\"value\":\"0.0\"},{\"uid\":\"StckDstSc\",\"value\":\"0.0\"},{\"uid\":\"LstByDsFc\",\"value\":\"0.0\"},{\"uid\":\"LstByDsSc\",\"value\":\"0.0\"},{\"uid\":\"StockSum\",\"value\":\"0.0\"},{\"uid\":\"StockSumFc\",\"value\":\"0.0\"},{\"uid\":\"StockSumSc\",\"value\":\"0.0\"},{\"uid\":\"StckSumApp\",\"value\":\"0.0\"},{\"uid\":\"StckAppFc\",\"value\":\"0.0\"},{\"uid\":\"StckAppSc\",\"value\":\"0.0\"},{\"uid\":\"ShipToCode\",\"value\":\"DESPACHO\"},{\"uid\":\"ShipToDesc\",\"value\":\"CALLE 9C  5025 OF 709  COLOMBIACOLOMBIA\"},{\"uid\":\"StckAppD\",\"value\":\"0.0\"},{\"uid\":\"StckAppDFC\",\"value\":\"0.0\"},{\"uid\":\"StckAppDSC\",\"value\":\"0.0\"},{\"uid\":\"BasePrice\",\"value\":\"E\"},{\"uid\":\"GTotal\",\"value\":\"60.000000\"},{\"uid\":\"GTotalFC\",\"value\":\"0.0\"},{\"uid\":\"GTotalSC\",\"value\":\"0.0\"},{\"uid\":\"DistribExp\",\"value\":\"N\"},{\"uid\":\"DescOW\",\"value\":\"N\"},{\"uid\":\"DetailsOW\",\"value\":\"N\"},{\"uid\":\"GrossBase\",\"value\":\"-5\"},{\"uid\":\"VatWoDpm\",\"value\":\"0.0\"},{\"uid\":\"VatWoDpmFc\",\"value\":\"0.0\"},{\"uid\":\"VatWoDpmSc\",\"value\":\"0.0\"},{\"uid\":\"TaxOnly\",\"value\":\"N\"},{\"uid\":\"WtCalced\",\"value\":\"N\"},{\"uid\":\"QtyToShip\",\"value\":\"0.0\"},{\"uid\":\"DelivrdQty\",\"value\":\"0.0\"},{\"uid\":\"OrderedQty\",\"value\":\"1.000000\"},{\"uid\":\"CiOppLineN\",\"value\":\"-1\"},{\"uid\":\"CogsAcct\",\"value\":\"5-1-01-01-021\"},{\"uid\":\"ChgAsmBoMW\",\"value\":\"N\"},{\"uid\":\"ActDelDate\",\"value\":\"20200706\"},{\"uid\":\"TaxDistSum\",\"value\":\"0.0\"},{\"uid\":\"TaxDistSFC\",\"value\":\"0.0\"},{\"uid\":\"TaxDistSSC\",\"value\":\"0.0\"},{\"uid\":\"PostTax\",\"value\":\"Y\"},{\"uid\":\"AssblValue\",\"value\":\"0.0\"},{\"uid\":\"StockValue\",\"value\":\"0.0\"},{\"uid\":\"GPTtlBasPr\",\"value\":\"0.0\"},{\"uid\":\"NumPerMsr2\",\"value\":\"1.000000\"},{\"uid\":\"SpecPrice\",\"value\":\"R\"},{\"uid\":\"isSrvCall\",\"value\":\"N\"},{\"uid\":\"PQTReqQty\",\"value\":\"0.0\"},{\"uid\":\"PcDocType\",\"value\":\"-1\"},{\"uid\":\"PcQuantity\",\"value\":\"0.0\"},{\"uid\":\"LinManClsd\",\"value\":\"N\"},{\"uid\":\"VatGrpSrc\",\"value\":\"N\"},{\"uid\":\"NoInvtryMv\",\"value\":\"N\"},{\"uid\":\"OpenRtnQty\",\"value\":\"0.0\"},{\"uid\":\"Surpluses\",\"value\":\"0.0\"},{\"uid\":\"DefBreak\",\"value\":\"0.0\"},{\"uid\":\"Shortages\",\"value\":\"0.0\"},{\"uid\":\"UomEntry\",\"value\":\"-1\"},{\"uid\":\"UomEntry2\",\"value\":\"-1\"},{\"uid\":\"UomCode\",\"value\":\"Manual\"},{\"uid\":\"UomCode2\",\"value\":\"Manual\"},{\"uid\":\"NeedQty\",\"value\":\"N\"},{\"uid\":\"PartRetire\",\"value\":\"N\"},{\"uid\":\"RetireQty\",\"value\":\"0.0\"},{\"uid\":\"RetireAPC\",\"value\":\"0.0\"},{\"uid\":\"RetirAPCFC\",\"value\":\"0.0\"},{\"uid\":\"RetirAPCSC\",\"value\":\"0.0\"},{\"uid\":\"InvQty\",\"value\":\"1.000000\"},{\"uid\":\"OpenInvQty\",\"value\":\"1.000000\"},{\"uid\":\"EnSetCost\",\"value\":\"N\"},{\"uid\":\"RetCost\",\"value\":\"0.0\"},{\"uid\":\"Incoterms\",\"value\":\"0\"},{\"uid\":\"TransMod\",\"value\":\"0\"},{\"uid\":\"LineVendor\",\"value\":\"\"},{\"uid\":\"DistribIS\",\"value\":\"N\"},{\"uid\":\"ISDistrb\",\"value\":\"0.0\"},{\"uid\":\"ISDistrbFC\",\"value\":\"0.0\"},{\"uid\":\"ISDistrbSC\",\"value\":\"0.0\"},{\"uid\":\"IsByPrdct\",\"value\":\"N\"},{\"uid\":\"ItemType\",\"value\":\"4\"},{\"uid\":\"PriceEdit\",\"value\":\"N\"},{\"uid\":\"LinePoPrss\",\"value\":\"N\"},{\"uid\":\"FreeChrgBP\",\"value\":\"N\"},{\"uid\":\"TaxRelev\",\"value\":\"Y\"},{\"uid\":\"ThirdParty\",\"value\":\"N\"},{\"uid\":\"InvQtyOnly\",\"value\":\"N\"},{\"uid\":\"GPBefDisc\",\"value\":\"59.500000\"},{\"uid\":\"ReturnRsn\",\"value\":\"-1\"},{\"uid\":\"ReturnAct\",\"value\":\"-1\"},{\"uid\":\"NCMCode\",\"value\":\"-1\"},{\"uid\":\"IsPrscGood\",\"value\":\"N\"},{\"uid\":\"IsCstmAct\",\"value\":\"N\"},{\"uid\":\"U_TipoDTELF\",\"value\":\"00\"}]}}}}}}";

                ////p.SendJson("http://localhost:2396/api/Factura/validarDocumento", "{\"ejemplo\": \"hola\"}");
                //p.SendJson("http://localhost:2396/api/Factura/validarDocumento", jsonSAP);

                //p.incremetar();
//                string query = @"SELECT |DocEntry| , |DocNum| FROM |OINV| WHERE |DocNum| = 2006704";
//                query = query.Replace("|", "\"");
//                SAPbobsCOM.Recordset oRs = p.doQuery(query);
//                int a = oRs.RecordCount;

//                engine.SetValue("sbo", p);

//                string script = @"
//                var result;
//                function execute()
//                {
//                    var query ='SELECT |DocEntry|,|DocNum| FROM |OINV| WHERE |DocNum| = 2006704';   
//                    var rs = sbo.doJson(query);  
//                    result = rs;
//                    return result;
//                }";
//                script = script.Replace("|", "\"");

//                engine.Execute(script);
//                Jint.Native.JsValue v = engine.Invoke("execute");
//                var result = engine.GetValue("result");
                //Jint.Native.JsValue v = engine.GetCompletionValue();

                // Console.WriteLine(engine.Execute("2+2").GetCompletionValue());

                Params list;
                string jsonMenuPath = @"params/params.json";
                using (StreamReader jsonStram = File.OpenText(jsonMenuPath))
                {
                    var json = jsonStram.ReadToEnd();
                    list = JsonConvert.DeserializeObject<Params>(json);
                }

                string prefix = list.urlws;
                ws = new Webserver(SendResponse, prefix);
                ws.Run();
                

//                script = @"
//                    var file = new System.IO.StreamWriter('log.txt');
//                    file.WriteLine('Hello World !');
//                    file.Dispose();
//                    (3 * 3).toString();
//                ";

//                var url = "http://localhost:50201";
//                var test2 = p.SendText(url, script);

                //validarJson();

                Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);

                //**************************************************************************************
                //******************* CODIGO CREACION ESTRUCTURA UDOS Y UDT ****************************
                //**************************************************************************************


                SBO_Application = Application.SBO_Application;
                oCompany = new SAPbobsCOM.Company();
                oCompany = (SAPbobsCOM.Company)SBO_Application.Company.GetDICompany();

                SBOFunctions.SBOFunctions CSBOFunctions = new SBOFunctions.SBOFunctions();

                CSBOFunctions.Cmpny = oCompany;
                CSBOFunctions.SBOApp = SBO_Application;
                CSBOFunctions.RunningUnderSQLServer = CSBOFunctions.GetRunningUnderSQLServer();

                String sPath = CSBOFunctions.GetPathApp();
                string XlsFile = sPath + "\\Docs\\UDFFELEC.xls";
                if (!CSBOFunctions.ValidEstructSHA1(XlsFile))
                {
                    CSBOFunctions.AddLog("InitApp: Estructura de datos 1 - Facturación Electronica");
                    SBO_Application.StatusBar.SetText("Inicializando AddOn Factura Electronica(1).", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    if (!CSBOFunctions.SyncTablasUdos("1.1", XlsFile))
                    {
                        CSBOFunctions.DeleteSHA1FromTable("EDAG.xls");
                        CSBOFunctions.AddLog("InitApp: sincronización de Estructura de datos fallo");
                        SBO_Application.MessageBox("Estructura de datos con problemas, consulte a soporte...", 1, "Ok", "", "");
                    }
                }

                XlsFile = sPath + "\\Docs\\UDFFELECCL.xls";
                if (!CSBOFunctions.ValidEstructSHA1(XlsFile))
                {
                    CSBOFunctions.AddLog("InitApp: Estructura de datos 2 - Facturación Electronica CL");
                    SBO_Application.StatusBar.SetText("Inicializando AddOn Factura Electronica(2).", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    if (!CSBOFunctions.SyncTablasUdos("1.1", XlsFile))
                    {
                        CSBOFunctions.DeleteSHA1FromTable("UDFFELECCL.xls");
                        CSBOFunctions.AddLog("InitApp: sincronización de Estructura de datos fallo");
                        SBO_Application.MessageBox("Estructura de datos con problemas, consulte a soporte...", 1, "Ok", "", "");
                    }
                }


                XlsFile = sPath + "\\Docs\\UDFSAP.xls";
                if (!CSBOFunctions.ValidEstructSHA1(XlsFile))
                {
                    CSBOFunctions.AddLog("InitApp: Estructura de datos 3 - Facturación Electronica CL");
                    SBO_Application.StatusBar.SetText("Inicializando AddOn Factura Electronica(3).", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    if (!CSBOFunctions.SyncTablasUdos("1.1", XlsFile))
                    {
                        CSBOFunctions.DeleteSHA1FromTable("UDFSAP.xls");
                        CSBOFunctions.AddLog("InitApp: sincronización de Estructura de datos fallo");
                        SBO_Application.MessageBox("Estructura de datos con problemas, consulte a soporte...", 1, "Ok", "", "");
                    }
                }

                //**************************************************************************************
                //**************************************************************************************
                //**************************************************************************************


                WebAppForm webAppForm = new WebAppForm("test", 500, 500, "http://restvk.planetsoft.cl");
                webAppForm.Show();

                oApp.Run();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(classid +": " + ex.Message);
            }
        }

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            try
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
            } catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(" App Event Exception: "+ e.Message);
            }
     
        }

        static void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            //SAPbouiCOM.Form oForm = null;
            //SAPbouiCOM.EditText oEdit = null;
            //SAPbouiCOM.DataTable oDataTable = null;

            try
            {
                contador++;
                if (contador == 1)
                {
                    //var sboJint = new SboJint();
                    //sboJint.levantarVentanaSAP("1", "Invoice");
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
