using System;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using UnrealEngine.Framework;

namespace UnrealEngine.Tests {
	public static class RadialForce {
		public static void OnBeginPlay() {
			Debug.Log(LogLevel.Display, "Hello, Unreal Engine!");
			Debug.AddOnScreenMessage(-1, 3.0f, Color.LightGreen, MethodBase.GetCurrentMethod().DeclaringType + " system started!");

			World.GetFirstPlayerController().SetViewTarget(World.GetActor<Camera>("MainCamera"));

			const int maxActors = 1;

			Actor[] actors = new Actor[maxActors];
			StaticMeshComponent[] staticMeshComponents = new StaticMeshComponent[maxActors];

			for (int i = 0; i < maxActors; i++) {
				actors[i] = new Actor();
				staticMeshComponents[i] = new StaticMeshComponent(actors[i], setAsRoot: true);
				staticMeshComponents[i].SetStaticMesh(StaticMesh.Cube);
				staticMeshComponents[i].SetMaterial(0, Material.Load("/Game/Tests/BasicMaterial"));
				staticMeshComponents[i].CreateAndSetMaterialInstanceDynamic(0).SetVectorParameterValue("Color", LinearColor.Red);
				staticMeshComponents[i].SetRelativeLocation(new Vector3(1000.0f, 0.0f, 0.0f));
				staticMeshComponents[i].UpdateToWorld(TeleportType.ResetPhysics);
				staticMeshComponents[i].SetSimulatePhysics(true);
				staticMeshComponents[i].SetCollisionChannel(CollisionChannel.PhysicsBody);
			}

			RadialForceComponent radialForceComponent = new RadialForceComponent(new Actor(), setAsRoot: true);

			radialForceComponent.IgnoreOwningActor = true;
			radialForceComponent.ImpulseVelocityChange = true;
			radialForceComponent.LinearFalloff = true;
			radialForceComponent.ForceStrength = 10000.0f;
			radialForceComponent.ImpulseStrength = 10000.0f;
			radialForceComponent.Radius = 1000;
			radialForceComponent.SetRelativeLocation(new Vector3(1000.0f, 0.0f, -100.0f));
			radialForceComponent.AttachToComponent(staticMeshComponents[0], AttachmentTransformRule.KeepRelativeTransform);
			radialForceComponent.FireImpulse();

			Debug.AddOnScreenMessage(-1, 3.0f, Color.LightGreen, "Actors are spawned! Number of actors in the world: " + World.ActorCount);
		}

		public static void OnEndPlay() {
			Debug.Log(LogLevel.Display, "See you soon, Unreal Engine!");
			Debug.ClearOnScreenMessages();
		}
	}
}