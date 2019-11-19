using Godot;

namespace Game.Characters
{
    public class MainCharacter : Character
    {
        public float MouseSensitivity = 0.05f;
 
        private static float BaseMouvementSpeed = 4;
        private static float BaseMouseSpeed = 0.5f;

        private Vector3 velocity = new Vector3();
        private Vector3 direction = new Vector3();
        private bool isRunning = false;

        private Spatial rotationHelper;
        private Camera camera;
        private RayCast rayCast;
        private SpotLight spotLight;


        public override void _Ready()
        {
            Input.SetMouseMode(Input.MouseMode.Captured);
            this.animationPlayer = GetNode<AnimationPlayer>("Player");
            this.rayCast = GetNode<RayCast>("RotationHelper/RayCast");
            this.camera = GetNode<Camera>("RotationHelper/Camera");
            this.rotationHelper = GetNode<Spatial>("RotationHelper");
            this.spotLight = GetNode<SpotLight>("RotationHelper/SpotLight");
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseMotion && Input.GetMouseMode() == Input.MouseMode.Captured)
            {
                InputEventMouseMotion mouseEvent = @event as InputEventMouseMotion;
                rotationHelper.RotateX(Mathf.Deg2Rad(-1.0f*mouseEvent.Relative.y * MouseSensitivity));
                RotateY(Mathf.Deg2Rad(-mouseEvent.Relative.x * MouseSensitivity));

                Vector3 cameraRot = rotationHelper.RotationDegrees;
                cameraRot.x = Mathf.Clamp(cameraRot.x, -70, 70);
                rotationHelper.RotationDegrees = cameraRot;
            }
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionPressed("exit"))
            {
                GetTree().Quit();
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            ProcessInput(delta);
            ProcessMovement(delta);
        }

        private void ProcessMovement(float delta)
        {
            direction.y = 0;
            direction = direction.Normalized();

            velocity.y += delta * Gravity;

            var hvel = velocity;
            hvel.y = 0.0f;

            var target = direction;
            target *= BaseMouvementSpeed;

            float accel;
            if(direction.Dot(hvel) > 0)
                accel = Accel;
            else
                accel = Deaccel;

            hvel = hvel.LinearInterpolate(target,Accel*delta);
            velocity.x = hvel.x;
            velocity.z = hvel.z;
            velocity = MoveAndSlide(velocity,new Vector3(0,1,0),false,4,Mathf.Deg2Rad(MaxSlopeAngle));

            if (Input.IsActionPressed("shoot") && !this.animationPlayer.IsPlaying())
            {
                this.animationPlayer.Play("shooting");
                var collider = this.rayCast.GetCollider();
                if (collider != null)
                {
                    GD.Print(collider.GetType().FullName);
                    if (this.rayCast.IsColliding() && collider is Ennemies.Enemie)
                    {
                        var ennemies = (Ennemies.Enemie)collider;
                        ennemies.Hit(1.0f);
                    }
                }
            }
        }


        private void ProcessInput(float delta)
        {
            direction = new Vector3();
            var vec2 = new Vector2();
            Transform camXForm = this.camera.GetGlobalTransform();

            if (Input.IsActionPressed("forward"))
            {
                vec2.y += 1;

            }
            if (Input.IsActionPressed("backward"))
            {
                vec2.y -= 1;
            }
            if (Input.IsActionPressed("leftward"))
            {
                vec2.x -= 1;

            }
            if (Input.IsActionPressed("rightward"))
            {
                vec2.x += 1;
            }

            vec2 = vec2.Normalized();

            direction += -camXForm.basis.z * vec2.y;
            direction += camXForm.basis.x * vec2.x;

            if (IsOnFloor() && Input.IsActionJustPressed("jumping"))
                velocity.y = JumpSpeed;
            
            isRunning = Input.IsActionJustPressed("sprinting");

            if(Input.IsActionJustPressed("toggle_flashlight")) {
                if(this.spotLight.IsVisibleInTree())
                    this.spotLight.Hide();
                else
                    this.spotLight.Show();
            }

            if (Input.IsActionJustPressed("ui_cancel"))
            {
                if (Input.GetMouseMode() == Input.MouseMode.Visible)
                    Input.SetMouseMode(Input.MouseMode.Captured);
                else
                    Input.SetMouseMode(Input.MouseMode.Visible);
            }

        }
    }
}