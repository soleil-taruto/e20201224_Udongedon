using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;
using Charlotte.GameCommons;
using Charlotte.Games.Scripts;
using Charlotte.GameCommons.Options;

namespace Charlotte.Games
{
	public class TitleMenu : IDisposable
	{
		public static TitleMenu I;

		public TitleMenu()
		{
			I = this;
		}

		public void Dispose()
		{
			I = null;
		}

		private DDSimpleMenu SimpleMenu;

		public void Perform()
		{
			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			Ground.I.Music.MUS_TITLE.Play();

			string[] items = new string[]
			{
				"ゲームスタート",
				"ステージ選択",
				"設定",
				"終了",
			};

			int selectIndex = 0;

			this.SimpleMenu = new DDSimpleMenu();

			this.SimpleMenu.BorderColor = new I3Color(64, 0, 64);
			this.SimpleMenu.WallColor = new I3Color(0, 70, 140);
			//this.SimpleMenu.WallPicture = Ground.I.Picture.P_TITLE_WALL;
			//this.SimpleMenu.WallCurtain = -0.8;
			this.SimpleMenu.WallDrawer = () => this.WallDrawer.Task();

			for (; ; )
			{
				selectIndex = this.SimpleMenu.Perform("東方っぽいシューティングの試作版", items, selectIndex);

				switch (selectIndex)
				{
					case 0:
						this.SelectChara(0);
						break;

					case 1:
						this.SelectStage();
						break;

					case 2:
						this.Setting();
						break;

					case 3:
						goto endMenu;

					default:
						throw new DDError();
				}
			}
		endMenu:
			DDMusicUtils.Fade();
			DDCurtain.SetCurtain(30, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				this.SimpleMenu.DrawWall();
				DDEngine.EachFrame();
			}

			DDEngine.FreezeInput();
		}

		#region WallDrawer

		private WallDrawerTask WallDrawer = new WallDrawerTask();

		private class WallDrawerTask : DDTask
		{
			public const double CURTAIN_W_DEF = DDConsts.Screen_W / 2;

			public double TargetCurtain_W = CURTAIN_W_DEF;
			private double Curtain_W = 0.0;

			private DDTaskList EL = new DDTaskList();

			public override IEnumerable<bool> E_Task()
			{
				for (int frame = 0; ; frame++)
				{
					if (frame % 130 == 0)
						this.EL.Add(new 響子Task().Task);

					this.EL.ExecuteAllTask();

					DDUtils.Approach(ref this.Curtain_W, this.TargetCurtain_W, 0.9);

					DDDraw.SetAlpha(0.5);
					DDDraw.SetBright(0, 0, 0);
					DDDraw.DrawRect(Ground.I.Picture.WhiteBox, 0, 0, this.Curtain_W, DDConsts.Screen_H);
					DDDraw.Reset();

					yield return true;
				}
			}

			public class 響子Task : DDTask
			{
				public override IEnumerable<bool> E_Task()
				{
					DDPicture picture = GetPicture(DDUtils.Random.GetRange(PIC_INDEX_MIN, PIC_INDEX_MAX));
					double x = DDConsts.Screen_W + 300.0;
					double y = DDConsts.Screen_H - 200.0;

					for (; ; )
					{
						x -= 3.0;

						if (x < -300.0)
							break;

						DDDraw.DrawCenter(picture, x, y);

						yield return true;
					}
				}

				private const string PIC_PREFIX = @"dat\dairi\67504816_p";
				private const string PIC_SUFFIX = ".png";
				private const int PIC_INDEX_MIN = 0;
				private const int PIC_INDEX_MAX = 10;

				private static DDPicture GetPicture(int index)
				{
					return DDCCResource.GetPicture(PIC_PREFIX + index + PIC_SUFFIX);
				}

				public static void Touch()
				{
					for (int index = PIC_INDEX_MIN; index <= PIC_INDEX_MAX; index++)
						GetPicture(index).GetHandle();
				}
			}
		}

		public static void TouchWallDrawerResources()
		{
			WallDrawerTask.響子Task.Touch(); // 響子の画像を全部触っておく
		}

		#endregion

		private void SelectStage()
		{
			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = this.SimpleMenu.Perform("ステージ選択", GameMaster.Stages.Select(v => v.Name).Concat(new string[] { "戻る" }).ToArray(), selectIndex);

				if (GameMaster.Stages.Length <= selectIndex)
					break;

				this.SelectChara(selectIndex);
			}
		}

		private void SelectChara(int startStageIndex)
		{
			Action<Player.PlayerWho_e> a_gameStart = plWho =>
			{
				this.LeaveTitleMenu();
				GameMaster.Start(startStageIndex, plWho);
				this.ReturnTitleMenu();
			};

			switch (this.SimpleMenu.Perform(
				"キャラ選択",
				new string[]
				{
					"小悪魔",
					"メディスン・メランコリー",
					"戻る",
				},
				0
				))
			{
				case 0:
					a_gameStart(Player.PlayerWho_e.小悪魔);
					break;

				case 1:
					a_gameStart(Player.PlayerWho_e.メディスン);
					break;

				case 2:
					break;

				default:
					throw null; // never
			}
		}

		private void Setting()
		{
			this.WallDrawer.TargetCurtain_W = 600;

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			int selectIndex = 0;

			for (; ; )
			{
				string[] items = new string[]
				{
					"ゲームパッドのボタン設定",
					"キーボードのキー設定",
					"ウィンドウサイズ変更",
					"ＢＧＭ音量",
					"ＳＥ音量",
					"自弾の動きに合わせて背景が歪む効果 [ 現在の設定：" + (Ground.I.自弾背景歪み ? "有効" : "無効") + " ]",
					"戻る",
				};

				selectIndex = this.SimpleMenu.Perform("設定", items, selectIndex);

				switch (selectIndex)
				{
					case 0:
						this.SimpleMenu.PadConfig();
						break;

					case 1:
						this.SimpleMenu.PadConfig(true);
						break;

					case 2:
						this.SimpleMenu.WindowSizeConfig();
						break;

					case 3:
						this.SimpleMenu.VolumeConfig("ＢＧＭ音量", DDGround.MusicVolume, 0, 100, 1, 10, volume =>
						{
							DDGround.MusicVolume = volume;
							DDMusicUtils.UpdateVolume();
						},
						() => { }
						);
						break;

					case 4:
						this.SimpleMenu.VolumeConfig("ＳＥ音量", DDGround.SEVolume, 0, 100, 1, 10, volume =>
						{
							DDGround.SEVolume = volume;
							DDSEUtils.UpdateVolume();
						},
						() =>
						{
							Ground.I.SE.SE_ITEMGOT.Play();
						}
						);
						break;

					case 5:
						Ground.I.自弾背景歪み ^= true;
						break;

					case 6:
						goto endMenu;

					default:
						throw new DDError();
				}
			}
		endMenu:
			DDEngine.FreezeInput();

			this.WallDrawer.TargetCurtain_W = WallDrawerTask.CURTAIN_W_DEF; // restore
		}

		private void LeaveTitleMenu()
		{
			DDMusicUtils.Fade();
			DDCurtain.SetCurtain(30, -1.0);

			foreach (DDScene scene in DDSceneUtils.Create(40))
			{
				this.SimpleMenu.DrawWall();
				DDEngine.EachFrame();
			}

			GC.Collect();
		}

		private void ReturnTitleMenu()
		{
			Ground.I.Music.MUS_TITLE.Play();

			GC.Collect();
		}
	}
}
