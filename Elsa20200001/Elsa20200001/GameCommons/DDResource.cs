using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Commons;

namespace Charlotte.GameCommons
{
	public static class DDResource
	{
		private static bool ReleaseMode = false;
		private static string ResourceDir;

		private class ResInfo
		{
			public string ResFile;
			public long Offset;
			public int Size;
		}

		private static Dictionary<string, ResInfo> File2ResInfo = SCommon.CreateDictionaryIgnoreCase<ResInfo>();

		public static void INIT()
		{
			if (File.Exists(DDConsts.ResourceFile)) // ? 外部リリース
			{
				ReleaseMode = true;
			}
			else if (Directory.Exists(DDConsts.ResourceDir_InternalRelease)) // ? 内部リリース
			{
				ResourceDir = DDConsts.ResourceDir_InternalRelease;
			}
			else // ? 開発環境
			{
				ResourceDir = DDConsts.ResourceDir_DevEnv;
			}

			if (ReleaseMode)
			{
				List<ResInfo> resInfos = new List<ResInfo>();

				using (FileStream reader = new FileStream(DDConsts.ResourceFile, FileMode.Open, FileAccess.Read))
				{
					while (reader.Position < reader.Length)
					{
						int size = SCommon.ToInt(SCommon.Read(reader, 4));

						if (size < 0)
							throw new DDError();

						resInfos.Add(new ResInfo()
						{
							ResFile = DDConsts.ResourceFile,
							Offset = reader.Position,
							Size = size,
						});

						reader.Seek((long)size, SeekOrigin.Current);
					}
				}
				string[] files = SCommon.TextToLines(SCommon.ENCODING_SJIS.GetString(LoadFile(resInfos[0])));

				if (files.Length != resInfos.Count)
					throw new DDError(files.Length + ", " + resInfos.Count);

				for (int index = 1; index < files.Length; index++)
				{
					string file = files[index];

					if (File2ResInfo.ContainsKey(file))
						throw new DDError(file);

					File2ResInfo.Add(file, resInfos[index]);
				}
			}
		}

		private static byte[] LoadFile(string resFile, long offset, int size)
		{
			using (FileStream reader = new FileStream(resFile, FileMode.Open, FileAccess.Read))
			{
				reader.Seek(offset, SeekOrigin.Begin);

				return DDJammer.Decode(SCommon.Read(reader, size));
			}
		}

		private static byte[] LoadFile(ResInfo resInfo)
		{
			return LoadFile(resInfo.ResFile, resInfo.Offset, resInfo.Size);
		}

		public static byte[] Load(string file)
		{
			if (ReleaseMode)
			{
				return LoadFile(File2ResInfo[file]);
			}
			else
			{
				string datFile = Path.Combine(ResourceDir, file);

				if (!File.Exists(datFile))
					throw new Exception(datFile);

				return File.ReadAllBytes(datFile);
			}
		}

		public static void Save(string file, byte[] fileData)
		{
			if (ReleaseMode)
			{
				throw new DDError();
			}
			else
			{
				File.WriteAllBytes(Path.Combine(ResourceDir, file), fileData);
			}
		}

		/// <summary>
		/// <para>ファイルリストを取得する。</para>
		/// <para>ソート済み</para>
		/// <para>'_' で始まるファイルの除去済み</para>
		/// </summary>
		/// <returns>ファイルリスト</returns>
		public static IEnumerable<string> GetFiles()
		{
			IEnumerable<string> files;

			if (ReleaseMode)
			{
				files = File2ResInfo.Keys;
			}
			else
			{
				files = Directory.GetFiles(ResourceDir, "*", SearchOption.AllDirectories).Select(file => SCommon.ChangeRoot(file, ResourceDir));

				// '_' で始まるファイルの除去
				// makeDDResourceFile は '_' で始まるファイルを含めない。
				files = files.Where(file => Path.GetFileName(file)[0] != '_');
			}

			// ソート
			// makeDDResourceFile はファイルリストを sortJLinesICase している。
			// ここでソートする必要は無いが、戻り値に統一性を持たせるため(毎回ファイルの並びが違うということのないように)ソートしておく。
			files = SCommon.Sort(files, SCommon.CompIgnoreCase);

			return files;
		}
	}
}
