using System;
using R3;

public static class R3BindingExtensions
{
    public static void BindTo<T>(this ReactiveProperty<T> source, Action<T> setter, CompositeDisposable disposables)
    {
        setter(source.Value);
        disposables.Add(source.Subscribe(setter));
    }
}
