using Godot;
using System.Collections.Generic;

using Game.Characters;
using Game.Objects.Navmesh;


namespace Game.Objects.Spawn
{

    public class SpawnManager : Spatial
    {

        [Signal]
        public delegate void RoundOver();


        public List<ZombieSpawn> AvailableSpawn
        {
            get; set;
        }

        public SpawnSettings Settings
        {
            get; set;
        }

        public MainCharacter MainCharacter
        {
            get; set;
        }

        public BaseLevelNavMesh navMesh
        {
            get; set;
        }

        public int Round
        {
            get; set;
        }

        private SpawnSettings currentSettings;

        private Timer timer;


        public override void _Ready()
        {
            this.Round = 0;
            timer = new Timer();
            timer.Connect("timeout", this, "_on_Timer_Timeout");
            AddChild(timer);
        }

        public void StartRound()
        {
            currentSettings = new SpawnSettings();
            currentSettings.NbrToSpawn = Settings.NbrToSpawn * (this.Round + 1);
            currentSettings.Timeout = Settings.Timeout - ((this.Round + 1) / 10.0f);
            currentSettings.Timeout = (currentSettings.Timeout < 1) ? 1 : currentSettings.Timeout;

            this.MainCharacter.labelMessage.AddMessage($"ROUND {Round + 1} STARTING");

            GD.Print("ROUND ", Round, " NBR SPAWN ", currentSettings.NbrToSpawn, " TIMEOUT ", currentSettings.Timeout);

            timer.Start();
        }

        private void OnRoundOver()
        {
            timer.Stop();
            EmitSignal(nameof(RoundOver));
        }


        private void _on_Timer_Timeout()
        {
            var items = this.navMesh.OrderSpawnByDistance(this.AvailableSpawn);
            var (distance,spawn) = items[0];
            GD.Print("Spawing spawn at ",distance," ",spawn.GetTranslation().ToString(), " CHARACTER ",MainCharacter.GetTranslation().ToString());
            spawn.Spawn(this.MainCharacter,this.navMesh);
        }

        private void _on_Ennemie_Death()
        {

        }

    }

}