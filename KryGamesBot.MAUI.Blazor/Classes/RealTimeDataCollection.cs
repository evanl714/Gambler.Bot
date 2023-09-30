using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.MAUI.Blazor.Classes
{
    public class RealTimeDataCollection : ObservableCollection<SimpleDataPoint>
    {
        public void AddRange(IList<SimpleDataPoint> items)
        {
            foreach (SimpleDataPoint item in items)
                Items.Add(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (IList)items, Items.Count - items.Count));
        }
        public void RemoveRangeAt(int startingIndex, int count)
        {
            var removedItems = new List<SimpleDataPoint>(count);
            for (int i = 0; i < count; i++)
            {
                removedItems.Add(Items[startingIndex]);
                Items.RemoveAt(startingIndex);
            }
            if (count > 0)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems, startingIndex));
        }
    }
}
