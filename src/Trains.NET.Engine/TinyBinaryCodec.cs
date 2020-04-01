using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trains.NET.Engine
{
    public class TinyBinaryCodec : ITrackCodec
    {
        /*
[0 Track  ][0 empty ] - Declares the current cell is empty, moves to next cell
[0 Track  ][1 track ][xxxx track dir  ] - Declares the current cell contains a particular track (as table below), moves to next cell
[1 Control][0 repeat][xxx  count      ] - Repeats the following command x times
[1 Control][1 misc  ][0    end of line] - Declares the line is over, move to next row & set col to 0
[1 Control][1 misc  ][1    swap col/row] - *Experimental*
        */
        public IEnumerable<Track> Decode(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return Enumerable.Empty<Track>();
        }
        public string Encode(IEnumerable<Track> tracks)
        {
            if (!tracks.Any()) return string.Empty;

            TinyBinaryWriter bw = new TinyBinaryWriter();

            Dictionary<int, IGrouping<int, Track>>? rowGroups = tracks.GroupBy(x => x.Row).ToDictionary(x=>x.Key, x=>x);
            int lastRow = rowGroups.Keys.Max();

            int eolCounter = 0;

            // For each row
            for(int i=0; i<lastRow; i++)
            {
                // If we don't have anything, EOL it!
                if(!rowGroups.ContainsKey(i) || !rowGroups[i].Any())
                {
                    eolCounter++;
                    continue;
                }
                
                // If we had an EOL count, output it
                if(eolCounter > 0)
                {
                    // EOL is 3 bytes
                    // Repeat is 5 + 3 EOL
                    // Only saves if we have 3 or more
                    if(eolCounter >= 3)
                    {
                        bw.WriteRepeat(eolCounter, x=> x.WriteEOL());
                    }
                    else
                    {
                        bw.WriteEOL();
                        bw.WriteEOL();
                    }
                    eolCounter = 0;
                }

                // Go through this row
                Dictionary<int, TrackDirection>? row = rowGroups[i].ToDictionary(x => x.Column, x => x.Direction);
                int lastCol = row.Keys.Max();

                TrackDirFlags lastDir = TrackDirFlags.Blank;
                int lastDirCount = 0;

                for (int c = 0; c < lastCol; c++)
                {
                    TrackDirFlags currentDir = TrackDirFlags.Blank;
                    if (row.ContainsKey(i))
                    {
                        currentDir = DirectionMap.Single(x => x.Value == row[i]).Key;
                    }
                    if(currentDir == lastDir)
                    {
                        lastDirCount++;
                    }
                    else
                    {
                        if (lastDirCount > 0)
                        {
                            WriteOut(bw, eolCounter, lastDir, lastDirCount);
                        }
                        lastDirCount = 1;
                        lastDir = currentDir;
                    }
                }
                if (lastDirCount > 0)
                {
                    WriteOut(bw, eolCounter, lastDir, lastDirCount);
                }

                bw.WriteEOL();
            }
        }

        private static void WriteOut(TinyBinaryWriter bw, int eolCounter, TrackDirFlags lastDir, int lastDirCount)
        {
            // Repeat is 5 + cmd
            // Empty is 2
            // Track is 6
            // Repeat empty if >= 4
            // Repeat track if >= 2
            if (lastDir == TrackDirFlags.Blank)
            {
                if (lastDirCount >= 4)
                {
                    bw.WriteRepeat(eolCounter, x => x.WriteEmpty());
                }
                else
                {
                    for (int j = 0; j < lastDirCount; j++)
                    {
                        bw.WriteEmpty();
                    }
                }
            }
            else
            {
                if (lastDirCount >= 2)
                {
                    bw.WriteRepeat(eolCounter, x => x.WriteTrack(lastDir));
                }
                else
                {
                    for (int j = 0; j < lastDirCount; j++)
                    {
                        bw.WriteTrack(lastDir);
                    }
                }
            }
        }

        private class TinyBinaryWriter
        {
            List<byte> stuff = new List<byte>();
            byte current = 0;
            int currentPos = 0;
            public void WriteEOL() => Write(1, 1, 0);
            public void WriteEmpty() => Write(0, 0);
            public void WriteTrack(TrackDirFlags track)
            {
                Write(0, 1, BitFromRight((int)track, 3), BitFromRight((int)track, 2), BitFromRight((int)track, 1), BitFromRight((int)track, 0));
            }
            private static int BitFromRight(int v, int index) => (v & (1 << index)) >> index;
            public void WriteRepeat(int count, Action<TinyBinaryWriter> action)
            {
                while(count > 7)
                {
                    Write(1, 0, 1, 1, 1);
                    action(this);
                    count -= 7;
                }
                if (count > 0)
                {
                    Write(1, 0, BitFromRight(count, 2), BitFromRight(count, 1), BitFromRight(count, 0));
                    action(this);
                }
            }
            private void Write(params int[] bits)
            {
                for(int i=0; i< bits.Length; i++)
                {
                    byte value = (byte)(bits[i] & 1);
                    current |= value;
                    current <<= 1;
                    if(++currentPos >= 8)
                    {
                        stuff.Add(current);
                        current = 0;
                    }
                }
            }
            public byte[] GetBytes()
            {
                if (current == 0) return stuff.ToArray();
                return stuff.Concat(new byte[] { (byte)(current << (8 - currentPos)) }).ToArray();
            }
        }

        private static readonly Dictionary<TrackDirFlags, TrackDirection> DirectionMap = new Dictionary<TrackDirFlags, TrackDirection>
        {
            {TrackDirFlags.Blank, TrackDirection.Undefined},
            {TrackDirFlags.Horizontal, TrackDirection.Horizontal},
            {TrackDirFlags.Vertical, TrackDirection.Vertical},
            {TrackDirFlags.LeftUp, TrackDirection.LeftUp},
            {TrackDirFlags.RightUp, TrackDirection.RightUp},
            {TrackDirFlags.RightDown, TrackDirection.RightDown},
            {TrackDirFlags.LeftDown, TrackDirection.LeftDown},
            {TrackDirFlags.RightUpDown, TrackDirection.RightUpDown},
            {TrackDirFlags.LeftRightDown, TrackDirection.LeftRightDown},
            {TrackDirFlags.LeftUpDown, TrackDirection.LeftUpDown},
            {TrackDirFlags.LeftRightUp, TrackDirection.LeftRightUp},
            {TrackDirFlags.Cross, TrackDirection.Cross}
        };

        private enum TrackDirFlags : ushort
        {
            Blank = 0b0000,
            Horizontal = 0b1100,
            Vertical = 0b0011,
            LeftUp = 0b1010,
            RightUp = 0b0110,
            RightDown = 0b0101,
            LeftDown = 0b1001,
            RightUpDown = 0b0111,
            LeftRightDown = 0b1101,
            LeftUpDown = 0b1011,
            LeftRightUp = 0b1110,
            Cross = 0b1111
        }
    }
}
