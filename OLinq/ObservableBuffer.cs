using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace OLinq
{

    public sealed class ObservableBuffer<TElement> : ObservableCollection<TElement>
    {

        ObservableView<TElement> view;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="view"></param>
        internal ObservableBuffer(ObservableView<TElement> view)
        {

            this.view = view;

            // subscribe to notifications
            view.CollectionChanged += view_CollectionChanged;
            // reset buffer items
            Reset();
        }

        /// <summary>
        /// Invoked when the view collection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void view_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
#if !SILVERLIGHT
                case NotifyCollectionChangedAction.Move:
#endif
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:

                    Reset();
                    OnCollectionChanged(args);
                    break;
                case NotifyCollectionChangedAction.Add:
                    // add new items
                    if (args.NewStartingIndex == -1)
                    {
                        args = new NotifyCollectionChangedEventArgs(args.Action, args.NewItems, Items.Count);
                        foreach (TElement item in args.NewItems)
                            Items.Add(item);
                    }
                    else
                    {
                        int index = args.NewStartingIndex;
                        foreach (TElement item in args.NewItems)
                            Items.Insert(index++, item);
                    }
                    OnCollectionChanged(args);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    // remove old items
                    if (args.OldStartingIndex == -1)
                    {
                        foreach (TElement item in args.OldItems)
                            Remove(item);
                    }
                    else
                    {
                        for (int index = 0; index < args.OldItems.Count; index++)
                            Items.RemoveAt(args.OldStartingIndex);
                        OnCollectionChanged(args);
                    }
                    break;
            }
        }

        /// <summary>
        /// Resets the buffered collection based on the underlying list.
        /// </summary>
        void Reset()
        {
            Items.Clear();
            foreach (var element in view)
                Items.Add(element);
        }


        /// <summary>
        /// Gets the associated view.
        /// </summary>
        public ObservableView<TElement> View
        {
            get { return view; }
        }
    }

}
