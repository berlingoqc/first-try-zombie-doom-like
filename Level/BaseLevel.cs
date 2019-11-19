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

        protected SpawnSettings roundsSettings;

        protected MainCharacter mainCharacter;

        protected List<ZombieSpawn> spawns;

        private int nbrEnnemieAlive = 0;

        private int currentRound = 0;

        public override void _Ready()
        {
            this.mainCharacter = GetNode<MainCharacter>("MainCharacter");
            this.mainCharacter.Connect(nameof(MainCharacter.MainCharacterDie), this, "_on_MainCharacter_Death");
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
            var settings = new SpawnSettings();
            settings.NbrToSpawn = roundsSettings.NbrToSpawn * (this.currentRound + 1);
            settings.Timeout = roundsSettings.Timeout - ((this.currentRound + 1) / 10.0f);
            settings.Timeout = (settings.Timeout < 1) ? 1 : settings.Timeout;
            this.spawns.ForEach(x =>
            {
                x.Settings = new SpawnSettings();
                x.Settings.NbrToSpawn = settings.NbrToSpawn;
                x.Settings.Timeout = settings.Timeout;
                x.Start();
            });

            this.mainCharacter.labelMessage.AddMessage($"ROUND {currentRound+1} STARTING");

            GD.Print("ROUND ", currentRound, " NBR SPAWN ", settings.NbrToSpawn, " TIMEOUT ", settings.Timeout);

        }


        private void _on_MainCharacter_Death(MainCharacter mainCharacter)
        {
            GD.Print("Main character dieee");
            Input.SetMouseMode(Input.MouseMode.Visible);
            this.mainCharacter.uiGameOver.ShowDialog(this.mainCharacter.Kills, this.currentRound);
            this.mainCharacter.canvasLayer.QueueFree();
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
            this.mainCharacter.Kills += 1;
            this.isRoundOver();
        }

        private bool isRoundOver()
        {
            if (this.spawns.All(x => x.isOver) && this.nbrEnnemieAlive == 0)
            {
                this.currentRound += 1;
                this.StartNextRound();
                return true;
            }
            return false;
        }
    }
}