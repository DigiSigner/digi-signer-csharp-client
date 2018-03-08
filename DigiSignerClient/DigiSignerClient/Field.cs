﻿using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace DigiSigner.Client
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Field
    {
        [JsonProperty("page")]
        public int Page
        {
            get; set;
        }


        [JsonProperty("rectangle")]
        private int[] Rectangle
        {
            get; set;
        }

        [JsonProperty("type")]
        public FieldType Type
        {
            get; set;
        }

        [JsonProperty("name")]
        public string Name
        {
            get; set;
        }

        [JsonProperty("api_id")]
        public string ApiId
        {
            get; set;
        }

        [JsonProperty("content")]
        public string Content
        {
            get; set;
        }

        [JsonProperty("label")]
        public String Label
        {
            get; set;
        }

        [JsonProperty("required")]
        public bool Required
        {
            get; set;
        }

        [JsonProperty("readonly")]
        public bool ReadOnly
        {
            get; set;
        }


        public Field()
        {
            Required = true;
        }

        public Field(int page, int[] rectangle, FieldType type)
        {
            Required = true;

            Page = page;
            Rectangle = rectangle;
            Type = type;
        }

        public Field(int page, int[] rectangle, FieldType type, string label, bool required)
        {
            Page = page;
            Rectangle = rectangle;
            Type = type;
            Label = label;
            Required = required;
        }
    }
}
