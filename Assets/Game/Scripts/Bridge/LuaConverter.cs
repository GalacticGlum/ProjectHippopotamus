using System;
using MoonSharp.Interpreter;

internal static class LuaConverter
{
    public static void RegisterConvertToFunc<T>()
    {
        Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Func<T>),
            converter =>
            {
                Closure function = converter.Function;
                return (Func<T>)(() => function.Call().ToObject<T>());
            });
    }

    public static void RegisterConvertToFunc<T1, TResult>()
    {
        Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Func<T1, TResult>),
        converter =>
        {
            Closure function = converter.Function;
            return (Func<T1, TResult>)(t1 => function.Call(t1).ToObject<TResult>());
        });
    }

    public static void RegisterConvertToFunc<T1, T2, TResult>()
    {
        Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Func<T1, T2, TResult>),
        converter =>
        {
            Closure function = converter.Function;
            return (Func<T1, T2, TResult>)((t1, t2) => function.Call(t1, t2).ToObject<TResult>());
        });
    }

    public static void RegisterConvertToFunc<T1, T2, T3, TResult>()
    {
        Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Func<T1, T2, T3, TResult>),
        converter =>
        {
            Closure function = converter.Function;
            return (Func<T1, T2, T3, TResult>)((t1, t2, t3) => function.Call(t1, t2, t3).ToObject<TResult>());
        });
    }

    public static void RegisterConvertToFunc<T1, T2, T3, T4, TResult>()
    {
        Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Function, typeof(Func<T1, T2, T3, T4, TResult>),
        converter =>
        {
            Closure function = converter.Function;
            return (Func<T1, T2, T3, T4, TResult>)((t1, t2, t3, t4) => function.Call(t1, t2, t3, t4).ToObject<TResult>());
        });
    }
}
