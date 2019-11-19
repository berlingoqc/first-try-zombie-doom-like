using Godot;
using System;

namespace Game.Characters
{
    public class Character : KinematicBody
    {
        [Export]
        public float Gravity = -24.8f;
        [Export]
        public float MaxSpeed = 20.0f;
        [Export]
        public float JumpSpeed = 18.0f;
        [Export]
        public float Accel = 4.5f;
        [Export]
        public float Deaccel = 16.0f;
        [Export]
        public float MaxSlopeAngle = 40.0f;
        [Export]
        public float SprintAccel = 2;


        protected string HitAnimation = "hit";
        protected string WalkAnimation = "walking";
        protected string DeathAnimation = "dying";


        protected string DeathSignal = "";


        protected float Health = 2.0f;
        protected float Armor;


        protected AnimationPlayer animationPlayer;
        protected CollisionShape collisionShape;

        public bool isDead
        {
            get
            {
                return Health <= 0;
            }
        }

        public virtual void Kill()
        {
            this.Health = 0;
            this.animationPlayer.Stop();
            this.animationPlayer.Play(DeathAnimation);
            this.animationPlayer.Connect("animation_finished", this, "_on_Death_Animation_Over");
            EmitSignal(this.DeathSignal, this);
        }

        public void Hit(float dmg)
        {
            if (isDead)
                return;
            if (dmg >= Health) { 
                this.Kill();
                return;
            }
            this.Health -= dmg;
			this.animationPlayer.Stop();
			this.animationPlayer.Connect("animation_finished",this,"OnHitOver");
            this.animationPlayer.Play(HitAnimation);
        }

        public void _on_Death_Animation_Over(string name) {
            this.QueueFree();
        }

        public void OnHitOver(string name) {
            this.animationPlayer.Disconnect("animation_finished",this,"OnHitOver");
            this.StartWalking();
        }

        public void StartWalking() {
            this.animationPlayer.Play("walking");
        }
    }
}
