using UnityEngine;
using UnityEngine.Events;

namespace Snake3DWorld.Game.Food
{
	public abstract class AFood : MonoBehaviour
	{
		public event UnityAction<AFood> OnAte;

		public bool WasAte { get; private set; }

		public void Reset()
		{
			WasAte = false;
		}

		public virtual bool Eat()
		{
			if (WasAte) return false;
		
			WasAte = true;
			OnAte?.Invoke(this);
			return true;
		}
	}
}