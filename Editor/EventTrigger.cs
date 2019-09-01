using System;

namespace EasyGrid
{
    public struct EventTrigger 
    {
        public Action Function;
        public TriggerEvent Event;

        public EventTrigger(Action function, TriggerEvent @event)
        {
            Function = function;
            Event = @event;
        }
    }
}