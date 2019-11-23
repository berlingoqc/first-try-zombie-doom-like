using Godot;
using Game.Characters;
using Game.Characters.Ennemies;
using Game.Objects.Spawn;
using Game.Objects.Navmesh;
using Game.Objects.Windows;
using System.Linq;
using System.Collections.Generic;

namespace Game.Level
{
    public class BaseLevel : Spatial
    {

        protected SpawnSettings roundsSettings;

        protected MainCharacter mainCharacter;

        protected List<ZombieSpawn> spawns;

        protected List<BaseWindows> windows;

        protected BaseLevelNavMesh navMesh;

        protected SpawnManager spawnManager;

        private int nbrEnnemieAlive = 0;

        private int currentRound = 0;

        public override void _Ready()
        {
            this.mainCharacter = GetNode<MainCharacter>("MainCharacter");
            this.mainCharacter.Connect(nameof(MainCharacter.MainCharacterDie), this, "_on_MainCharacter_Death");

            this.InitiazeWindows();
            this.InitializeNavMesh();
            this.InitializeSpawn();
            this.spawnManager.StartRound();
        }

        public void InitializeSpawn()
        {
            this.spawnManager = GetNode<SpawnManager>("SpawnManager");
            this.spawns = GetChildren().Where(x => x is ZombieSpawn).Select(x =>
            {
                var spawn = x as ZombieSpawn;
                return spawn;
            }).ToList();


            this.spawnManager.AvailableSpawn = this.spawns;
            this.spawnManager.Settings = this.roundsSettings;
            this.spawnManager.MainCharacter = this.mainCharacter;
            this.spawnManager.navMesh = this.navMesh;
        }

        private void InitiazeWindows()
        {
            this.windows = GetChildren().Where(x => x is BaseWindows).Select(x =>
            {
                var window = x as BaseWindows;
                return window;
            }).ToList();
        }

        private void InitializeNavMesh()
        {
            this.navMesh = GetNode<BaseLevelNavMesh>("Level");
            this.navMesh.draw = GetNode<ImmediateGeometry>("Draw");
            this.navMesh.mainCharacter = mainCharacter;
            this.navMesh.windows = windows;
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
            /*if (this.spawns.All(x => x.isOver) && this.nbrEnnemieAlive == 0)
            {
                this.currentRound += 1;
                this.StartNextRound();
                return true;
            }
            */
            return false;
        }
    }
}