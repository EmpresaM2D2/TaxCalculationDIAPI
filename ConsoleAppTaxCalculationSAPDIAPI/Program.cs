using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleAppTaxCalculationSAPDIAPI
{

    internal class Program
    {
        static void Main(string[] args)
        {
            var message = "This is a tax calculation method using DI API. We need this in Service Layer";
            MessageHelper.PrintRectangle(message);            
            var company = CompanyHelper.GetCompany();
            
            if (company.Connect() != 0)
            {
                Console.WriteLine("Connection failed");
                Console.WriteLine(company.GetLastErrorDescription());
                Console.WriteLine("Please change the connection parameters in CompanyHelper.cs file and try again");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Connection successful");

            company.StartTransaction();

            try
            {
                var sapDraft = company.GetBusinessObject(BoObjectTypes.oDrafts) as SAPbobsCOM.Documents;
                sapDraft.DocObjectCode = BoObjectTypes.oInvoices;

                sapDraft.Comments = "The tax determination was defined by Usage field";
                sapDraft.CardCode = "C00001";
                sapDraft.DocDate = DateTime.Now.Date;

                sapDraft.Lines.ItemCode = "A00001";
                sapDraft.Lines.Quantity = 1;
                sapDraft.Lines.UnitPrice = 1000;
                sapDraft.Lines.WarehouseCode = "01";
                sapDraft.Lines.TaxCode = "1101-001";
                sapDraft.Lines.Usage = "13";//Many cases the tax determination is based on the usage field

                var saved = sapDraft.Add();
                if (saved != 0)
                {
                    Console.WriteLine(company.GetLastErrorDescription());
                    company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    return;
                }

                var docEntry = int.Parse(company.GetNewObjectKey());
                Console.WriteLine($"Temporary document saved with docEntry: {docEntry}");

                var sql = GetSqlTaxValues(docEntry);
                var recordset = company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                recordset.DoQuery(sql);
                recordset.MoveFirst();

                var taxResponse = new TaxResponse();
               
                while (!recordset.EoF)
                {
                    var taxCodeInfo = new TaxCodeInf(recordset);
                    taxResponse.AddTaxInformation(taxCodeInfo);
                    Console.WriteLine($"ItemCode: {recordset.Fields.Item("ItemCode").Value}");
                    Console.WriteLine($"LineNum: {recordset.Fields.Item("LineNum").Value}");
                    Console.WriteLine($"TaxType: {recordset.Fields.Item("TaxType").Value}");
                    Console.WriteLine($"TaxName: {recordset.Fields.Item("TaxName").Value}");
                    Console.WriteLine($"BaseSum: {recordset.Fields.Item("BaseSum").Value}");
                    Console.WriteLine($"TaxSum: {recordset.Fields.Item("TaxSum").Value}");
                    Console.WriteLine($"DeductTax: {recordset.Fields.Item("DeductTax").Value}");
                    Console.WriteLine($"DdctTaxFrg: {recordset.Fields.Item("DdctTaxFrg").Value}");
                    Console.WriteLine($"DdctTaxSys: {recordset.Fields.Item("DdctTaxSys").Value}");
                    Console.WriteLine($"InGrossRev: {recordset.Fields.Item("InGrossRev").Value}");
                    Console.WriteLine($"TaxInPrice: {recordset.Fields.Item("TaxInPrice").Value}");
                    Console.WriteLine($"Unencumbrd: {recordset.Fields.Item("Unencumbrd").Value}");
                    Console.WriteLine($"TaxSumOrg: {recordset.Fields.Item("TaxSumOrg").Value}");
                    Console.WriteLine($"TaxSumOrgS: {recordset.Fields.Item("TaxSumOrgS").Value}");
                    Console.WriteLine($"TaxOnRI: {recordset.Fields.Item("TaxOnRI").Value}");
                    Console.WriteLine($"InFirstIns: {recordset.Fields.Item("InFirstIns").Value}");
                    Console.WriteLine($"ExtTaxRate: {recordset.Fields.Item("ExtTaxRate").Value}");
                    Console.WriteLine($"ExtTaxSum: {recordset.Fields.Item("ExtTaxSum").Value}");
                    Console.WriteLine($"TaxAmtSrc: {recordset.Fields.Item("TaxAmtSrc").Value}");
                    Console.WriteLine($"ExtTaxSumF: {recordset.Fields.Item("ExtTaxSumF").Value}");
                    Console.WriteLine($"U_Base: {recordset.Fields.Item("U_Base").Value}");
                    Console.WriteLine($"U_Isento: {recordset.Fields.Item("U_Isento").Value}");
                    Console.WriteLine($"U_Outros: {recordset.Fields.Item("U_Outros").Value}");
                    Console.WriteLine($"U_Minimo: {recordset.Fields.Item("U_Minimo").Value}");
                    Console.WriteLine($"U_Unidades: {recordset.Fields.Item("U_Unidades").Value}");
                    Console.WriteLine($"U_Medida: {recordset.Fields.Item("U_Medida").Value}");
                    Console.WriteLine($"U_Moeda: {recordset.Fields.Item("U_Moeda").Value}");
                    Console.WriteLine($"U_Lucro: {recordset.Fields.Item("U_Lucro").Value}");
                    Console.WriteLine($"U_Reducao1: {recordset.Fields.Item("U_Reducao1").Value}");
                    Console.WriteLine($"U_Reducao2: {recordset.Fields.Item("U_Reducao2").Value}");
                    Console.WriteLine($"U_ReduICMS: {recordset.Fields.Item("U_ReduICMS").Value}");
                    Console.WriteLine($"U_PrecoFix: {recordset.Fields.Item("U_PrecoFix").Value}");
                    Console.WriteLine($"U_FatorPrc: {recordset.Fields.Item("U_FatorPrc").Value}");
                    Console.WriteLine($"U_ExcAmtL: {recordset.Fields.Item("U_ExcAmtL").Value}");
                    Console.WriteLine($"U_ExcAmtF: {recordset.Fields.Item("U_ExcAmtF").Value}");
                    Console.WriteLine($"U_ExcAmtS: {recordset.Fields.Item("U_ExcAmtS").Value}");
                    Console.WriteLine($"U_OthAmtL: {recordset.Fields.Item("U_OthAmtL").Value}");
                    Console.WriteLine($"U_OthAmtF: {recordset.Fields.Item("U_OthAmtF").Value}");
                    Console.WriteLine($"U_OthAmtS: {recordset.Fields.Item("U_OthAmtS").Value}");
                    Console.WriteLine($"U_TotalBL: {recordset.Fields.Item("U_TotalBL").Value}");
                    Console.WriteLine($"U_TotalBF: {recordset.Fields.Item("U_TotalBF").Value}");
                    Console.WriteLine($"U_TotalBS: {recordset.Fields.Item("DdctTaxSys").Value}");
                    Console.WriteLine("--------Rate details (Optional data)----------");
                    var sqlRates = GetSqlTaxRates(recordset.Fields.Item("Code").Value.ToString());
                    var recordsetRates = company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                    recordsetRates.DoQuery(sqlRates);
                    recordsetRates.MoveFirst();
                    while (!recordsetRates.EoF)
                    {
                        Console.WriteLine(new string('»', 6)+"»");
                        Console.WriteLine(new string('»', 6)+$"TaxType: {recordsetRates.Fields.Item("Name").Value}");
                        Console.WriteLine(new string('»', 6)+$"Rate: {recordsetRates.Fields.Item("Rate").Value}");
                        Console.WriteLine(new string('»', 6)+$"TaxInPrice: {recordsetRates.Fields.Item("TaxInPrice").Value}");
                        Console.WriteLine(new string('»', 6)+$"Exempt: {recordsetRates.Fields.Item("Exempt").Value}");
                        Console.WriteLine(new string('»', 6)+$"InGrossRev: {recordsetRates.Fields.Item("InGrossRev").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_Base: {recordsetRates.Fields.Item("U_Base").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_Isento: {recordsetRates.Fields.Item("U_Isento").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_Outros: {recordsetRates.Fields.Item("U_Outros").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_Minimo: {recordsetRates.Fields.Item("U_Minimo").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_Unidades: {recordsetRates.Fields.Item("U_Unidades").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_PrecoMin: {recordsetRates.Fields.Item("U_PrecoMin").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_Lucro: {recordsetRates.Fields.Item("U_Lucro").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_Reducao1: {recordsetRates.Fields.Item("U_Reducao1").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_Reducao2: {recordsetRates.Fields.Item("U_Reducao2").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_ReduICMS: {recordsetRates.Fields.Item("U_ReduICMS").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_PrecoFix: {recordsetRates.Fields.Item("U_PrecoFix").Value}");
                        Console.WriteLine(new string('»', 6)+$"U_FatorPrc: {recordsetRates.Fields.Item("U_FatorPrc").Value}");
                        recordsetRates.MoveNext();
                    }

                    Console.WriteLine($"_____________________________________________________");
                    recordset.MoveNext();

                  
                }

                Console.WriteLine($"_____________Withholding tax________________________________________");
                var sqlWithholdingTax = GetSqlWithholdingTaxValues(docEntry);
                var recordsetWithholdingTax = company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
                recordsetWithholdingTax.DoQuery(sqlWithholdingTax);
                recordsetWithholdingTax.MoveFirst();
                while (!recordsetWithholdingTax.EoF)
                {
                    taxResponse.WithholdingTax.Add(new WithholdTax(recordsetWithholdingTax));
                    Console.WriteLine("Withholding tax details");
                    Console.WriteLine($"AbsEntry: {recordsetWithholdingTax.Fields.Item("AbsEntry").Value}");
                    Console.WriteLine($"WTCode: {recordsetWithholdingTax.Fields.Item("WTCode").Value}");
                    Console.WriteLine($"Rate: {recordsetWithholdingTax.Fields.Item("Rate").Value}");
                    Console.WriteLine($"TxblAmntSC: {recordsetWithholdingTax.Fields.Item("TxblAmntSC").Value}");
                    Console.WriteLine($"TxblAmntFC: {recordsetWithholdingTax.Fields.Item("TxblAmntFC").Value}");
                    Console.WriteLine($"WTAmnt: {recordsetWithholdingTax.Fields.Item("WTAmnt").Value}");
                    Console.WriteLine($"Account: {recordsetWithholdingTax.Fields.Item("Account").Value}");
                    Console.WriteLine("_____________________________________________________");
                    recordsetWithholdingTax.MoveNext();
                }


                Console.WriteLine($"Expected result");
                var json = taxResponse.ConvertObjectToJson();   
                LogService.Log(json);
                Console.WriteLine(json);

                company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                Console.WriteLine("Transaction canceled");

                Console.WriteLine("Do you want do open log file? yes/no");
                var openLog = Console.ReadLine();
                if (openLog.ToLower() == "yes" || openLog.ToLower() == "y")
                {
                    System.Diagnostics.Process.Start("Log.txt");
                }

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
            }
            finally
            {
                company.Disconnect();
            }

            if (company.Connected && company.InTransaction)
                company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
            
        }

        private static void DrawingMessage(int maxHeight)
        {
            for (int height = 0; height < maxHeight; height++)
            {
                for (int width = 0; width <= height; width++)
                {
                    Console.Write('*');
                }
                Console.Write("\n");
            }
            Console.ReadLine();
        }


        public static string GetSqlTaxValues(int docEntry)
        {
            return $@"
            SELECT 
                DRF4.""DocEntry"", DRF4.""LineNum"",
                DRF1.""ItemCode"",
	            DRF4.""staType"", OSTT.""Name"" AS ""TaxType"", OSTC.""Code"", OSTC.""Name"" AS ""TaxName"", 
	            DRF4.""StcCode"", DRF4.""StaCode"", DRF4.""TaxSum"", DRF4.""TaxSumFrgn"", DRF4.""BaseSumSys"", 
	            DRF4.""BaseSum"", DRF4.""DeductTax"", DRF4.""DdctTaxFrg"", DRF4.""DdctTaxSys"", DRF4.""BaseAppld"",
	            DRF4.""InGrossRev"", 
	            DRF4.""TaxInPrice"", --very important
	            DRF4.""Unencumbrd"", --very important, In Brazil called ""Desonerado"" it is similar a discount
	            DRF4.""TaxSumOrg"", DRF4.""TaxSumOrgS"", DRF4.""TaxOnRI"", DRF4.""InFirstIns"", DRF4.""ExtTaxRate"", DRF4.""ExtTaxSum"",
				DRF4.""TaxAmtSrc"", DRF4.""ExtTaxSumF"", DRF4.""U_Base"", DRF4.""U_Isento"", DRF4.""U_Outros"", DRF4.""U_Minimo"",
				DRF4.""U_Unidades"", DRF4.""U_Medida"", DRF4.""U_Moeda"", DRF4.""U_Lucro"", DRF4.""U_Reducao1"", DRF4.""U_Reducao2"", DRF4.""U_ReduICMS"", DRF4.""U_PrecoFix"", DRF4.""U_FatorPrc"", DRF4.""U_ExcAmtL"", DRF4.""U_ExcAmtF"", DRF4.""U_ExcAmtS"", DRF4.""U_OthAmtL"", DRF4.""U_OthAmtF"", DRF4.""U_OthAmtS"", DRF4.""U_TotalBL"", DRF4.""U_TotalBF"", DRF4.""U_TotalBS"" 
            FROM DRF4 
	            INNER JOIN 
            DRF1 ON DRF1.""LineNum"" = DRF4.""LineNum"" AND DRF1.""DocEntry"" = DRF4.""DocEntry"" 
	            INNER JOIN 
            OSTC ON OSTC.""Code"" = DRF1.""TaxCode"" INNER JOIN OSTT ON OSTT.""AbsId"" = DRF4.""staType"" WHERE DRF4.""DocEntry"" = {docEntry}";

        }

        public static string GetSqlWithholdingTaxValues(int docEntry)
        {
            return $@"
            select ""AbsEntry"", ""WTCode"", ""Rate"", ""TxblAmntSC"", ""TxblAmntFC"", ""TaxbleAmnt"", ""WTAmnt"", ""Account"" from DRF5 where ""AbsEntry""= {docEntry}";

        }

        public static string GetSqlTaxRates(string taxCode)
        {
            return $@"SELECT
				OSTA.""Name""
				,OSTA.""Rate""
				,OSTA.""TaxInPrice""
				,OSTA.""Exempt""
				,OSTA.""InGrossRev""
				,OSTA.""U_Base""
				,OSTA.""U_Isento""
				,OSTA.""U_Outros""
				,OSTA.""U_Minimo""
				,OSTA.""U_Unidades""
				,OSTA.""U_PrecoMin""
				,OSTA.""U_Lucro""
				,OSTA.""U_Reducao1""
				,OSTA.""U_Reducao2""
				,OSTA. ""U_ReduICMS""
				,OSTA.""U_PrecoFix""
				,OSTA.""U_FatorPrc""
			FROM 
				STC1
			INNER JOIN 
				OSTA ON OSTA.""Code"" = STC1.""STACode"" 
			INNER JOIN 
				OSTC ON OSTC.""Code"" = STC1.""STCCode"" 
			AND 
				STC1.""STAType"" = OSTA.""Type"" WHERE STC1.""STCCode"" = '{taxCode}';";
        }
    }
}
