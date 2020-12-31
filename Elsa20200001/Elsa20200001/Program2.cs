using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games;
using Charlotte.Tests;
using Charlotte.Tests.Games;

namespace Charlotte
{
	public class Program2
	{
		public void Main2()
		{
			try
			{
				Main3();
			}
			catch (Exception e)
			{
				ProcMain.WriteLog(e);
			}
		}

		private void Main3()
		{
			DDMain2.Perform(Main4);
		}

		private void Main4()
		{
			// *.INIT
			{
				// アプリ固有 >

				RippleEffect.INIT();
				画面分割.INIT();
				画面分割_Effect.INIT();

				// < アプリ固有
			}

			#region Charge To DDTouch

			// アプリ固有 >

			DDTouch.Add(TitleMenu.TouchWallDrawerResources);

			// 個別に設定(例)
			//DDTouch.Add(Ground.I.Picture.P_KOAKUMA_P1);
			//DDTouch.Add(Ground.I.Music.MUS_TITLE);
			//DDTouch.Add(Ground.I.SE.SE_KASURI);

			// 全部設定
			DDTouch.AddAllPicture();
			DDTouch.AddAllMusic();
			DDTouch.AddAllSE();

			// < アプリ固有

			#endregion

			//DDTouch.Touch(); // moved -> Logo

			if (ProcMain.ArgsReader.ArgIs("//D")) // 引数は適当な文字列
			{
				Main4_Debug();
			}
			else
			{
				Main4_Release();
			}
		}

		private void Main4_Debug()
		{
			// ---- choose one ----

			//Main4_Release();
			//new Test0001().Test01();
			//new TitleMenuTest().Test01();
			//new GameTest().Test01();
			//new GameTest().Test02();
			new GameTest().Test03(); // スクリプトを選択

			// ----
		}

		private void Main4_Release()
		{
			using (new Logo())
			{
				Logo.I.Perform();
			}
			using (new TitleMenu())
			{
				TitleMenu.I.Perform();
			}
		}
	}
}
