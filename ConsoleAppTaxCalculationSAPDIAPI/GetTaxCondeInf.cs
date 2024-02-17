using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsoleAppTaxCalculationSAPDIAPI
{
    [DataContract]
    public class TaxResponse
    {
        public TaxResponse()
        {
            this.TaxInformations = new List<TaxCodeInf>();
        }
        [DataMember]
        public List<TaxCodeInf> TaxInformations { get; set; } = new List<TaxCodeInf>();
        [DataMember]
        public List<WithholdTax> WithholdingTax { get; set; } = new List<WithholdTax>();
        public void AddTaxInformation(TaxCodeInf taxCodeInf)
        {
            this.TaxInformations.Add(taxCodeInf);
        }

        public void AddWithholdingTax(WithholdTax withholdTax)
        {
            this.WithholdingTax.Add(withholdTax);
        }   
    }

    [DataContract]
    public class TaxCodeInf
    {
        private Recordset recordset;
        public TaxCodeInf(Recordset recordset)
        {
            this.SetFields(recordset);
        }

        private void SetFields(Recordset recordset)
        {
            this.ItemCode = recordset.Fields.Item("ItemCode").Value.ToString();
            this.LineNum = Convert.ToInt32(recordset.Fields.Item("LineNum").Value.ToString());
            this.TaxType = recordset.Fields.Item("TaxType").Value.ToString();
            this.TaxName = recordset.Fields.Item("TaxName").Value.ToString();
            this.BaseSum = Convert.ToDecimal(recordset.Fields.Item("BaseSum").Value.ToString());
            this.DeductTax = Convert.ToDecimal(recordset.Fields.Item("DeductTax").Value.ToString());
            this.DdctTaxFrg = Convert.ToDecimal(recordset.Fields.Item("DdctTaxFrg").Value.ToString());
            this.InGrossRev = recordset.Fields.Item("InGrossRev").Value.ToString();
            this.TaxInPrice = recordset.Fields.Item("TaxInPrice").Value.ToString();
            this.Unencumbrd = recordset.Fields.Item("Unencumbrd").Value.ToString();
            this.TaxSumOrg = Convert.ToDecimal(recordset.Fields.Item("TaxSumOrg").Value.ToString());
            this.TaxSumOrgS = Convert.ToDecimal(recordset.Fields.Item("TaxSumOrgS").Value.ToString());
            this.TaxOnRI = recordset.Fields.Item("TaxOnRI").Value.ToString();
            this.InFirstIns = recordset.Fields.Item("InFirstIns").Value.ToString();
            this.ExtTaxRate = Convert.ToDecimal(recordset.Fields.Item("ExtTaxRate").Value.ToString());
            this.ExtTaxSum = Convert.ToDecimal(recordset.Fields.Item("ExtTaxSum").Value.ToString());
            this.TaxAmtSrc = recordset.Fields.Item("TaxAmtSrc").Value.ToString();
            this.ExtTaxSumF = Convert.ToDecimal(recordset.Fields.Item("ExtTaxSumF").Value.ToString());
            this.U_Base = Convert.ToDecimal(recordset.Fields.Item("U_Base").Value.ToString());
            this.U_Isento = Convert.ToDecimal(recordset.Fields.Item("U_Isento").Value.ToString());
            this.U_Outros = Convert.ToDecimal(recordset.Fields.Item("U_Outros").Value.ToString());
            this.U_Minimo = Convert.ToDecimal(recordset.Fields.Item("U_Minimo").Value.ToString());
            this.U_Unidades = Convert.ToDecimal(recordset.Fields.Item("U_Unidades").Value.ToString());

            this.U_Lucro = Convert.ToDecimal(recordset.Fields.Item("U_Lucro").Value.ToString());
            this.U_Reducao1 = Convert.ToDecimal(recordset.Fields.Item("U_Reducao1").Value.ToString());
            this.U_Reducao2 = Convert.ToDecimal(recordset.Fields.Item("U_Reducao2").Value.ToString());
            this.U_ReduICMS = Convert.ToDecimal(recordset.Fields.Item("U_ReduICMS").Value.ToString());
            this.U_PrecoFix = Convert.ToDecimal(recordset.Fields.Item("U_PrecoFix").Value.ToString());
            this.U_FatorPrc = Convert.ToDecimal(recordset.Fields.Item("U_FatorPrc").Value.ToString());
            this.U_ExcAmtL = Convert.ToDecimal(recordset.Fields.Item("U_ExcAmtL").Value.ToString());
            this.U_ExcAmtF = Convert.ToDecimal(recordset.Fields.Item("U_ExcAmtF").Value.ToString());
            this.U_ExcAmtS = Convert.ToDecimal(recordset.Fields.Item("U_ExcAmtS").Value.ToString());
            this.U_OthAmtL = Convert.ToDecimal(recordset.Fields.Item("U_OthAmtL").Value.ToString());
            this.U_OthAmtF = Convert.ToDecimal(recordset.Fields.Item("U_OthAmtF").Value.ToString());
            this.U_OthAmtS = Convert.ToDecimal(recordset.Fields.Item("U_OthAmtS").Value.ToString());
            this.U_TotalBL = Convert.ToDecimal(recordset.Fields.Item("U_TotalBL").Value.ToString());
            this.U_TotalBF = Convert.ToDecimal(recordset.Fields.Item("U_TotalBF").Value.ToString());
            this.U_TotalBS = Convert.ToDecimal(recordset.Fields.Item("U_TotalBS").Value.ToString());
        }

        [DataMember] public string ItemCode { get; set; }
        [DataMember] public int LineNum { get; set; }
        [DataMember] public string TaxType { get; set; }
        [DataMember] public string TaxName { get; set; }
        [DataMember] public decimal BaseSum { get; set; }
        [DataMember] public decimal DeductTax { get; set; }
        [DataMember] public decimal DdctTaxFrg { get; set; }
        [DataMember] public string InGrossRev { get; set; }
        [DataMember] public string TaxInPrice { get; set; }
        [DataMember] public string Unencumbrd { get; set; }
        [DataMember] public decimal TaxSumOrg { get; set; }

        [DataMember] public decimal TaxSumOrgS { get; set; }

        [DataMember] public string TaxOnRI { get; set; }
        [DataMember] public string InFirstIns { get; set; }
        [DataMember] public decimal ExtTaxRate { get; set; }
        [DataMember] public decimal ExtTaxSum { get; set; }
        [DataMember] public string TaxAmtSrc { get; set; }
        [DataMember] public decimal ExtTaxSumF { get; set; }
        [DataMember] public decimal U_Base { get; set; }
        [DataMember] public decimal U_Isento { get; set; }
        [DataMember] public decimal U_Outros { get; set; }
        [DataMember] public decimal U_Minimo { get; set; }
        [DataMember] public decimal U_Unidades { get; set; }

        [DataMember] public decimal U_Lucro { get; set; }
        [DataMember] public decimal U_Reducao1 { get; set; }
        [DataMember] public decimal U_Reducao2 { get; set; }
        [DataMember] public decimal U_ReduICMS { get; set; }
        [DataMember] public decimal U_PrecoFix { get; set; }
        [DataMember] public decimal U_FatorPrc { get; set; }
        [DataMember] public decimal U_ExcAmtL { get; set; }
        [DataMember] public decimal U_ExcAmtF { get; set; }
        [DataMember] public decimal U_ExcAmtS { get; set; }
        [DataMember] public decimal U_OthAmtL { get; set; }
        [DataMember] public decimal U_OthAmtF { get; set; }
        [DataMember] public decimal U_OthAmtS { get; set; }
        [DataMember] public decimal U_TotalBL { get; set; }
        [DataMember] public decimal U_TotalBF { get; set; }

        [DataMember] public decimal U_TotalBS { get; set; }
    }

    public class GetTaxCondeInfDefail
    {
        public GetTaxCondeInfDefail(Recordset recordset)
        {

        }
    }




    [DataContract]
    public class WithholdTax
    {
        public  WithholdTax(Recordset recordset)
        {
            this.AbsEntry = Convert.ToInt32(recordset.Fields.Item("AbsEntry").Value.ToString());
            this.WTCode = recordset.Fields.Item("WTCode").Value.ToString();
            this.Rate = Convert.ToDecimal(recordset.Fields.Item("Rate").Value.ToString());
            this.TaxbleAmnt = Convert.ToDecimal(recordset.Fields.Item("TaxbleAmnt").Value.ToString());
            this.TxblAmntSC = Convert.ToDecimal(recordset.Fields.Item("TxblAmntSC").Value.ToString());
            this.TxblAmntFC = Convert.ToDecimal(recordset.Fields.Item("TxblAmntFC").Value.ToString());
            this.WTAmnt = Convert.ToDecimal(recordset.Fields.Item("WTAmnt").Value.ToString());
            this.Account = recordset.Fields.Item("Account").Value.ToString();
            

        }
        [DataMember]
        public int AbsEntry { get; set; }

        [DataMember]
        public string WTCode { get; set; }

        [DataMember]
        public decimal? Rate { get; set; }

        [DataMember]
        public decimal? TaxbleAmnt { get; set; }

        [DataMember]
        public decimal? TxblAmntSC { get; set; }

        [DataMember]
        public decimal? TxblAmntFC { get; set; }

        [DataMember]
        public decimal? WTAmnt { get; set; }

        [DataMember]
        public decimal? WTAmntSC { get; set; }

        [DataMember]
        public decimal? WTAmntFC { get; set; }

        [DataMember]
        public decimal? ApplAmnt { get; set; }

        [DataMember]
        public decimal? ApplAmntSC { get; set; }

        [DataMember]
        public decimal? ApplAmntFC { get; set; }

        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public string Criteria { get; set; }

        [DataMember]
        public string Account { get; set; }

       
        
       


    }

}