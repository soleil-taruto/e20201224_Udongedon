using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.GameCommons
{
	/// <summary>
	/// 特定の或いは全てのリソースをロードする。
	/// 泥縄式にリソースをロードすることによって処理落ちする場合があるため、予めロードしておく。
	/// </summary>
	public static class DDTouch
	{
		private static List<Action> A_Touches = new List<Action>();

		/// <summary>
		/// リソースをロードする。
		/// 以下のタイミングで実行すること。
		/// -- ゲーム開始時
		/// -- リソースを開放した後
		/// </summary>
		public static void Touch()
		{
			foreach (Action a_touch in A_Touches)
				a_touch();
		}

		/// <summary>
		/// 任意のロード手続きを追加する。
		/// 画像・音楽・効果音以外のリソースのロードなども想定する。
		/// </summary>
		/// <param name="a_touch">ロード手続き</param>
		public static void Add(Action a_touch)
		{
			A_Touches.Add(a_touch);
		}

		/// <summary>
		/// 画像リソースを追加する。
		/// </summary>
		/// <param name="picture">画像リソース</param>
		public static void Add(DDPicture picture)
		{
			Add(() => picture.GetHandle());
		}

		/// <summary>
		/// 音楽リソースを追加する。
		/// </summary>
		/// <param name="music">音楽リソース</param>
		public static void Add(DDMusic music)
		{
			Add(() => music.Sound.GetHandle(0));
		}

		/// <summary>
		/// 効果音リソースを追加する。
		/// </summary>
		/// <param name="se">効果音リソース</param>
		public static void Add(DDSE se)
		{
			Add(() =>
			{
				for (int index = 0; index < DDSE.HANDLE_COUNT; index++)
					se.Sound.GetHandle(index);
			});
		}

		/// <summary>
		/// 全ての画像リソースを追加する。
		/// これまでに DDCCResource.GetPicture() した画像も含むことに注意
		/// </summary>
		public static void AddAllPicture()
		{
			foreach (DDPicture picture in DDPictureUtils.Pictures.Concat(DDDerivationUtils.Derivations))
				Add(picture);
		}

		/// <summary>
		/// 全ての音楽リソースを追加する。
		/// これまでに DDCCResource.GetMusic() した音楽も含むことに注意
		/// </summary>
		public static void AddAllMusic()
		{
			foreach (DDMusic music in DDMusicUtils.Musics)
				Add(music);
		}

		/// <summary>
		/// 全ての効果音リソースを追加する。
		/// これまでに DDCCResource.GetSE() した効果音も含むことに注意
		/// </summary>
		public static void AddAllSE()
		{
			foreach (DDSE se in DDSEUtils.SEList)
				Add(se);
		}
	}
}
