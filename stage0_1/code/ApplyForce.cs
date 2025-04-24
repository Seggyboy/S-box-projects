using Sandbox;
using System;

public sealed class ApplyForce : Component
{
	[Property] CameraComponent cam;
	[Property] Rigidbody rb;
	float camHeight = 100f;
	protected override void OnEnabled()
	{
		cam = Scene.GetAllComponents<CameraComponent>().FirstOrDefault();
		rb = this.GameObject.GetComponent<Rigidbody>();
	}
	protected override void OnUpdate()
	{
		this.GameObject.WorldPosition += new Vector3( 0, 0, 0 );
		Vector3 cubePos = this.GameObject.WorldPosition;
		cam.WorldPosition = cubePos - this.GameObject.WorldRotation.Forward * 300f + Vector3.Up * camHeight;
		var targetPosition = ( this.GameObject.WorldPosition + (this.GameObject.WorldRotation.Forward * 300f) - cam.WorldPosition ).Normal;
		cam.WorldRotation = Rotation.LookAt( targetPosition );


		this.GameObject.WorldRotation *= Rotation.FromYaw(-Mouse.Delta.x * 0.05f);
		camHeight = Math.Clamp(camHeight + (Mouse.Delta.y * 0.1f), 50,150);

		var trstart = this.GameObject.WorldPosition + Vector3.Up * 10;
		var trend = trstart + this.GameObject.WorldRotation.Forward * 20f;
		SceneTraceResult tr = Scene.Trace.Ray(  trstart, trend )
			.WithCollisionRules( "solid" )
			.Run();

		Gizmo.Draw.Line( trstart, trend );
		
			
			if ( Input.Down( "Forward" ) && !tr.Hit )
			{
				this.GameObject.WorldPosition += this.GameObject.WorldRotation.Forward * 1;
			}

			else if ( Input.Down( "Backward" ) )
			{
				this.GameObject.WorldPosition -= this.GameObject.WorldRotation.Forward * 1;
			}
			
			if (Input.Down("Jump") )
			{
				rb.ApplyImpulse(new Vector3(0,0, 200 * rb.Mass));
			}
		

		
	}
}
