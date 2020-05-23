using Blamite.IO;
using System;

namespace Blamite.Blam
{
	/// <summary>
	///     Retrieves engine version information from a cache file.
	/// </summary>
	public class CacheFileVersionInfo
	{
        // first gen has a few different versions kicking around
        private const int FirstGenXboxVersion = 5;
        private const int FirstGenPCVersion = 7;// also CEA
        private const int FirstGenCustomEditionVersion = 609;
        private const int FirstGenInvaderVersion = 343;// invader tool uses version 343

        // second gen needs more checks to find the platform
		private const int SecondGenVersion = 8;

		private const int ThirdGenVersion = 9;
		private const int SecondGenMCCVersion = 10;

		public CacheFileVersionInfo(IReader reader)
		{
			reader.SeekTo(0x4);
			Version = reader.ReadInt32();

            if (Version == FirstGenPCVersion)
            {
                // first gen is all similar enough
                Engine = EngineType.FirstGeneration;

                // Read first-generation build string
                reader.SeekTo(0x40);
                BuildString = reader.ReadAscii();
            }
            else if (Version == FirstGenCustomEditionVersion)
            {
                // first gen is all similar enough
                Engine = EngineType.FirstGeneration;

                // Read first-generation build string
                reader.SeekTo(0x40);
                BuildString = reader.ReadAscii();
            }
            else if (Version == FirstGenInvaderVersion)
            {
                // first gen is all similar enough
                Engine = EngineType.FirstGeneration;

                // Read first-generation build string
                reader.SeekTo(0x40);
                BuildString = reader.ReadAscii();
            }
            else if (Version == FirstGenXboxVersion)
            {
                // first gen is all similar enough
                Engine = EngineType.FirstGeneration;

                // Read first-generation build string
                reader.SeekTo(0x40);
                BuildString = reader.ReadAscii();
            }
			else if (Version == SecondGenVersion || Version == SecondGenMCCVersion)
			{
                Engine = EngineType.SecondGeneration;

                // turns out H2X doesnt use the meta offset mask
                // but neither do vista campaign maps
                // thankfully vista sets some U32 to 0xFFFFFFFF
                reader.SeekTo(0x20);
                uint metaOffsetMask = reader.ReadUInt32();
                uint unkNull = reader.ReadUInt32();

                // Read second-generation build string
                // are we loading an xbox cache?
                if (metaOffsetMask == 0 && unkNull != 0xFFFFFFFF)
                    reader.SeekTo(0x120);
                else  // probably a vista cache
                    reader.SeekTo(0x12C);

                BuildString = reader.ReadAscii();
            }
			else if (Version >= ThirdGenVersion)
			{
				Engine = EngineType.ThirdGeneration;

				// Read third-generation build string
				if (reader.Endianness == Endian.BigEndian)
					reader.SeekTo(0x11C);
				else
					reader.SeekTo(0x120);

				BuildString = reader.ReadAscii();
			}
		}

		/// <summary>
		///     The version number stored in the file header (if there is one).
		/// </summary>
		public int Version { get; private set; }

		/// <summary>
		///     The engine type the map was built for.
		/// </summary>
		public EngineType Engine { get; private set; }

		/// <summary>
		///     The engine build version string stored in the file.
		/// </summary>
		public string BuildString { get; private set; }
	}
}