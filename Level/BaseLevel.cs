using Godot;
using Game.Characters;
using Game.Characters.Ennemies;
using Game.Objects.Spawn;
using System.Linq;
using System.Collections.Generic;

namespace Game.Level
{
    public class BaseLevel : Spatial
    {

        protected List<SpawnSettings> roundsSettings;

        protected MainCharacter mainCharacter;

        protected List<ZombieSpawn> spawns;

        private int nbrEnnemieAlive = 0;

        private int currentRound = 0;

        public override void _Ready()
        {
            this.mainCharacter = GetNode<MainCharacter>("MainCharacter");
            this.InitializeSpawn();
            this.StartNextRound();
        }

        public void InitializeSpawn()
        {
            this.spawns = GetChildren().Where(x => x is ZombieSpawn).Select(x =>
            {
                GD.Print("Got spawnn");
                var spawn = x as ZombieSpawn;
                spawn.SetMainCharacter(this.mainCharacter);
                spawn.Connect("SpawningOver", this, "_on_SpawnOver");
                spawn.Connect("EnemieSpawn", this, "_on_EnemieSpawn");
                return spawn;
            }).ToList();
        }


        public void StartNextRound()
        {
            GD.Print(this.spawns.Count);
            this.spawns.ForEach(x =>
            {
                x.Settings = this.roundsSettings[this.currentRound];
                x.Start();
            });
        }

        private void _on_SpawnOver(ZombieSpawn spawn)
        {
            this.isRoundOver();
        }

        private void _on_EnemieSpawn(Enemie enemie)
        {
            this.nbrEnnemieAlive += 1;
            enemie.Connect("EnemieDeath", this, "_on_EnemieDeath");
        }

        private void _on_EnemieDeath(Enemie enemie)
        {
            this.nbrEnnemieAlive -= 1;
            this.isRoundOver();
        }

        private bool isRoundOver()
        {
            if (this.spawns.All(x => x.isOver) && this.nbrEnnemieAlive == 0)
            {
                GD.Print("Game over");
                return true;
            }
            return false;
        }
    }
}