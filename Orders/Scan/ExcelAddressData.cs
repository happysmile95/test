using System;
using System.Collections.Generic;
using System.Text;

namespace Scan
{
    public static class ExcelAddressData
    {
        public const string SoldTo = "A";
        public static readonly string CustName = "B";
        public static readonly string ShipTo = "C";
        public static readonly string ShipToNa = "D";
        public static readonly string OrderType = "E";
        public static readonly string DV = "F";
        public static readonly string OrderNum = "G";
        public static readonly string Material = "H";
        public static readonly string MatDes = "I";
        public static readonly string Size = "J";
        public static readonly string AltSize = "K";
        public static readonly string OnOrdQty = "L";
        public static readonly string ShipQty = "M";
        public static readonly string RejecQty = "N";
    }

    public enum ExcelColumnNumber
    {
        SoldTo = 1,
        CustName,
        ShipTo,
        ShipToNa,
        OrderType,
        DV,
        OrderNum,
        Material,
        MatDes,
        Size,
        AltSize,
        OnOrdQty,
        ShipQty,
        RejecQty,
}
}
