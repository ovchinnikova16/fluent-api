using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ObjectPrinting
{
    public class PropertyPrintingConfig<TOwner, TPropType> : IPropertyPrintingConfig<TOwner, TPropType>
    {
        private readonly PrintingConfig<TOwner> printingConfig;

        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig)
        {
            this.printingConfig = printingConfig;
        }

        public PrintingConfig<TOwner> Using(Func<TPropType, string> print)
        {
            if (printingConfig.TypeSerializations.ContainsKey(typeof(TPropType)) 
                && printingConfig.TypeSerializations[typeof(TPropType)] == null)
                printingConfig.TypeSerializations[typeof(TPropType)] = o => print((TPropType)o);

            else if (printingConfig.PropSerializations.ContainsKey(printingConfig.Property))
                printingConfig.PropSerializations[printingConfig.Property] = o => print((TPropType)o);

            return printingConfig;
        }

        PrintingConfig<TOwner> IPropertyPrintingConfig<TOwner, TPropType>.PrintingConfig => printingConfig;
    }

    public interface IPropertyPrintingConfig<TOwner, TPropType>
    {
        PrintingConfig<TOwner> PrintingConfig { get; }
    }
}
