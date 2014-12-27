using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace OLinq
{

    static class NotifyCollectionChangedUtil
    {

        public static void RaiseAddEvent<T>(Action<NotifyCollectionChangedEventArgs> raise, IEnumerable<T> newItems)
        {
            raise(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems.ToList()));
        }

        public static void RaiseAddEvent<T>(Action<NotifyCollectionChangedEventArgs> raise, T newItem)
        {
            raise(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItem, -1));
        }

        public static void RaiseRemoveEvent<T>(Action<NotifyCollectionChangedEventArgs> raise, IEnumerable<T> oldItems)
        {
            raise(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems.ToList()));
        }

        public static void RaiseRemoveEvent<T>(Action<NotifyCollectionChangedEventArgs> raise, T oldItem)
        {
            raise(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, -1));
        }

        public static void RaiseReplaceEvent<T>(Action<NotifyCollectionChangedEventArgs> raise, IEnumerable<T> oldItems, IEnumerable<T> newItems)
        {
            raise(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItems.ToList(), oldItems.ToList()));
        }

        public static void RaiseReplaceEvent<T>(Action<NotifyCollectionChangedEventArgs> raise, T oldItem, T newItem)
        {
            raise(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new [] { newItem }, new [] { oldItem }));
        }

        public static void RaiseResetEvent<T>(Action<NotifyCollectionChangedEventArgs> raise)
        {
            raise(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

    }

}
