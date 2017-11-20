using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPrinting
{
    public static class PropertyPrintingConfigExtensions
    {
        public static string PrintToString<T>(this T obj, Func<PrintingConfig<T>, PrintingConfig<T>> config)
        {
            return config(ObjectPrinter.For<T>()).PrintToString(obj);
        }

        public static PrintingConfig<TOwner> TrimmedToLength<TOwner>(this PropertyPrintingConfig<TOwner, string> propConfig, int maxLen)
        {
            return ((IPropertyPrintingConfig<TOwner, string>)propConfig).ParentConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, int> propertyPrintingConfig, CultureInfo cultureInfo)
        {
            return ((IPropertyPrintingConfig<TOwner, int>)propertyPrintingConfig)
                   .ParentConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
    this PropertyPrintingConfig<TOwner, double> propertyPrintingConfig, CultureInfo cultureInfo)
        {
            return ((IPropertyPrintingConfig<TOwner, double>)propertyPrintingConfig)
                   .ParentConfig;
        }

        public static PrintingConfig<TOwner> Using<TOwner>(
            this PropertyPrintingConfig<TOwner, float> propertyPrintingConfig, CultureInfo cultureInfo)
        {
            return ((IPropertyPrintingConfig<TOwner, float>)propertyPrintingConfig)
                   .ParentConfig;
        }
    }
}

