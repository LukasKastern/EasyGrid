using System.Collections.Generic;
using System.Linq;

namespace EasyGrid
{
    /// <summary>
    /// The Hierarchy is used to keep Track of all Graphic Objects
    /// </summary>
    public static class Hierarchy
    {
        private static readonly List<GridGraphic> Graphics = new List<GridGraphic>();

        public static IEnumerable<GridGraphic> GetGraphics()
        {
            foreach (var graphic in Graphics)
            {
                yield return graphic;
            }
        }
    
        public static void AddGraphic(GridGraphic graphicToAdd)
        {
            Graphics.Add(graphicToAdd);
        }

        public static void RemoveGraphic(GridGraphic graphicToRemove)
        {
            Graphics.Remove(graphicToRemove);
        }

        public static IEnumerable<GridGraphic> GetGraphicsInArea(CellBounds area)
        {
            return GetGraphics().Where(graphic => graphic.Bounds.Intersects(area));
        }

        public static IEnumerable<GridEventTrigger> GetActiveEventTriggers()
        {
            return GetGraphics().Where(i => i.IsInteractable).Select(graphic => graphic.EventTrigger);
        }
    }
}