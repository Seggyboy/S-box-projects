using Sandbox;

public sealed class Meancubes : Component
{
	[Property] NavMeshAgent Agent { get; set; }
	[Property] GameObject Target { get;set; }

	

	protected override void OnEnabled()
	{
		Vector3 randomPos = (Vector3) Scene.NavMesh.GetRandomPoint();
		Agent.MoveTo( randomPos );
	}

}
