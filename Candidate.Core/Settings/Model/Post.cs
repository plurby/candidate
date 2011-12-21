﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Candidate.Core.Settings.Model
{
    public class Post
    {
        [Required]
        [DisplayName("Post batch")]
        public string Batch { get; set; }
    }
}
