using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WS_EquiposMedicos.Controllers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ELColumn : Attribute
    {
        string _name;
        public ELColumn(string name)
        {
            this._name = name;
        }
        public string Name
        {
            get { return this._name; }
        }
    }
}