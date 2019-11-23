using Godot;
using System.Collections.Generic;
using System.Linq;
using Game.Characters;
using Game.Objects.Windows;
using Game.Objects.Spawn;

namespace Game.Objects.Navmesh
{
    public class BaseLevelNavMesh : Navigation
    {


        public MainCharacter mainCharacter;

        public List<BaseWindows> windows;


        public ImmediateGeometry draw;

        private SpatialMaterial material = new SpatialMaterial();

        public (float, Vector3[], BaseWindows) GetPathClosestWindows(Spatial item)
        {
            return this.windows.Select(x =>
            {
                var (distance, paths) = this.GetSimplePathAndDistance(item, x);
                GD.Print(distance);
                return (distance, paths, x);
            }).OrderBy(x => x.distance).First();
        }

        public List<(float,ZombieSpawn)> OrderSpawnByDistance(List<ZombieSpawn> spawns)
        {
            return spawns.Select(x => {
                var distance = x.GetTranslation().DistanceTo(mainCharacter.GetTranslation());
                return (distance,x);
            }).OrderBy(x => x.distance).ToList();
        }

    }
}
