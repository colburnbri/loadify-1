﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Id3;
using Id3.Frames;

namespace loadify.Spotify
{
    public class Mp3FileDescriptor : AudioFileDescriptor
    {
        public Mp3FileDescriptor(AudioFileMetaData audioFileMetaData):
            base(audioFileMetaData)
        { }

        public override void Write(string inputFilePath)
        {
            using (var fileStream = new FileStream(inputFilePath, FileMode.Open))
            {
                using (var mp3 = new Mp3Stream(fileStream, Mp3Permissions.ReadWrite))
                {
                    mp3.DeleteAllTags(); // make sure the file got no tags
                    var id3Tag = new Id3Tag();
                    id3Tag.Title.Value = Data.Title;
                    id3Tag.Artists.Value = Data.Artists;
                    id3Tag.Album.Value = Data.Album;
                    id3Tag.Year.Value = Data.Year.ToString();
                    id3Tag.Pictures.Add(new PictureFrame() { EncodingType = Id3TextEncoding.Iso8859_1, MimeType = "image/png", PictureType = PictureType.FrontCover, PictureData = Data.Cover });
                    mp3.WriteTag(id3Tag, 1, 0, WriteConflictAction.NoAction);
                }
            }
        }

        public override void Read(string inputFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
