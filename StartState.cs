using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BaseProject
{
	class StartState : GameState
	{
		public StartState()
		{
			gameObjectList.Add(new LevelLoader());
		}
	}
}