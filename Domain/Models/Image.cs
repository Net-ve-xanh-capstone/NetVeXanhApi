﻿using Domain.Models.Base;

namespace Domain.Models;

public class Image : BaseModel
{
    public string Url { get; set; }
    public string? Description { get; set; }
    public Guid PostId { get; set; }

    //Relation
    public Post Post { get; set; }
}