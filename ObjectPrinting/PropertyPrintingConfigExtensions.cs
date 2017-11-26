using System;
using System.Globalization;


namespace ObjectPrinting
{
    public static class PropertyPrintingConfigExtensions
    {
        public static string PrintToString<T>(this T obj, Func<PrintingConfig<T>, PrintingConfig<T>> config)
        {
            return config(ObjectPrinter.For<T>()).PrintToString(obj);
        }

        public static PrintingConfig<TOwner> CutToLength<TOwner>(this PropertyPrintingConfig<TOwner, string> config, int maxLen)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, string>)config)
                        .PrintingConfig;
            printingConfig.TypeSerializations[typeof(string)] = 
                s => ((string)s).Substring(0, Math.Min(((string)s).Length, maxLen));

            return printingConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, int> config, CultureInfo cultureInfo)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, int>)config)
                        .PrintingConfig;
            printingConfig.TypeSerializations[typeof(int)] = o => ((int)o).ToString(cultureInfo);

            return printingConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
    this PropertyPrintingConfig<TOwner, double> config, CultureInfo cultureInfo)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, double>)config)
                                    .PrintingConfig;
            printingConfig.TypeSerializations[typeof(double)] = o => ((double)o).ToString(cultureInfo);

            return printingConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, float> config, CultureInfo cultureInfo)
        {
            var printingConfig = ((IPropertyPrintingConfig<TOwner, float>)config)
                                    .PrintingConfig;
            printingConfig.TypeSerializations[typeof(float)] = o => ((float)o).ToString(cultureInfo);

            return printingConfig;
        }
    }
}

