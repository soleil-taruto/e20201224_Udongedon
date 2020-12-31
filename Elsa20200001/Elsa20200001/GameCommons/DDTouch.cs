using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.GameCommons
{
	public static class DDTouch
	{
		private static List<Action> A_Touches = new List<Action>();

		public static void Touch()
		{
			foreach (Action a_touch in A_Touches)
				a_touch();
		}

		public static void Add(Action a_touch)
		{
			A_Touches.Add(a_touch);
		}

		public static void Add(DDPicture picture)
		{
			Add(() => picture.GetHandle());
		}

		public static void Add(DDMusic music)
		{
			Add(() => music.Sound.GetHandle(0));
		}

		public static void Add(DDSE se)
		{
			Add(() =>
			{
				for (int index = 0; index < DDSE.HANDLE_COUNT; index++)
					se.Sound.GetHandle(index);
			});
		}

		public static void AddAllPicture()
		{
			foreach (DDPicture picture in DDPictureUtils.Pictures)
				Add(picture);
		}

		public static void AddAllMusic()
		{
			foreach (DDMusic music in DDMusicUtils.Musics)
				Add(music);
		}

		public static void AddAllSE()
		{
			foreach (DDSE se in DDSEUtils.SEList)
				Add(se);
		}
	}
}
