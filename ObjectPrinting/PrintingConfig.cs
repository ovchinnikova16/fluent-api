using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner>
    {
        private readonly HashSet<Type> excludedTypes = new HashSet<Type>();
        private readonly HashSet<string> excludedProps = new HashSet<string>();
        private readonly Dictionary<Type, Func<object, string>> typeSerializations 
            = new Dictionary<Type, Func<object, string>>();
        private readonly Dictionary<string, Func<object, string>> propSerializations 
            = new Dictionary<string, Func<object, string>>();


        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>()
        {
            return new PropertyPrintingConfig<TOwner, TPropType>(this);
        }

        public static PrintingConfig<TOwner> SetTypeSerializations(
            PrintingConfig<TOwner> printingConfig, Type type, Func<object, string> function)
        {
            printingConfig.typeSerializations[type] = function;
            return printingConfig;
        }

        public static PrintingConfig<TOwner> SetPropSerializations(PrintingConfig<TOwner> printingConfig, string name, Func<object, string> function)
        {
            printingConfig.propSerializations[name] = function;
            return printingConfig;
        }

        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>(Expression<Func<TOwner, TPropType>> memberSelector)
        {
            var property = ((MemberExpression) memberSelector.Body).Member.Name;
            return new PropertyPrintingConfig<TOwner, TPropType>(this, property);
        }

        public PrintingConfig<TOwner> Excluding<TPropType>(Expression<Func<TOwner, TPropType>> memberSelector)
        {
            excludedProps.Add(((MemberExpression)memberSelector.Body).Member.Name);
            return this;
        }

        internal PrintingConfig<TOwner> Excluding<TPropType>()
        {
            excludedTypes.Add(typeof(TPropType));
            return this;
        }

        public string PrintToString(TOwner obj)
        {
            return PrintToString(obj, 0);
        }

        private string PrintToString(object obj, int nestingLevel)
        {
            if (obj == null)
                return "null" + Environment.NewLine;

            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (finalTypes.Contains(obj.GetType()))
                return obj + Environment.NewLine;

            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();
            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties())
            {
                if (typeSerializations.ContainsKey(propertyInfo.PropertyType))
                    sb.Append(identation + propertyInfo.Name + " = "
                        + typeSerializations[propertyInfo.PropertyType](propertyInfo.GetValue(obj))
                        + Environment.NewLine);

                else if (propSerializations.ContainsKey(propertyInfo.Name))
                    sb.Append(identation + propertyInfo.Name + " = "
                        + propSerializations[propertyInfo.Name](propertyInfo.GetValue(obj))
                        + Environment.NewLine);

                else if (!excludedTypes.Contains(propertyInfo.PropertyType) 
                            && !excludedProps.Contains(propertyInfo.Name))
                {
                    sb.Append(identation + propertyInfo.Name + " = " +
                                PrintToString(propertyInfo.GetValue(obj),
                                nestingLevel + 1));
                }
            }
            return sb.ToString();
        }
    }
}