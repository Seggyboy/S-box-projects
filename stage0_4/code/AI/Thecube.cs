using System.Runtime.CompilerServices;
using Sandbox;
using Sandbox.Utility.Svg;

public sealed class Thecube : Component
{
	[Property] NavMeshAgent agent { get; set; }
	[Property] GameObject body { get; set; }
	[Property] Rigidbody rb { get; set; }

	[Property] PlayerController player { get; set; }

	[Property] ModelRenderer renderer { get; set; }
	[Property] int health = 2;

	

	
	protected override void OnEnabled()
	{
		player = Scene.Directory.FindByName("Player Controller").FirstOrDefault().GetComponent<PlayerController>();
	}

	protected override void OnDisabled()
	{
		agent.Enabled = false;
		WakeUp();
	}

	public async void WakeUp()
	{
		await GameTask.DelaySeconds( 5 );
		this.Enabled = true;
		agent.Enabled = true;
	}

	protected override void OnUpdate()
	{
		Hunt();



		
	}

	public void Hunt()
	{
		
		float timeToReach = agent.Velocity.Length / Vector3.DistanceBetween(agent.WorldPosition, player.WorldPosition );
		Vector3 predPos = player.WorldPosition + player.Velocity * timeToReach;
		//Log.Info( predPos );
		body.WorldPosition = agent.WorldPosition + Vector3.Up * 22;
		body.WorldRotation.SlerpTo( Rotation.LookAt( predPos - agent.WorldPosition ), 0.1f);
		agent.MoveTo( predPos );

	}

	public async void Hurt()
	{
		health--;
		if ( health <= 0 )
		{
			
			renderer.Tint = Color.Red;
			await GameTask.DelaySeconds( 0.5f );
			body.Destroy();

		}
		else if ( health < 2 )
		{
			renderer.Tint = Color.Orange;
		}

			
	}

	
}
