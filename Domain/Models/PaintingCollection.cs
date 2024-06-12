﻿namespace Domain.Models;

public class PaintingCollection
{
    public int Id { get; set; }
    public Guid? PaintingId { get; set; }

    public Guid? CollectionId { get; set; }

    //Relation
    public Collection Collection { get; set; }

    public Painting Painting { get; set; }
}