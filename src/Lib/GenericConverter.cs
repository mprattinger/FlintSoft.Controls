﻿using System.ComponentModel;

namespace FlintSoft.Controls;

public static class GenericConverter
{
    public static T ChangeType<T>(object value)
    {
        return (T)ChangeType(typeof(T), value);
    }

    public static object ChangeType(Type t, object value)
    {
        TypeConverter tc = TypeDescriptor.GetConverter(t);
        return tc.ConvertFrom(value) ?? default!;
    }

    public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
    {

        TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
    }
}