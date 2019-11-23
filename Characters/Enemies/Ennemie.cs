using Godot;
using System.Collections.Generic;

namespace Game.Characters.Ennemies
{
    public class Enemie : Character
    {

        [Signal]
        public delegate void EnemieDeath(Enemie enemie);

        private static float BaseMouvementSpeed = 0.6f;

        private RayCast rayCast;

        private MainCharacter character;

        private List<Vector3> path;

        private bool isInside = false;


        public void SetPath(List<Vector3> path)
        {
            this.path = path;
        }
        public void SetCharacter(MainCharacter ch)
        {
            this.character = ch;
        }

        public override void Kill()
        {
            this.collisionShape.Disabled = true;
            base.Kill();
        }

        public override void _Ready()
        {
            this.DeathSignal = nameof(EnemieDeath);
            this.collisionShape = GetNode<CollisionShape>("CollisionShape");
            this.animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            this.rayCast = GetNode<RayCast>("RayCast");
            this.animationPlayer.Play("walking");
            this.character = GetTree().GetRoot().GetNode<MainCharacter>("Spatial/MainCharacter");
            //AddToGroup("ennemies");
        }

        public override void _PhysicsProcess(float delta)
        {
            if (isDead || this.character == null)
                return;


            //var vecToPlayer = this.character.Translation - this.GlobalTransform.origin;
            //vecToPlayer.y = 0;
            //vecToPlayer = vecToPlayer.Normalized();
            //rayCast.CastTo = vecToPlayer * 1.5f;
            if (this.path != null && this.path.Count > 0)
            {
                var toWalk = delta * 4.0;
                var toWatch = new Vector3(0,1,0);
                while(toWalk > 0 && path.Count >= 2) {
                    var pFrom = path[path.Count - 1];
                }

                MoveAndCollide(this.path[0].Normalized() * BaseMouvementSpeed * delta);
            }


            if (rayCast.IsColliding())
            {
                var coll = rayCast.GetCollider();
                if (coll != null && coll is MainCharacter)
                {
                    (coll as MainCharacter).Hit(25.0f);
                }
            }
        }
    }
}
