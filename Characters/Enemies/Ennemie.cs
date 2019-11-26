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

        private Spatial target;

        private List<Vector3> path;

        private bool isInside = false;


        public void SetPath(List<Vector3> path)
        {
            this.path = path;
        }
        public void SetTarget(Spatial ch)
        {
            this.target = ch;
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
            //AddToGroup("ennemies");
        }

        public override void _PhysicsProcess(float delta)
        {
            if (isDead || this.target == null)
                return;

            var vecToPlayer = this.target.Translation - this.GlobalTransform.origin;
            vecToPlayer.y = 0;
            vecToPlayer = vecToPlayer.Normalized();
            rayCast.CastTo = vecToPlayer * 1.5f;

            MoveAndCollide(vecToPlayer * BaseMouvementSpeed * delta);

            /*if (this.path != null && this.path.Count > 0)
            {
                foreach(var p in this.path) {
                    GD.Print("PATH ",p.ToString());
                }
                var toWalk = delta * 4.0;
                var toWatch = new Vector3(0,1,0);
                while(toWalk > 0 && path.Count >= 2) {
                    var pFrom = path[path.Count - 1];
                    var pto = path[path.Count - 2];
                    toWatch = (pto - pFrom).Normalized();
                    var d = pFrom.DistanceTo(pto);
                    if(d <= toWalk) {
                        path.RemoveAt(path.Count -1);
                        toWalk -= d;
                    } else {
                        path[path.Count-1] = pFrom.LinearInterpolate(pto,(float)((double)toWalk / d));
                        toWalk = 0;
                    }
                }

                var atPos = path[path.Count - 1];
                var atDir = toWatch;
                atDir.y = 0;

                var t = new Transform();

                t.origin = atPos;
                t = t.LookingAt(atPos + atDir,new Vector3(0,1,0));
                this.SetTransform(t);

                if(path.Count < 2) {
                    path = new List<Vector3>();
                }

                //MoveAndCollide(this.path[0].Normalized() * BaseMouvementSpeed * delta);
            }
            */
            

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
