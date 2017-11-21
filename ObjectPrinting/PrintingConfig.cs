using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner>
    {
        private HashSet<Type> ExcludedTypes = new HashSet<Type>();
        private HashSet<string> ExcludedProps = new HashSet<string>();
        public Dictionary<Type, Func<object, string>> TypeSerializations 
            = new Dictionary<Type, Func<object, string>>();
        public Dictionary<string, Func<object, string>> PropSerializations 
            = new Dictionary<string, Func<object, string>>();
        public string Property;

        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>()
        {
            TypeSerializations[typeof(TPropType)] = null;
            return new PropertyPrintingConfig<TOwner, TPropType>(this);
        }

        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>(Expression<Func<TOwner, TPropType>> memberSelector)
        {
            Property = ((MemberExpression) memberSelector.Body).Member.Name;
            PropSerializations[Property] = null;
            return new PropertyPrintingConfig<TOwner, TPropType>(this);
        }

        public PrintingConfig<TOwner> Excluding<TPropType>(Expression<Func<TOwner, TPropType>> memberSelector)
        {
            ExcludedProps.Add(((MemberExpression)memberSelector.Body).Member.Name);
            return this;
        }

        internal PrintingConfig<TOwner> Excluding<TPropType>()
        {
            ExcludedTypes.Add(typeof(TPropType));
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
                if (TypeSerializations.ContainsKey(propertyInfo.PropertyType))
                    sb.Append(identation + propertyInfo.Name + " = "
                        + TypeSerializations[propertyInfo.PropertyType](propertyInfo.GetValue(obj))
                        + Environment.NewLine);

                else if (PropSerializations.ContainsKey(propertyInfo.Name))
                    sb.Append(identation + propertyInfo.Name + " = "
                        + PropSerializations[propertyInfo.Name](propertyInfo.GetValue(obj))
                        + Environment.NewLine);

                else if (!ExcludedTypes.Contains(propertyInfo.PropertyType) 
                            && !ExcludedProps.Contains(propertyInfo.Name))
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