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

        private Spatial world;
       
        public override void _Ready()
        {
            scene = ResourceLoader.Load("res://Characters/Enemies/PrivateZombie/PrivateZombie.tscn") as PackedScene;
            this.world = GetTree().GetRoot().GetNode<Spatial>("Spatial");
        }

        public void Spawn(MainCharacter mainCharacter, BaseLevelNavMesh navigation)
        {
            if (scene.CanInstance())
            {
                PrivateZombie privateZombie = (PrivateZombie)scene.Instance();
                privateZombie.SetTranslation(this.Translation);
                privateZombie.SetScale(new Vector3(0.5f,0.5f,0.5f));
                this.world.AddChild(privateZombie);

                var (_,d,w) = navigation.GetPathClosestWindows(privateZombie);
                privateZombie.SetTarget(w);
            }
        }

    }
}