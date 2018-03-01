﻿using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace DigiSigner.Client
{
    public class DocumentField
    {
        public DocumentField()
        {
            ReadOnly = false;
            PdfField = false;
        }

        [JsonProperty("api_id")]
        public string apiId
        {
            get; set;
        }

        [JsonProperty("role")]
        public String Role
        {
            get; set;
        }

        [JsonProperty("type")]
        public FieldType Type
        {
            get; set;
        }

        [JsonProperty("page")]
        public int page  // starts with 0
        {
            get; set;
        }

        [JsonProperty("rectangle")]
        public int[] rectangle
        {
            get; set;
        }

        [JsonProperty("status")]
        public DocumentFieldStatus Status
        {
            get; set;
        }

        [JsonProperty("content")]
        public String content;

        [JsonProperty("submitted_content")]
        public String submittedContent
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

        [JsonProperty("name")]
        public String Name
        {
            get; set;
        }

        [JsonProperty("index")]
        public int Index  // relevant only for check boxes
        {
            get; set;
        }

        
        [JsonProperty("read_only")]
        public bool ReadOnly
        {
            get;
            set;
        }

        [JsonProperty("pdf_field")]
        public bool PdfField
        {
            get; set;
        }

        [JsonProperty("font_size")]
        private float FontSize
        {
            get; set;
        }


        [JsonProperty("max_length")]
        public int MaxLength
        {
            get; set;
        }

        [JsonProperty("alignment")]
        public DocumentFieldAlignment Alignment
        {
            get; set;
        }
    }
}