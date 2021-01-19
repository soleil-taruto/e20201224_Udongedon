using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.GameCommons;

namespace Charlotte
{
	public class ResourceSE
	{
		public DDSE Dummy = new DDSE(@"dat\General\muon.wav");

		public DDSE SE_PLAYERSHOT = new DDSE(@"dat\Shoot_old_Resource\beetlepancake\shot004.wav");
		public DDSE SE_KASURI = new DDSE(@"dat\Shoot_old_Resource\beetlepancake\kasuri001.wav");
		public DDSE SE_ENEMYDAMAGED = new DDSE(@"dat\Shoot_old_Resource\beetlepancake\shot003.wav");
		public DDSE SE_ENEMYKILLED = new DDSE(@"dat\小森平\explosion01.mp3");
		public DDSE SE_ITEMGOT = new DDSE(@"dat\小森平\powerup03.mp3");

		public ResourceSE()
		{
			//this.Dummy.Volume = 0.1; // 非推奨

			this.SE_PLAYERSHOT.Volume = 0.1;
			//this.SE_ENEMYDAMAGED.Volume = 0.4;
			this.SE_ENEMYKILLED.Volume = 0.3;
		}
	}
}
