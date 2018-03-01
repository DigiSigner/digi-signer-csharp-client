﻿using System;

using Newtonsoft.Json;

namespace DigiSigner.Client
{
    public class Signature
    {
        public Signature()
        {
        }

        [JsonProperty("page")]
        public int Page
        {
            set;
            get;
        }

        [JsonProperty("rectangle")]
        public int[] Rectangle
        {
            set;
            get;
        }

        [JsonProperty("image")]
        public byte[] Image
        {
            set;
            get;
        }

        [JsonProperty("draw_coordinates")]
        public String DrawCoordinates
        {
            get;
            set;
        }
    }
}