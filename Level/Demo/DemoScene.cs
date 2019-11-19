
using System.Collections.Generic;
using Game.Objects.Spawn;

using Newtonsoft.Json;

namespace Game.Level.Demo
{
    public class DemoScene : BaseLevel
    {
      public DemoScene() {
        roundsSettings = new SpawnSettings();
        roundsSettings.NbrToSpawn = 2;
        roundsSettings.Timeout = 3.0f;
      }
    }

}
