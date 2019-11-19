using Godot;

using Game.Characters;
using Game.Characters.Ennemies;


namespace Game.Objects.Spawn
{

    public class ZombieSpawn : Spatial
    {
        [Signal]
        public delegate void SpawningOver(ZombieSpawn spawn);
        [Signal]
        public delegate void EnemieSpawn(Enemie enemie);


        public SpawnSettings Settings {
            get;set;
        }

        private MainCharacter mainCharacter;
        private PackedScene scene;
        private Timer timer;
        
        public void SetMainCharacter(MainCharacter mainCharacter)
        {
            this.mainCharacter = mainCharacter;
        }

        public bool isOver {
            get { return timer.IsStopped(); }
        }

        public override void _Ready()
        {
            scene = ResourceLoader.Load("res://Characters/Enemies/PrivateZombie/PrivateZombie.tscn") as PackedScene;
            timer = new Timer();
            timer.Connect("timeout", this, "_on_Timer_timeout");
            AddChild(timer);
        }

        public void Start()
        {
            timer.SetWaitTime(this.Settings.Timeout);
            timer.Start();
        }

        private void _on_Timer_timeout()
        {
            this.Spawn();
        }

        private void Spawn()
        {
            if (scene.CanInstance())
            {
                PrivateZombie privateZombie = (PrivateZombie)scene.Instance();
                privateZombie.SetCharacter(this.mainCharacter);
                AddChild(privateZombie);
                privateZombie.SetCharacter(this.mainCharacter);
                EmitSignal(nameof(EnemieSpawn),privateZombie);

                this.Settings.NbrToSpawn -= 1;
                if (this.Settings.NbrToSpawn <= 0)
                {
                    this.timer.Stop();
                    EmitSignal(nameof(SpawningOver), this);
                }
            }
        }

    }
}