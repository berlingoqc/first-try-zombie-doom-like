using Godot;

namespace Game.Characters.Ennemies
{
    public class Enemie : Character
    {

        [Signal]
        public delegate void EnemieDeath(Enemie enemie);

        private static float BaseMouvementSpeed = 0.6f;

        private RayCast rayCast;

        private MainCharacter character;


        public void SetCharacter(MainCharacter ch) {
            this.character = ch;
        }

        public override void _Ready() {
            GD.Print("Instancing ennemie");
            this.DeathSignal = nameof(EnemieDeath);
            this.animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            this.rayCast = GetNode<RayCast>("RayCast");
            this.animationPlayer.Play("walking");
            this.character =  GetTree().GetRoot().GetNode<MainCharacter>("Spatial/MainCharacter");
            AddToGroup("ennemies");
        }

        public override void _PhysicsProcess(float delta) {
            if(isDead || this.character == null)
                return;

            var vecToPlayer = this.character.Translation - this.GlobalTransform.origin;
            vecToPlayer.y = 0;
            vecToPlayer = vecToPlayer.Normalized();
            rayCast.CastTo = vecToPlayer * 1.5f;

            MoveAndCollide(vecToPlayer * BaseMouvementSpeed * delta);

            if(rayCast.IsColliding()) {
                var coll = rayCast.GetCollider();
                if(coll != null && coll is MainCharacter) {
                    GD.Print("CHARACTER");
                }
            }

        }
    }
}
