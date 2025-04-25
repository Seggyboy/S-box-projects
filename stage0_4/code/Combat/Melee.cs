using System;
using Sandbox;

public sealed class Melee : Component
{
	float lastAttack = Time.Now;
	float lastStamInc = Time.Now;
	float disabledTime = Time.Now;
	[Property] PlayerController pc { get; set; }
	[Property] SkinnedModelRenderer renderer { get; set; }

	[Property] GameObject body { get; set; }

	[Property] float stamina { get; set; } = 100;

	enum StaminaState
	{
		Normal,
		Tired,
		Exhausted
	}

	StaminaState stamState =  new StaminaState();



	protected override void OnEnabled()
	{
		pc = this.GameObject.GetComponents<PlayerController>().FirstOrDefault();
		renderer = pc.Renderer;
	}
	protected override void OnUpdate()
	{

		SetTiredState( stamina );
		//Log.Info(stamState.ToString() + " " + stamina.ToString() );
		if ( pc.IsValid() )
		{
			Chill();

			if ( CheckAttack() )
			{

				Attack();

			}
		}
		Log.Info( stamina );

	}

	public void Attack()
	{
		UpdateStamina( -10 );
		renderer.Set( "holdtype", 5 );
		renderer.Set( "b_attack", true );

		var trstart = body.WorldPosition + Vector3.Up * (pc.BodyHeight - 25) + body.WorldRotation.Forward * 5;
		var trend = trstart + body.WorldRotation.Forward * 40;
		Gizmo.Draw.Line( trstart, trend );
		var tr = Scene.Trace.Ray( trstart, trend )
			.WithTag( "enemy" )
			.Run();
		Log.Info( tr.Hit );
		if ( tr.Hit )
		{
			Log.Info( "Hit" );
			var target = tr.GameObject;
			if ( target.IsValid() )
			{
				//Log.Info( "Hit " + target.Name );
				var rb = target.GetComponent<Rigidbody>();
				if ( rb.IsValid() )
				{
					var agent = (Thecube) target.Children.FirstOrDefault().GetComponent<Thecube>();
					if ( agent.IsValid() )
					{
						agent.Enabled = false;
						agent.Hurt();
					}



					rb.ApplyImpulse( body.WorldRotation.Forward * 5000 * rb.Mass );
				}
			}
		}


	}

	public void Chill()
	{
		if ( Time.Now - lastAttack > 2 && Time.Now-lastStamInc>1 )
		{
			lastStamInc = Time.Now;
			UpdateStamina( 5 );


		}

		if ( Time.Now - lastAttack > 5 )
		{
			renderer.Set( "holdtype", 0 );


		}
	}

	public bool CheckAttack()
	{
		if ( Input.Down( "attack1" ) && Time.Now - lastAttack > 0.5 && stamina>0 )
		{
			lastAttack = Time.Now;
			return true;
		}
		else
		{
			return false;
		}

	}

	public void UpdateStamina(float dStam)
	{
		stamina = Math.Clamp( stamina + dStam, 0, 100 );
	}


	public void SetTiredState( float stamina )
	{
		switch ( stamina )
		{
			case > 70:
				stamState = StaminaState.Normal;
				break;
			case > 40:
				stamState = StaminaState.Tired;
				break;
			case > 0:
				stamState = StaminaState.Exhausted;
				break;
		}


	}

	


}
