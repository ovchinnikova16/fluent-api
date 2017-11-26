using System;
using NUnit.Framework.Internal;


namespace ObjectPrinting
{
    public class PropertyPrintingConfig<TOwner, TPropType> : IPropertyPrintingConfig<TOwner, TPropType>
    {
        private readonly PrintingConfig<TOwner> printingConfig;
        private string propertyName;
        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig)
        {
            this.printingConfig = printingConfig;
        }

        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig, string property)
        {
            this.printingConfig = printingConfig;
            propertyName = property;
        }
        public PrintingConfig<TOwner> Using(Func<TPropType, string> print)
        {
            if (propertyName == null) 
                PrintingConfig<TOwner>.SetTypeSerializations(printingConfig, typeof(TPropType), o => print((TPropType)o));

            else PrintingConfig<TOwner>.SetPropSerializations(printingConfig, propertyName, o => print((TPropType)o));

            return printingConfig;
        }

        PrintingConfig<TOwner> IPropertyPrintingConfig<TOwner, TPropType>.PrintingConfig => printingConfig;
    }

    public interface IPropertyPrintingConfig<TOwner, TPropType>
    {
        PrintingConfig<TOwner> PrintingConfig { get; }
    }
}
