using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyGrid
{
    [System.Serializable]
    public class GridEventTrigger
    {
        private List<EventTrigger> entries = new List<EventTrigger>();

        public void AddEventTrigger(TriggerEvent type, Action action)
        {
            entries.Add(new EventTrigger(action, type));
        }

        public void Invoke(TriggerEvent typeToInvoke)
        {
            foreach (var entry in entries.Where(i => i.Event == typeToInvoke))
            {
                entry.Function?.DynamicInvoke();
            }
        }
    }
}