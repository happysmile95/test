using System;
using System.Collections.Generic;
using System.Text;

namespace Scan
{
    public class GoodDto
    {
        public Guid Id { get; set; }

        public string SoldTo { get; set; }

        public string CustName { get; set; }

        public string ShipTo { get; set; }

        public string ShipToNa { get; set; }

        public string OrderType { get; set; }

        public string Dv { get; set; }

        public string OrderNum { get; set; }

        public string Material { get; set; }

        public string MatDes { get; set; }

        public string Size { get; set; }

        public string AltSize { get; set; }

        public int OnOrdQty { get; set; }

        public int ShipQty { get; set; }

        public int RejectQty { get; set; }
    }
}
