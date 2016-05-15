//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpediteTool.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class GP_vSCMInvLatestSnapshot
    {
        public Nullable<short> CalendarID { get; set; }
        public short SCMInvSnapshotID { get; set; }
        public int SCMInvID { get; set; }
        public Nullable<System.DateTime> SCMInv_DateExtracted { get; set; }
        public string Lot { get; set; }
        public string RevLoc { get; set; }
        public string Phase { get; set; }
        public string Operation { get; set; }
        public string Die { get; set; }
        public string Family { get; set; }
        public string Transfer_Stat { get; set; }
        public string Origin { get; set; }
        public string Failure_Description { get; set; }
        public string Comments { get; set; }
        public string LTS_MPN { get; set; }
        public string SCM_MPN { get; set; }
        public string Die_Rev { get; set; }
        public string SCM_Plant_Location { get; set; }
        public string Stock_PSO { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> Start_Date { get; set; }
        public Nullable<System.DateTime> SCM_End_Date { get; set; }
        public Nullable<int> SCM_End_Date_To_CT { get; set; }
        public string SCM_Hit_Miss { get; set; }
        public Nullable<System.DateTime> Phase_Est_Date { get; set; }
        public Nullable<int> Phase_Est_Date_To_CT { get; set; }
        public string Phase_Est_Hit_Miss { get; set; }
        public Nullable<short> MPN_CT { get; set; }
        public string Receiv_Plant { get; set; }
        public string Fab_Rev { get; set; }
        public string IntDevice { get; set; }
        public string Rom_Code { get; set; }
        public string Flash { get; set; }
        public string EEPROM { get; set; }
        public string Owner { get; set; }
        public string Group_ID { get; set; }
        public string Auto { get; set; }
        public string GPC { get; set; }
        public string Package { get; set; }
        public Nullable<short> Leads { get; set; }
        public string Speed { get; set; }
        public string Type { get; set; }
        public string Bond_Type { get; set; }
        public string Asm_Location { get; set; }
        public string Test_Location { get; set; }
        public string MfgLoc { get; set; }
        public string FabLoc { get; set; }
        public string Storage_Loc { get; set; }
        public string Lot_Stat { get; set; }
        public string Comets_Part_Number { get; set; }
        public string CPN { get; set; }
        public string SL { get; set; }
        public string Mapping_Part_Number { get; set; }
        public string Last_MPN { get; set; }
        public Nullable<int> Last_Qty { get; set; }
        public string Last_Owner { get; set; }
        public string Last_Location { get; set; }
        public string Last_Loc_Type { get; set; }
        public string Last_Store { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> LastModif { get; set; }
        public string Audit_ID { get; set; }
        public Nullable<short> ABI_Start_delay { get; set; }
        public Nullable<short> OpDays { get; set; }
        public string AsmDateCd { get; set; }
        public Nullable<int> DieQty { get; set; }
        public Nullable<int> WaferQty { get; set; }
        public Nullable<int> NGD { get; set; }
        public string NGDSource { get; set; }
        public Nullable<int> DieQtyCalculated { get; set; }
        public Nullable<int> FGDieQtyCalculated { get; set; }
        public Nullable<int> WaferQtyCalculated { get; set; }
        public string PhaseWithDB { get; set; }
        public string MDL_devfam { get; set; }
        public string devfam { get; set; }
        public string DieCached { get; set; }
        public string BU { get; set; }
        public string PF { get; set; }
    }
}
