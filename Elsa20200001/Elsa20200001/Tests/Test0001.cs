using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;

namespace Charlotte.Tests
{
	public class Test0001
	{
		public void Test01()
		{
			for (; ; )
			{
				if (DDInput.PAUSE.GetInput() == 1)
				{
					Ground.I.Music.MUS_TITLE.Play();
				}
				if (DDInput.A.GetInput() == 1)
				{
					Ground.I.Music.MUS_STAGE_01.Play();
				}
				if (DDInput.B.GetInput() == 1)
				{
					Ground.I.Music.MUS_BOSS_01.Play();
				}
				if (DDInput.C.GetInput() == 1)
				{
					Ground.I.Music.MUS_STAGE_02.Play();
				}
				if (DDInput.D.GetInput() == 1)
				{
					Ground.I.Music.MUS_BOSS_02.Play();
				}

				DDCurtain.DrawCurtain();

				DDPrint.SetPrint();
				DDPrint.Print("音量テスト");

				DDEngine.EachFrame();
			}
		}
	}
}
