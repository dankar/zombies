using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Map_Editor
{
    [Serializable()]
    public enum LayerEnum
    {
        Layer1,
        Layer2,
        Layer3,
        Layer4,
        Layer5
    }

    public class LayerEnumConverter : TypeConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        /*public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
        return true; 
        }*/
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[]{"Layer 1","Layer 2","Layer 3","Layer 4","Layer 5"});
        }
    }
}
