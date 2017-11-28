using System;


namespace ObjectPrinting
{
    public class PropertyPrintingConfig<TOwner, TPropType> : IPropertyPrintingConfig<TOwner, TPropType>
    {
        private PrintingConfig<TOwner> printingConfig;
        public readonly string PropertyName;

        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig, string property = null)
        {
            this.printingConfig = printingConfig;
            PropertyName = property;
        }
        public PrintingConfig<TOwner> Using(Func<TPropType, string> print)
        {
            if (PropertyName == null) 
                printingConfig = PrintingConfig<TOwner>.SetTypeSerializations(
                    printingConfig, typeof(TPropType), o => print((TPropType)o));

            else
                printingConfig = PrintingConfig<TOwner>.SetPropSerializations(
                    printingConfig, PropertyName, o => print((TPropType)o));

            return printingConfig;
        }

        PrintingConfig<TOwner> IPropertyPrintingConfig<TOwner, TPropType>.PrintingConfig => printingConfig;
    }

    public interface IPropertyPrintingConfig<TOwner, TPropType>
    {
        PrintingConfig<TOwner> PrintingConfig { get; }
    }
}
