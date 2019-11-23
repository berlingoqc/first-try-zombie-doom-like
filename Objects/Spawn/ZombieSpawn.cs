using Godot;
using System.Collections.Generic;
using Game.Characters;
using Game.Characters.Ennemies;
using Game.Objects.Navmesh;


namespace Game.Objects.Spawn
{

    public class ZombieSpawn : Spatial
    {
        private PackedScene scene;
       
        public override void _Ready()
        {
            scene = ResourceLoader.Load("res://Characters/Enemies/PrivateZombie/PrivateZombie.tscn") as PackedScene;
        }

        public void Spawn(MainCharacter mainCharacter, BaseLevelNavMesh navigation)
        {
            if (scene.CanInstance())
            {
                PrivateZombie privateZombie = (PrivateZombie)scene.Instance();
                privateZombie.SetCharacter(mainCharacter);
                privateZombie.SetScale(new Vector3(0.5f,0.5f,0.5f));
                AddChild(privateZombie);

                var (_,d,w) = navigation.GetPathClosestWindows(privateZombie);
                GD.Print("Going to ",w.ID);
                privateZombie.SetPath(new List<Vector3>(d));
            }
        }

    }
}