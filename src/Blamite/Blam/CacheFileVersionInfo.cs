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

		private const int SecondGenVersion = 8;
		private const int ThirdGenVersion = 9;

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
            // TODO: support xbox
            //       gonna need de/compression
            else if (Version == FirstGenXboxVersion)
            {
                throw new NotImplementedException("assembly does not support loading first gen xbox maps (yet)");
            }
			else if (Version == SecondGenVersion)
			{
				Engine = EngineType.SecondGeneration;

                // TODO: need to check if this is an xbox cache or PC cache
                //       second gen uses the same version but different layout
                //       which makes it a lil more annoying but still workable

				// Read second-generation build string
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