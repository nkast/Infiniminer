namespace Infiniminer;

public abstract class VirtualInput
{
    public void Register() => InputManager.VirtualInputs.Add(this);
    public void Unregister() => InputManager.VirtualInputs.Remove(this);
    public virtual void Update() { }
}