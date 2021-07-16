using UnityEngine;

namespace GameLogic 
{
	[CreateAssetMenu(fileName = "SceneList", menuName = "My Game/Scenes List", order = 1)]
	public class SceneList: ScriptableObject
	{
		public SceneReference[] scenes;
	}
}