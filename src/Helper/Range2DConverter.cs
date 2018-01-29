// <copyright file="Range2DConverter.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia.
// </copyright>

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace Helper
{
    public class Range2DConverter : TypeConverter
    {
        public override bool CanConvertFrom(
            ITypeDescriptorContext context,
            Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(
            ITypeDescriptorContext context,
            Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value)
        {
            if (!(value is string text))
            {
                return base.ConvertFrom(context, culture, value);
            }

            text = text.Trim();

            if (text.Length == 0)
            {
                return null;
            }

            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }

            var separators = culture.TextInfo.ListSeparator[0];
            var tokens = text.Split(new char[] { separators });
            var values = new int[tokens.Length];
            var intConverter = TypeDescriptor.GetConverter(typeof(int));
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = (int)intConverter.ConvertFromString(
                    context,
                    culture,
                    tokens[i]);
            }

            if (values.Length != 2)
            {
                throw new ArgumentException(nameof(text));
            }

            return new Range2D(values[0], values[1]);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException(nameof(destinationType));
            }

            if (!(value is Range2D range))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            if (destinationType == typeof(string))
            {
                if (culture == null)
                {
                    culture = SR.CurrentCulture;
                }

                var sep = culture.TextInfo.ListSeparator + " ";
                var intConverter = TypeDescriptor.GetConverter(typeof(int));
                var args = new string[2];
                var nArg = 0;

                args[nArg++] = intConverter.ConvertToString(
                    context,
                    culture,
                    range.Width);

                args[nArg++] = intConverter.ConvertToString(
                    context,
                    culture,
                    range.Height);

                return String.Join(sep, args);
            }

            if (destinationType != typeof(InstanceDescriptor))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            var @params = new Type[] { typeof(int), typeof(int) };
            var ctor = typeof(Range2D).GetConstructor(@params);

            if (ctor != null)
            {
                var args = new object[] { range.Width, range.Height };
                return new InstanceDescriptor(ctor, args);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(
            ITypeDescriptorContext context,
            IDictionary propertyValues)
        {
            if (propertyValues == null)
            {
                throw new ArgumentNullException(nameof(propertyValues));
            }

            var width = propertyValues[nameof(Range2D.Width)];
            var height = propertyValues[nameof(Range2D.Height)];

            if (width == null || !(width is int w))
            {
                throw new ArgumentException(nameof(Range2D.Width));
            }

            if (height == null || !(height is int h))
            {
                throw new ArgumentException(nameof(Range2D.Height));
            }

            return new Range2D(w, h);
        }

        public override bool GetCreateInstanceSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(
            ITypeDescriptorContext context,
            object value,
            Attribute[] attributes)
        {
            var props = TypeDescriptor.GetProperties(
                typeof(Range2D),
                attributes);

            var names = new string[] {
                nameof(Range2D.Width),
                nameof(Range2D.Height)};

            return props.Sort(names);
        }

        public override bool GetPropertiesSupported(
            ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
