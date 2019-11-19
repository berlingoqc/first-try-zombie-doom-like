
using System.Collections.Generic;
using Game.Objects.Spawn;

using Newtonsoft.Json;

namespace Game.Level.Demo
{
    public class DemoScene : BaseLevel
    {
      public DemoScene() {
        roundsSettings = new List<SpawnSettings>();
        var r = new SpawnSettings();
        r.NbrToSpawn = 5;
        r.Timeout = 3.0f;
        roundsSettings.Add(r);
      }
    }

}
