using LiveChartsCore.Kernel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace KryGamesBot.Ava.Classes.Charts
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
    public class SimpleDataPoint : IChartEntity
    {

        public double Argument { get; private set; }
        public double Value { get; private set; }
        public bool Win { get; set; }
        public ChartEntityMetaData? MetaData { get ; set ; }
        public Coordinate Coordinate { get ; set; }

        public SimpleDataPoint(double arg, double val, bool win)
        {
            Argument = arg;
            Value = val;
            Win = win;
            Coordinate = new Coordinate(arg, val);
            
        }
    }
}
