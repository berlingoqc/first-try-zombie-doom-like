using Godot;

namespace Game.Objects.Navmesh
{
    static class NavigationUtils
    {

        public static (float, Vector3[]) GetSimplePathAndDistance(this Navigation navigation, Spatial item, Spatial destination)
        {
            /*var itemTranslation = item.GetTranslation();
            var paths = navigation.GetSimplePath(itemTranslation, destination.GetTranslation());
            float distance = 0;
            Vector3 lastPoint = itemTranslation;
            foreach (var p in paths)
            {
                distance += lastPoint.DistanceTo(p);
                lastPoint = p;
            }
            GD.Print("DISTANCE ", distance);
            */
            return (item.Translation.DistanceTo(destination.Translation), null);
        }
    }

}