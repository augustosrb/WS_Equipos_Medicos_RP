using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_EquiposMedicos.Controllers
{
    public class ELResultado
    {
        [ELColumn("cDato")]
        public string cDato { get; set; }

        [ELColumn("cValor")]
        public string cValor { get; set; }

        [ELColumn("cEstado")]
        public string cEstado { get; set; }

        [ELColumn("cRangos")]
        public string cRangos { get; set; }

    }
}