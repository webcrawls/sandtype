using Sandbox.UI;

namespace Sandtype.UI.Game;

public interface ClientWorldEntity
{

	public bool IsActive();

	public void Spawn();

	public void Delete();

	public void Think();

}
