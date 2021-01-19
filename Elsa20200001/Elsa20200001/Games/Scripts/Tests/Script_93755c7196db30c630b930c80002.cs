using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Games.Walls;
using Charlotte.Games.Enemies.鍵山雛s;

namespace Charlotte.Games.Scripts.Tests
{
	public class Script_鍵山雛テスト0002 : Script
	{
		protected override IEnumerable<bool> E_EachFrame()
		{
			Ground.I.Music.MUS_BOSS_01.Play();

			Game.I.Walls.Add(new Wall_Dark());
			Game.I.Walls.Add(new Wall_11001());
			Game.I.Walls.Add(new Wall_11002());

			Game.I.Enemies.Add(new Enemy_鍵山雛_02(GameConsts.FIELD_W / 2, GameConsts.FIELD_H / 7));

			for (; ; )
			{
				yield return true;
			}
		}
	}
}
