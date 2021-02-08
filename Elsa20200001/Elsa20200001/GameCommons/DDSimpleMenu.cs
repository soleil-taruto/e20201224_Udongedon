using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxLibDLL;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public class DDSimpleMenu
	{
		public I3Color? Color = null;
		public I3Color? BorderColor = null;
		public I3Color? WallColor = null;
		public DDPicture WallPicture = null;
		public Action WallDrawer = null;
		public double WallCurtain = 0.0; // -1.0 ～ 1.0
		public int X = 16;
		public int Y = 16;
		public int YStep = 32;

		// <---- prm

		private bool MouseUsable;

		public DDSimpleMenu()
			: this(DDUtils.GetMouseDispMode())
		{ }

		public DDSimpleMenu(bool mouseUsable)
		{
			this.MouseUsable = mouseUsable;
		}

		private void DrawWallPicture()
		{
			DDDraw.DrawRect(
				this.WallPicture,
				DDUtils.AdjustRectExterior(this.WallPicture.GetSize().ToD2Size(), new D4Rect(0, 0, DDConsts.Screen_W, DDConsts.Screen_H))
				);
		}

		public void DrawWall()
		{
			DDCurtain.DrawCurtain();

			if (this.WallColor != null)
				DX.DrawBox(0, 0, DDConsts.Screen_W, DDConsts.Screen_H, DDUtils.GetColor(this.WallColor.Value), 1);

			if (this.WallPicture != null)
			{
				DrawWallPicture();
				DDCurtain.DrawCurtain(this.WallCurtain);
			}
			if (this.WallDrawer != null)
				this.WallDrawer();
		}

		public int Perform(string title, string[] items, int selectIndex, bool ポーズボタンでメニュー終了 = false, bool noPound = false)
		{
			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			for (; ; )
			{
				// ★★★ キー押下は 1 マウス押下は -1 で判定する。

				if (this.MouseUsable)
				{
					int musSelIdxY = DDMouse.Y - (this.Y + this.YStep);

					if (0 <= musSelIdxY)
					{
						int musSelIdx = musSelIdxY / this.YStep;

						if (musSelIdx < items.Length)
						{
							selectIndex = musSelIdx;
						}
					}
					if (DDMouse.L.GetInput() == -1)
					{
						break;
					}
					if (DDMouse.R.GetInput() == -1)
					{
						selectIndex = items.Length - 1;
						break;
					}
				}

				if (ポーズボタンでメニュー終了 && DDInput.PAUSE.GetInput() == 1)
				{
					selectIndex = items.Length - 1;
					break;
				}

				bool chgsel = false;

				if (DDInput.A.GetInput() == 1)
				{
					break;
				}
				if (DDInput.B.GetInput() == 1)
				{
					if (selectIndex == items.Length - 1)
						break;

					selectIndex = items.Length - 1;
					chgsel = true;
				}
				if (noPound ? DDInput.DIR_8.GetInput() == 1 : DDInput.DIR_8.IsPound())
				{
					selectIndex--;
					chgsel = true;
				}
				if (noPound ? DDInput.DIR_2.GetInput() == 1 : DDInput.DIR_2.IsPound())
				{
					selectIndex++;
					chgsel = true;
				}

				selectIndex += items.Length;
				selectIndex %= items.Length;

				if (this.MouseUsable && chgsel)
				{
					DDMouse.X = 0;
					DDMouse.Y = this.Y + (selectIndex + 1) * this.YStep + this.YStep / 2;

					DDMouse.PosChanged();
				}

				this.DrawWall();

				if (this.Color != null)
					DDPrint.SetColor(this.Color.Value);

				if (this.BorderColor != null)
					DDPrint.SetBorder(this.BorderColor.Value);

				DDPrint.SetPrint(DDConsts.Screen_W - 45, 2);
				DDPrint.Print("[M:" + (this.MouseUsable ? "E" : "D") + "]");

				DDPrint.SetPrint(this.X, this.Y, this.YStep);
				//DDPrint.SetPrint(16, 16, 32); // old
				DDPrint.PrintLine(title);

				for (int c = 0; c < items.Length; c++)
				{
					DDPrint.PrintLine(string.Format("[{0}] {1}", selectIndex == c ? ">" : " ", items[c]));
				}
				DDPrint.Reset();

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();

			return selectIndex;
		}

		private class ButtonInfo
		{
			public DDInput.Button Button;
			public string Name;

			public ButtonInfo(DDInput.Button button, string name)
			{
				this.Button = button;
				this.Name = name;
			}
		}

		#region KeyInfos

		private class KeyInfo
		{
			public int KeyId;
			public string Name;

			public KeyInfo(int keyId, string name)
			{
				this.KeyId = keyId;
				this.Name = name;
			}
		}

		private static KeyInfo[] KeyInfos = new KeyInfo[]
		{
			new KeyInfo(DX.KEY_INPUT_0, "0"),
			new KeyInfo(DX.KEY_INPUT_1, "1"),
			new KeyInfo(DX.KEY_INPUT_2, "2"),
			new KeyInfo(DX.KEY_INPUT_3, "3"),
			new KeyInfo(DX.KEY_INPUT_4, "4"),
			new KeyInfo(DX.KEY_INPUT_5, "5"),
			new KeyInfo(DX.KEY_INPUT_6, "6"),
			new KeyInfo(DX.KEY_INPUT_7, "7"),
			new KeyInfo(DX.KEY_INPUT_8, "8"),
			new KeyInfo(DX.KEY_INPUT_9, "9"),
			new KeyInfo(DX.KEY_INPUT_A, "A"),
			new KeyInfo(DX.KEY_INPUT_ADD, "ADD"),
			new KeyInfo(DX.KEY_INPUT_APPS, "APPS"),
			new KeyInfo(DX.KEY_INPUT_AT, "AT"),
			new KeyInfo(DX.KEY_INPUT_B, "B"),
			new KeyInfo(DX.KEY_INPUT_BACK, "BACK"),
			new KeyInfo(DX.KEY_INPUT_BACKSLASH, "BACKSLASH"),
			new KeyInfo(DX.KEY_INPUT_C, "C"),
			new KeyInfo(DX.KEY_INPUT_CAPSLOCK, "CAPSLOCK"),
			new KeyInfo(DX.KEY_INPUT_COLON, "COLON"),
			new KeyInfo(DX.KEY_INPUT_COMMA, "COMMA"),
			new KeyInfo(DX.KEY_INPUT_CONVERT, "CONVERT"),
			new KeyInfo(DX.KEY_INPUT_D, "D"),
			new KeyInfo(DX.KEY_INPUT_DECIMAL, "DECIMAL"),
			new KeyInfo(DX.KEY_INPUT_DELETE, "DELETE"),
			new KeyInfo(DX.KEY_INPUT_DIVIDE, "DIVIDE"),
			new KeyInfo(DX.KEY_INPUT_DOWN, "DOWN"),
			new KeyInfo(DX.KEY_INPUT_E, "E"),
			new KeyInfo(DX.KEY_INPUT_END, "END"),
			new KeyInfo(DX.KEY_INPUT_ESCAPE, "ESCAPE"),
			new KeyInfo(DX.KEY_INPUT_F, "F"),
			new KeyInfo(DX.KEY_INPUT_F1, "F1"),
			new KeyInfo(DX.KEY_INPUT_F10, "F10"),
			new KeyInfo(DX.KEY_INPUT_F11, "F11"),
			new KeyInfo(DX.KEY_INPUT_F12, "F12"),
			new KeyInfo(DX.KEY_INPUT_F2, "F2"),
			new KeyInfo(DX.KEY_INPUT_F3, "F3"),
			new KeyInfo(DX.KEY_INPUT_F4, "F4"),
			new KeyInfo(DX.KEY_INPUT_F5, "F5"),
			new KeyInfo(DX.KEY_INPUT_F6, "F6"),
			new KeyInfo(DX.KEY_INPUT_F7, "F7"),
			new KeyInfo(DX.KEY_INPUT_F8, "F8"),
			new KeyInfo(DX.KEY_INPUT_F9, "F9"),
			new KeyInfo(DX.KEY_INPUT_G, "G"),
			new KeyInfo(DX.KEY_INPUT_H, "H"),
			new KeyInfo(DX.KEY_INPUT_HOME, "HOME"),
			new KeyInfo(DX.KEY_INPUT_I, "I"),
			new KeyInfo(DX.KEY_INPUT_INSERT, "INSERT"),
			new KeyInfo(DX.KEY_INPUT_J, "J"),
			new KeyInfo(DX.KEY_INPUT_K, "K"),
			new KeyInfo(DX.KEY_INPUT_KANA, "KANA"),
			new KeyInfo(DX.KEY_INPUT_KANJI, "KANJI"),
			new KeyInfo(DX.KEY_INPUT_L, "L"),
			new KeyInfo(DX.KEY_INPUT_LALT, "LALT"),
			new KeyInfo(DX.KEY_INPUT_LBRACKET, "LBRACKET"),
			new KeyInfo(DX.KEY_INPUT_LCONTROL, "LCONTROL"),
			new KeyInfo(DX.KEY_INPUT_LEFT, "LEFT"),
			new KeyInfo(DX.KEY_INPUT_LSHIFT, "LSHIFT"),
			new KeyInfo(DX.KEY_INPUT_LWIN, "LWIN"),
			new KeyInfo(DX.KEY_INPUT_M, "M"),
			new KeyInfo(DX.KEY_INPUT_MINUS, "MINUS"),
			new KeyInfo(DX.KEY_INPUT_MULTIPLY, "MULTIPLY"),
			new KeyInfo(DX.KEY_INPUT_N, "N"),
			new KeyInfo(DX.KEY_INPUT_NOCONVERT, "NOCONVERT"),
			new KeyInfo(DX.KEY_INPUT_NUMLOCK, "NUMLOCK"),
			new KeyInfo(DX.KEY_INPUT_NUMPAD0, "NUMPAD0"),
			new KeyInfo(DX.KEY_INPUT_NUMPAD1, "NUMPAD1"),
			new KeyInfo(DX.KEY_INPUT_NUMPAD2, "NUMPAD2"),
			new KeyInfo(DX.KEY_INPUT_NUMPAD3, "NUMPAD3"),
			new KeyInfo(DX.KEY_INPUT_NUMPAD4, "NUMPAD4"),
			new KeyInfo(DX.KEY_INPUT_NUMPAD5, "NUMPAD5"),
			new KeyInfo(DX.KEY_INPUT_NUMPAD6, "NUMPAD6"),
			new KeyInfo(DX.KEY_INPUT_NUMPAD7, "NUMPAD7"),
			new KeyInfo(DX.KEY_INPUT_NUMPAD8, "NUMPAD8"),
			new KeyInfo(DX.KEY_INPUT_NUMPAD9, "NUMPAD9"),
			new KeyInfo(DX.KEY_INPUT_NUMPADENTER, "NUMPADENTER"),
			new KeyInfo(DX.KEY_INPUT_O, "O"),
			new KeyInfo(DX.KEY_INPUT_P, "P"),
			new KeyInfo(DX.KEY_INPUT_PAUSE, "PAUSE"),
			new KeyInfo(DX.KEY_INPUT_PERIOD, "PERIOD"),
			new KeyInfo(DX.KEY_INPUT_PGDN, "PGDN"),
			new KeyInfo(DX.KEY_INPUT_PGUP, "PGUP"),
			new KeyInfo(DX.KEY_INPUT_PREVTRACK, "PREVTRACK"),
			new KeyInfo(DX.KEY_INPUT_Q, "Q"),
			new KeyInfo(DX.KEY_INPUT_R, "R"),
			new KeyInfo(DX.KEY_INPUT_RALT, "RALT"),
			new KeyInfo(DX.KEY_INPUT_RBRACKET, "RBRACKET"),
			new KeyInfo(DX.KEY_INPUT_RCONTROL, "RCONTROL"),
			new KeyInfo(DX.KEY_INPUT_RETURN, "RETURN"),
			new KeyInfo(DX.KEY_INPUT_RIGHT, "RIGHT"),
			new KeyInfo(DX.KEY_INPUT_RSHIFT, "RSHIFT"),
			new KeyInfo(DX.KEY_INPUT_RWIN, "RWIN"),
			new KeyInfo(DX.KEY_INPUT_S, "S"),
			new KeyInfo(DX.KEY_INPUT_SCROLL, "SCROLL"),
			new KeyInfo(DX.KEY_INPUT_SEMICOLON, "SEMICOLON"),
			new KeyInfo(DX.KEY_INPUT_SLASH, "SLASH"),
			new KeyInfo(DX.KEY_INPUT_SPACE, "SPACE"),
			new KeyInfo(DX.KEY_INPUT_SUBTRACT, "SUBTRACT"),
			new KeyInfo(DX.KEY_INPUT_SYSRQ, "SYSRQ"),
			new KeyInfo(DX.KEY_INPUT_T, "T"),
			new KeyInfo(DX.KEY_INPUT_TAB, "TAB"),
			new KeyInfo(DX.KEY_INPUT_U, "U"),
			new KeyInfo(DX.KEY_INPUT_UP, "UP"),
			new KeyInfo(DX.KEY_INPUT_V, "V"),
			new KeyInfo(DX.KEY_INPUT_W, "W"),
			new KeyInfo(DX.KEY_INPUT_X, "X"),
			new KeyInfo(DX.KEY_INPUT_Y, "Y"),
			new KeyInfo(DX.KEY_INPUT_YEN, "YEN"),
			new KeyInfo(DX.KEY_INPUT_Z, "Z"),
		};

		#endregion

		public void PadConfig(bool keyMode = false)
		{
			ButtonInfo[] btnInfos = new ButtonInfo[]
			{
#if false // 例
				new ButtonInfo(DDInput.DIR_2, "下"),
				new ButtonInfo(DDInput.DIR_4, "左"),
				new ButtonInfo(DDInput.DIR_6, "右"),
				new ButtonInfo(DDInput.DIR_8, "上"),
				new ButtonInfo(DDInput.A, "Ａボタン"),
				new ButtonInfo(DDInput.B, "Ｂボタン"),
				new ButtonInfo(DDInput.C, "Ｃボタン"),
				//new ButtonInfo(DDInput.D, ""), // 使用しないボタン
				//new ButtonInfo(DDInput.E, ""), // 使用しないボタン
				//new ButtonInfo(DDInput.F, ""), // 使用しないボタン
				new ButtonInfo(DDInput.L, "Ｌボタン"),
				new ButtonInfo(DDInput.R, "Ｒボタン"),
				//new ButtonInfo(DDInput.PAUSE, ""), // 使用しないボタン
				//new ButtonInfo(DDInput.START, ""), // 使用しないボタン
#else
				// アプリ固有の設定 >

				new ButtonInfo(DDInput.DIR_2, "下"),
				new ButtonInfo(DDInput.DIR_4, "左"),
				new ButtonInfo(DDInput.DIR_6, "右"),
				new ButtonInfo(DDInput.DIR_8, "上"),
				new ButtonInfo(DDInput.A, "低速／決定"),
				new ButtonInfo(DDInput.B, "ショット／キャンセル"),
				new ButtonInfo(DDInput.C, "ボム"),
				//new ButtonInfo(DDInput.D, ""),
				//new ButtonInfo(DDInput.E, ""),
				//new ButtonInfo(DDInput.F, ""),
				new ButtonInfo(DDInput.L, "会話スキップ"),
				new ButtonInfo(DDInput.R, "当たり判定表示(デバッグ用)"),
				new ButtonInfo(DDInput.PAUSE, "ポーズボタン"),
				//new ButtonInfo(DDInput.START, ""),

				// < アプリ固有の設定
#endif
			};

			foreach (ButtonInfo btnInfo in btnInfos)
				btnInfo.Button.Backup();

			bool? mouseDispModeBk = null;

			try
			{
				if (keyMode)
				{
					mouseDispModeBk = DDUtils.GetMouseDispMode();
					DDUtils.SetMouseDispMode(true);

					foreach (ButtonInfo btnInfo in btnInfos)
						btnInfo.Button.KeyId = -1;

					DDCurtain.SetCurtain();
					DDEngine.FreezeInput();

					int currBtnIndex = 0;

					while (currBtnIndex < btnInfos.Length)
					{
						if (DDMouse.R.GetInput() == -1)
						{
							return;
						}
						if (DDMouse.L.GetInput() == -1)
						{
							currBtnIndex++;
							goto endInput;
						}

						{
							int pressKeyId = -1;

							foreach (KeyInfo keyInfo in KeyInfos)
								if (DDKey.GetInput(keyInfo.KeyId) == 1)
									pressKeyId = keyInfo.KeyId;

							for (int c = 0; c < currBtnIndex; c++)
								if (btnInfos[c].Button.KeyId == pressKeyId)
									pressKeyId = -1;

							if (pressKeyId != -1)
							{
								btnInfos[currBtnIndex].Button.KeyId = pressKeyId;
								currBtnIndex++;
							}
						}
					endInput:

						this.DrawWall();

						if (this.Color != null)
							DDPrint.SetColor(this.Color.Value);

						if (this.BorderColor != null)
							DDPrint.SetBorder(this.BorderColor.Value);

						DDPrint.SetPrint(this.X, this.Y, this.YStep);
						//DDPrint.SetPrint(16, 16, 32); // old
						DDPrint.PrintLine("キーボードのキー設定");

						for (int c = 0; c < btnInfos.Length; c++)
						{
							DDPrint.Print(string.Format("[{0}] {1}", currBtnIndex == c ? ">" : " ", btnInfos[c].Name));

							if (c < currBtnIndex)
							{
								int keyId = btnInfos[c].Button.KeyId;

								DDPrint.Print("　->　");

								if (keyId == -1)
									DDPrint.Print("割り当てナシ");
								else
									DDPrint.Print(KeyInfos.First(keyInfo => keyInfo.KeyId == keyId).Name);
							}
							DDPrint.PrintRet();
						}
						DDPrint.PrintLine("★　カーソルの指す機能に割り当てるキーを押して下さい。");
						DDPrint.PrintLine("★　画面を左クリックするとキーの割り当てをスキップします。(非推奨)");
						DDPrint.PrintLine("★　画面を右クリックするとキャンセルします。");

						DDEngine.EachFrame();
					}
				}
				else
				{
					foreach (ButtonInfo btnInfo in btnInfos)
						btnInfo.Button.BtnId = -1;

					DDCurtain.SetCurtain();
					DDEngine.FreezeInput();

					int currBtnIndex = 0;

					while (currBtnIndex < btnInfos.Length)
					{
						if (DDKey.GetInput(DX.KEY_INPUT_SPACE) == 1)
						{
							return;
						}
						if (DDKey.GetInput(DX.KEY_INPUT_Z) == 1)
						{
							currBtnIndex++;
							goto endInput;
						}

						{
							int pressBtnId = -1;

							for (int padId = 0; padId < DDPad.GetPadCount(); padId++)
								for (int btnId = 0; btnId < DDPad.PAD_BUTTON_MAX; btnId++)
									if (DDPad.GetInput(padId, btnId) == 1)
										pressBtnId = btnId;

							for (int c = 0; c < currBtnIndex; c++)
								if (btnInfos[c].Button.BtnId == pressBtnId)
									pressBtnId = -1;

							if (pressBtnId != -1)
							{
								btnInfos[currBtnIndex].Button.BtnId = pressBtnId;
								currBtnIndex++;
							}
						}
					endInput:

						this.DrawWall();

						if (this.Color != null)
							DDPrint.SetColor(this.Color.Value);

						if (this.BorderColor != null)
							DDPrint.SetBorder(this.BorderColor.Value);

						DDPrint.SetPrint(this.X, this.Y, this.YStep);
						//DDPrint.SetPrint(16, 16, 32); // old
						DDPrint.PrintLine("ゲームパッドのボタン設定");

						for (int c = 0; c < btnInfos.Length; c++)
						{
							DDPrint.Print(string.Format("[{0}] {1}", currBtnIndex == c ? ">" : " ", btnInfos[c].Name));

							if (c < currBtnIndex)
							{
								int btnId = btnInfos[c].Button.BtnId;

								DDPrint.Print("　->　");

								if (btnId == -1)
									DDPrint.Print("割り当てナシ");
								else
									DDPrint.Print("" + btnId);
							}
							DDPrint.PrintRet();
						}
						DDPrint.PrintLine("★　カーソルの指す機能に割り当てるボタンを押して下さい。");
						DDPrint.PrintLine("★　[Z]キーを押すとボタンの割り当てをスキップします。");
						DDPrint.PrintLine("★　スペースキーを押すとキャンセルします。");

						if (this.MouseUsable)
						{
							DDPrint.PrintLine("★　右クリックするとキャンセルします。");

							if (DDMouse.R.GetInput() == -1)
							{
								return;
							}
						}

						DDEngine.EachFrame();
					}
				}
				btnInfos = null;

				// 最後の画面を維持
				{
					DDMain.KeepMainScreen();

					for (int c = 0; c < 30; c++)
					{
						DDDraw.DrawSimple(DDGround.KeptMainScreen.ToPicture(), 0, 0);
						DDEngine.EachFrame();
					}
				}
			}
			finally
			{
				if (btnInfos != null)
					foreach (ButtonInfo info in btnInfos)
						info.Button.Restore();

				if (mouseDispModeBk != null)
					DDUtils.SetMouseDispMode(mouseDispModeBk.Value);

				DDEngine.FreezeInput();
			}
		}

		private static int WinSzExp(int size, int index)
		{
			return (size * (8 + index)) / 8;
		}

		public void WindowSizeConfig()
		{
			string[] items = new string[]
			{
				string.Format("{0} x {1} (デフォルト)", DDConsts.Screen_W, DDConsts.Screen_H),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  1), WinSzExp(DDConsts.Screen_H,  1)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  2), WinSzExp(DDConsts.Screen_H,  2)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  3), WinSzExp(DDConsts.Screen_H,  3)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  4), WinSzExp(DDConsts.Screen_H,  4)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  5), WinSzExp(DDConsts.Screen_H,  5)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  6), WinSzExp(DDConsts.Screen_H,  6)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  7), WinSzExp(DDConsts.Screen_H,  7)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  8), WinSzExp(DDConsts.Screen_H,  8)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W,  9), WinSzExp(DDConsts.Screen_H,  9)),
				string.Format("{0} x {1}", WinSzExp(DDConsts.Screen_W, 10), WinSzExp(DDConsts.Screen_H, 10)),
				"フルスクリーン 画面に合わせる (非推奨)",
				"フルスクリーン 縦横比を維持する (推奨)",
				"戻る",
			};

			int selectIndex = 0;

			for (; ; )
			{
				selectIndex = Perform("ウィンドウサイズ設定", items, selectIndex);

				switch (selectIndex)
				{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
						DDMain.SetScreenSize(WinSzExp(DDConsts.Screen_W, selectIndex), WinSzExp(DDConsts.Screen_H, selectIndex));
						break;

					case 11:
						DDMain.SetScreenSize(DDGround.MonitorRect.W, DDGround.MonitorRect.H);
						break;

					case 12:
						DDMain.SetFullScreen();
						break;

					case 13:
						goto endLoop;

					default:
						throw new DDError();
				}
			}
		endLoop:
			;
		}

		public int IntVolumeConfig(string title, int value, int minval, int maxval, int valStep, int valFastStep, Action<int> valChanged, Action pulse)
		{
			const int PULSE_FRM = 60;

			int origval = value;

			DDCurtain.SetCurtain();
			DDEngine.FreezeInput();

			for (; ; )
			{
				bool chgval = false;

				if (DDInput.A.GetInput() == 1 || this.MouseUsable && DDMouse.L.GetInput() == -1)
				{
					break;
				}
				if (DDInput.B.GetInput() == 1 || this.MouseUsable && DDMouse.R.GetInput() == -1)
				{
					if (value == origval)
						break;

					value = origval;
					chgval = true;
				}
				if (this.MouseUsable)
				{
					value += DDMouse.Rot;
					chgval = true;
				}
				if (DDInput.DIR_8.IsPound())
				{
					value += valFastStep;
					chgval = true;
				}
				if (DDInput.DIR_6.IsPound())
				{
					value += valStep;
					chgval = true;
				}
				if (DDInput.DIR_4.IsPound())
				{
					value -= valStep;
					chgval = true;
				}
				if (DDInput.DIR_2.IsPound())
				{
					value -= valFastStep;
					chgval = true;
				}
				if (chgval)
				{
					value = SCommon.ToRange(value, minval, maxval);
					valChanged(value);
				}
				if (DDEngine.ProcFrame % PULSE_FRM == 0)
				{
					pulse();
				}

				this.DrawWall();

				if (this.Color != null)
					DDPrint.SetColor(this.Color.Value);

				if (this.BorderColor != null)
					DDPrint.SetBorder(this.BorderColor.Value);

				DDPrint.SetPrint(this.X, this.Y, this.YStep);
				DDPrint.PrintLine(title);
				DDPrint.PrintLine(string.Format("[{0}]　最小={1}　最大={2}", value, minval, maxval));

				if (this.MouseUsable)
				{
					DDPrint.PrintLine("★　ホイール上・右＝上げる");
					DDPrint.PrintLine("★　ホイール下・左＝下げる");
					DDPrint.PrintLine("★　上＝速く上げる");
					DDPrint.PrintLine("★　下＝速く下げる");
					DDPrint.PrintLine("★　調整が終わったら左クリック・決定ボタンを押して下さい。");
					DDPrint.PrintLine("★　右クリック・キャンセルボタンを押すと変更をキャンセルします。");
				}
				else
				{
					DDPrint.PrintLine("★　右＝上げる");
					DDPrint.PrintLine("★　左＝下げる");
					DDPrint.PrintLine("★　上＝速く上げる");
					DDPrint.PrintLine("★　下＝速く下げる");
					DDPrint.PrintLine("★　調整が終わったら決定ボタンを押して下さい。");
					DDPrint.PrintLine("★　キャンセルボタンを押すと変更をキャンセルします。");
				}
				DDPrint.Reset();

				DDEngine.EachFrame();
			}
			DDEngine.FreezeInput();

			return value;
		}

		public double VolumeConfig(string title, double rate, int minval, int maxval, int valStep, int valFastStep, Action<double> valChanged, Action pulse)
		{
			return VolumeValueToRate(
				IntVolumeConfig(
					title,
					RateToVolumeValue(rate, minval, maxval),
					minval,
					maxval,
					valStep,
					valFastStep,
					v => valChanged(VolumeValueToRate(v, minval, maxval)),
					pulse
					),
				minval,
				maxval
				);
		}

		private static double VolumeValueToRate(int value, int minval, int maxval)
		{
			return (double)(value - minval) / (maxval - minval);
		}

		private static int RateToVolumeValue(double rate, int minval, int maxval)
		{
			return minval + SCommon.ToInt(rate * (maxval - minval));
		}
	}
}
