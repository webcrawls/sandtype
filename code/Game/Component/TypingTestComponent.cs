using System;
using Sandbox;

namespace Sandtype.Game.Component;

public class TypingTestComponent : EntityComponent<Pawn>
{

	public int CursorPos;
	public String TargetText;
	public String InputText;

	public float Progress => (float)(InputText?.Length ?? 1) / (TargetText?.Length ?? 1);

	public void Reset()
	{
		InputText = "";
		TargetText = "Hello!";
	}

	protected override void OnActivate()
	{
		base.OnActivate();
		Reset();
	}
}
