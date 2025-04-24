using System.Transactions;
using Sandbox;

public sealed class Melee : Component
{
	float lastAttack = Time.Now;
	[Property] PlayerController pc { get; set; }
	[Property] SkinnedModelRenderer renderer { get; set; }

	[Property] GameObject body { get; set; }



	protected override void OnEnabled()
	{
		pc = this.GameObject.GetComponents<PlayerController>().FirstOrDefault();
		renderer = pc.Renderer;
	}
	protected override void OnUpdate()
	{
		
	
		if (pc.IsValid())
		{
			Chill();

			if ( CheckAttack() )
			{

				Attack();
				
			}
		}
		
		
	}

	public void Attack()
	{
		renderer.Set( "holdtype", 5 );
		renderer.Set( "b_attack", true );

		var trstart = body.WorldPosition + Vector3.Up * (pc.BodyHeight - 30) + body.WorldRotation.Forward * 5;
		var trend = trstart + body.WorldRotation.Forward * 30;
		Gizmo.Draw.Line( trstart, trend );
		var tr = Scene.Trace.Ray( trstart, trend )
			.WithTag( "enemy" )
			.Run();
		Log.Info( tr.Hit );
		if (tr.Hit)
		{
			Log.Info( "Hit");
			var target = tr.GameObject;
			if ( target.IsValid() )
			{
				Log.Info( "Hit " + target.Name );
				var rb = target.GetComponent<Rigidbody>();
				if ( rb.IsValid() )
				{
					rb.ApplyImpulse( body.WorldRotation.Forward * 1000 * rb.Mass );
				}
			}
		}


	}

	public void Chill()
	{
		if ( Time.Now - lastAttack > 5 )
		{
			renderer.Set( "holdtype", 0 );


		}
	}

	public bool CheckAttack()
	{
		if ( Input.Down( "attack1" ) && Time.Now - lastAttack > 0.5 )
		{
			lastAttack = Time.Now;
			return true;
		}
		else
		{
			return false;
		}
			
	}
}
