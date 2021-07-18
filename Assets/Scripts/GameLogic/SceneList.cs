using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameLogic 
{
	[CreateAssetMenu(fileName = "SceneList", menuName = "My Game/Scenes List", order = 1)]
	public class SceneList: ScriptableObject
	{
		public SceneReference[] scenes;

#if UNITY_EDITOR
		[ContextMenu("Bake scenes")]
		void BakeScenes()
		{
			var scenesNames = scenes.Select(s => s.ScenePath).ToArray();

			Lightmapping.BakeMultipleScenes(scenesNames);
		}
#endif
	}


}